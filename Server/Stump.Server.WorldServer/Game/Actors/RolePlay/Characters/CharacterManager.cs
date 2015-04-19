using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.IO;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.IPC.Messages;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Database.Shortcuts;
using Stump.Server.WorldServer.Game.Breeds;
using Stump.Server.WorldServer.Game.Guilds;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Characters
{
    public class CharacterManager : DataManager<CharacterManager>
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Called before the record is saved
        /// </summary>
        public event Action<CharacterRecord> CreatingCharacter;

        private void OnCreatingCharacter(CharacterRecord record)
        {
            var handler = CreatingCharacter;
            if (handler != null) handler(record);
        }

        /// <summary>
        ///   Maximum number of characters you can create/store in your account
        /// </summary>
        [Variable(true)] public static uint MaxCharacterSlot = 5;

        private static readonly Regex m_nameCheckerRegex = new Regex(
            "^[A-Z][a-z]{2,9}(?:-[A-Za-z][a-z]{2,9}|[a-z]{1,10})$", RegexOptions.Compiled);

        public CharacterRecord GetCharacterById(int id)
        {
            return Database.Query<CharacterRecord>(string.Format(CharacterRelator.FetchById, id)).FirstOrDefault();
        }

        public CharacterRecord GetCharacterByName(string name)
        {
            return Database.Query<CharacterRecord>(CharacterRelator.FetchByName, name).FirstOrDefault();
        }

        public List<CharacterRecord> GetCharactersByAccount(WorldClient client)
        {
            if (client.Account.Characters == null ||
                client.Account.Characters.Count == 0)
                return new List<CharacterRecord>();

            var characterIds =
                client.Account.Characters.Where(x => x.WorldId == WorldServer.ServerInformation.Id)
                      .Select(x => x.CharacterId).ToList();

            if (characterIds.Count == 0)
                return new List<CharacterRecord>();

            var characters = Database.Fetch<CharacterRecord>(string.Format(CharacterRelator.FetchByMultipleId, characterIds.ToCSV(",")));

            if (characters.Count == client.Account.Characters.Count)
                return characters;

            // delete characters that doesn't exist anymore
            foreach (var id in characterIds.Where(id => characters.All(character => character.Id != id)).Where(id => IPCAccessor.Instance.IsConnected))
            {
                IPCAccessor.Instance.Send(new DeleteCharacterMessage(client.Account.Id, id));
            }

            return characters;
        }

        /*public CharacterRecord GetCharacterById(int id)
        {
            var character = Database.Fetch<CharacterRecord>(string.Format(CharacterRelator.FetchByMultipleId, id));

            return character.Count == 0 ? null : character.FirstOrDefault();
        }*/

        public bool DoesNameExist(string name)
        {
            return Database.ExecuteScalar<object>("SELECT 1 FROM characters WHERE Name=@0", name) != null;
        }

        public void CreateCharacter(WorldClient client, string name, sbyte breedId, bool sex,
                                                           IEnumerable<int> colors, int headId, Action successCallback, Action<CharacterCreationResultEnum> failCallback)
        {
            if (client.Characters.Count >= MaxCharacterSlot && client.UserGroup.Role <= RoleEnum.Player)
            {
                failCallback(CharacterCreationResultEnum.ERR_TOO_MANY_CHARACTERS);
                return;
            }

            if (DoesNameExist(name))
            {
                failCallback(CharacterCreationResultEnum.ERR_NAME_ALREADY_EXISTS);
                return;
            }

            if (!m_nameCheckerRegex.IsMatch(name))
            {
                failCallback(CharacterCreationResultEnum.ERR_INVALID_NAME);
                return;
            }

            var breed = BreedManager.Instance.GetBreed(breedId);

            if (breed == null ||
                !client.Account.CanUseBreed(breedId) || !BreedManager.Instance.IsBreedAvailable(breedId))
            {
                failCallback(CharacterCreationResultEnum.ERR_NOT_ALLOWED);
                return;
            }

            var head = BreedManager.Instance.GetHead(headId);

            if (head.Breed != breedId || head.Gender == 1 != sex)
            {
                failCallback(CharacterCreationResultEnum.ERR_NO_REASON);
                return;
            }

            var look = breed.GetLook(sex ? SexTypeEnum.SEX_FEMALE : SexTypeEnum.SEX_MALE, true);
            var i = 0;
            var breedColors = !sex ? breed.MaleColors : breed.FemaleColors;
            foreach (var color in colors)
            {
                if (breedColors.Length > i)
                {
                    look.AddColor(i + 1, color == -1 ? Color.FromArgb((int) breedColors[i]) : Color.FromArgb(color));
                }

                i++;
            }

            foreach (var skin in head.Skins)
                look.AddSkin(skin);

            CharacterRecord record;
            using (var transaction = Database.GetTransaction())
            {
                record = new CharacterRecord(breed)
                    {
                        Experience = ExperienceManager.Instance.GetCharacterLevelExperience(breed.StartLevel),
                        Name = name,
                        Sex = sex ? SexTypeEnum.SEX_FEMALE : SexTypeEnum.SEX_MALE,
                        Head = headId,
                        EntityLook = look,
                        CreationDate = DateTime.Now,
                        LastUsage = DateTime.Now,
                        AlignmentSide = AlignmentSideEnum.ALIGNMENT_NEUTRAL,
                        WarnOnConnection = true,
                        WarnOnLevel = true,
                    };

                Database.Insert(record);

                // add items here

                var spellsToLearn = from spell in breed.Spells
                                    where spell.ObtainLevel <= breed.StartLevel
                                    orderby spell.ObtainLevel , spell.Spell ascending
                                    select spell;

                var slot = 0;
                foreach (var spellRecord in spellsToLearn.Select(learnableSpell => SpellManager.Instance.CreateSpellRecord(record,
                    SpellManager.Instance.
                        GetSpellTemplate(
                            learnableSpell.
                                Spell))))
                {
                    Database.Insert(spellRecord);

                    var shortcut = new SpellShortcut(record, slot, (short) spellRecord.SpellId);
                    Database.Insert(shortcut);
                    slot++;
                }

                foreach (var itemRecord in breed.Items.Select(startItem => startItem.GenerateItemRecord(record)))
                {
                    Database.Insert(itemRecord);
                }

                OnCreatingCharacter(record);

                if (client.Characters == null)
                    client.Characters = new List<CharacterRecord>();

                client.Characters.Insert(0, record);
                transaction.Complete();
            }

            IPCAccessor.Instance.SendRequest(new AddCharacterMessage(client.Account.Id, record.Id),
                                             x => successCallback(),
                                             x =>
                                                 {
                                                     // todo cascade
                                                     Database.Delete(record);
                                                     failCallback(CharacterCreationResultEnum.ERR_NO_REASON);
                                                 });
            ;

            logger.Debug("Character {0} created", record.Name);
        }

        public void DeleteCharacterOnAccount(CharacterRecord character, WorldClient client)
        {   
            // todo cascade
            var guildMember = GuildManager.Instance.TryGetGuildMember(character.Id);

            if (guildMember != null)
                GuildManager.Instance.DeleteGuildMember(guildMember);

            Database.Delete(character);
            client.Characters.Remove(character);

            // no check needed
            IPCAccessor.Instance.Send(new DeleteCharacterMessage(client.Account.Id, character.Id));
        }

        #region Character Name Random Generation

        private const string Vowels = "aeiouy";
        private const string Consonants = "bcdfghjklmnpqrstvwxz";

        public string GenerateName()
        {
            var rand = new Random();
            var namelen = rand.Next(5, 10);
            var name = string.Empty;

            var vowel = rand.Next(0, 2) == 0;
            name += GetChar(vowel, rand).ToString(CultureInfo.InvariantCulture).ToUpper();
            vowel = !vowel;

            for (var i = 0; i < namelen - 1; i++)
            {
                name += GetChar(vowel, rand);
                vowel = !vowel;
            }

            /*do
            {
                var rand = new Random();
                var namelen = rand.Next(5, 10);
                name = string.Empty;

                var vowel = rand.Next(0, 2) == 0;
                name += GetChar(vowel, rand).ToString(CultureInfo.InvariantCulture).ToUpper();
                vowel = !vowel;

                for (var i = 0; i < namelen - 1; i++)
                {
                    name += GetChar(vowel, rand);
                    vowel = !vowel;
                }
            } while (DoesNameExist(name));*/

            return name;
        }

        private static char GetChar(bool vowel, Random rand)
        {
            return vowel ? RandomVowel(rand) : RandomConsonant(rand);
        }

        private static char RandomVowel(Random rand)
        {
            return Vowels[rand.Next(0, Vowels.Length - 1)];
        }

        private static char RandomConsonant(Random rand)
        {
            return Consonants[rand.Next(0, Consonants.Length - 1)];
        }

        #endregion
    }
}