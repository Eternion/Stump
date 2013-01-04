using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.IO;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.DofusProtocol.Types.Extensions;
using Stump.ORM;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Breeds;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Shortcuts;
using Stump.Server.WorldServer.Game.Breeds;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay.Characters
{
    public class CharacterManager : DataManager<CharacterManager>
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///   Maximum number of characters you can create/store in your account
        /// </summary>
        [Variable(true)] public static uint MaxCharacterSlot = 5;

        private static readonly Regex m_nameCheckerRegex = new Regex(
            "^[A-Z][a-z]{2,9}(?:-[A-Z][a-z]{2,9}|[a-z]{1,10})$", RegexOptions.Compiled);

        public List<CharacterRecord> GetCharactersByAccount(WorldClient client)
        {
            var characters =
                Database.Fetch<CharacterRecord>(string.Format(CharacterRelator.FetchByMultipleId,client.Account.CharactersId.ToCSV(",")));

            if (characters.Count != client.Account.CharactersId.Count)
            {
                foreach (
                    int characterId in
                        client.Account.CharactersId.Where(id => characters.All(character => character.Id != id)))
                {
                    // character do not exist, then we remove it from the auth database
                    int id = characterId;
                    WorldServer.Instance.IOTaskPool.AddMessage(() =>
                                                               IpcAccessor.Instance.ProxyObject.DeleteAccountCharacter(
                                                                   client.Account.Id, id));
                }
            }

            return characters;
        }

        public bool DoesNameExist(string name)
        {
            return Database.ExecuteScalar<bool>("SELECT EXISTS(SELECT 1 FROM characters WHERE Name=@0)", name);
        }

        public CharacterCreationResultEnum CreateCharacter(WorldClient client, string name, sbyte breedId, bool sex,
                                                           IEnumerable<int> colors, int headId)
        {
            if (client.Characters.Count >= MaxCharacterSlot)
                return CharacterCreationResultEnum.ERR_TOO_MANY_CHARACTERS;

            if (DoesNameExist(name))
                return CharacterCreationResultEnum.ERR_NAME_ALREADY_EXISTS;

            if (!m_nameCheckerRegex.IsMatch(name))
                return CharacterCreationResultEnum.ERR_INVALID_NAME;

            var breed = BreedManager.Instance.GetBreed(breedId);

            if (breed == null ||
                !client.Account.CanUseBreed(breedId) || !BreedManager.Instance.IsBreedAvailable(breedId))
                return CharacterCreationResultEnum.ERR_NOT_ALLOWED;

            var head = BreedManager.Instance.GetHead(headId);

            if (head.Breed != breedId || head.Gender == 1 != sex)
                return CharacterCreationResultEnum.ERR_NO_REASON;

            var indexedColors = new List<int>();
            int i = 0;
            foreach (int color in colors)
            {
                uint[] breedColors = !sex ? breed.MaleColors : breed.FemaleColors;
                if (breedColors.Length > i)
                {
                    if (color == -1)
                        indexedColors.Add((i + 1) << 24 | (int) breedColors[i]);
                    else
                        indexedColors.Add((i + 1) << 24 | color);
                }

                i++;
            }

            EntityLook look = !sex ? breed.MaleLook.Copy() : breed.FemaleLook.Copy();
            look.indexedColors = indexedColors;

            CharacterRecord record;
            using (Transaction transaction = Database.GetTransaction())
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

                IOrderedEnumerable<BreedSpell> spellsToLearn = from spell in breed.Spells
                                                               where spell.ObtainLevel <= breed.StartLevel
                                                               orderby spell.ObtainLevel , spell.Spell ascending
                                                               select spell;

                int slot = 0;
                foreach (BreedSpell learnableSpell in spellsToLearn)
                {
                    CharacterSpellRecord spellRecord = SpellManager.Instance.CreateSpellRecord(record,
                                                                                               SpellManager.Instance.
                                                                                                   GetSpellTemplate(
                                                                                                       learnableSpell.
                                                                                                           Spell));
                    Database.Insert(spellRecord);

                    var shortcut = new SpellShortcut(record, slot, (short) spellRecord.SpellId);
                    Database.Insert(shortcut);
                    slot++;
                }

                foreach (BreedItem startItem in breed.Items)
                {
                    PlayerItemRecord itemRecord = startItem.GenerateItemRecord(record);
                    Database.Insert(itemRecord);
                }


                if (client.Characters == null)
                    client.Characters = new List<CharacterRecord>();

                client.Characters.Insert(0, record);
                transaction.Complete();
            }

            IpcAccessor.Instance.ProxyObject.AddAccountCharacter(client.Account.Id,
                                                                 record.Id);

            logger.Debug("Character {0} created", record.Name);


            return CharacterCreationResultEnum.OK;
        }

        public void DeleteCharacterOnAccount(CharacterRecord character, WorldClient client)
        {
            // todo casade
            Database.Delete(character);
            client.Characters.Remove(character);

            WorldServer.Instance.IOTaskPool.AddMessage(
                () => IpcAccessor.Instance.ProxyObject.DeleteAccountCharacter(client.Account.Id, character.Id));
        }

        #region Character Name Random Generation

        private const string Vowels = "aeiouy";
        private const string Consonants = "bcdfghjklmnpqrstvwxz";

        public string GenerateName()
        {
            string name;

            do
            {
                var rand = new Random();
                int namelen = rand.Next(5, 10);
                name = string.Empty;

                bool vowel = rand.Next(0, 2) == 0;
                name += GetChar(vowel, rand).ToString().ToUpper();
                vowel = !vowel;

                for (int i = 0; i < namelen - 1; i++)
                {
                    name += GetChar(vowel, rand);
                    vowel = !vowel;
                }
            } while (DoesNameExist(name));

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