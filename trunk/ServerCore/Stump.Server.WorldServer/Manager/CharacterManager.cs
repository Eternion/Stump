
using System;
using System.Collections.Generic;
using System.Linq;
using Castle.ActiveRecord;
using Stump.Database;
using Stump.Database.WorldServer;
using Stump.DofusProtocol.Classes.Custom;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.IPC;

namespace Stump.Server.WorldServer.Manager
{
    public static class CharacterManager
    {
        public static List<CharacterRecord> GetCharactersByAccount(WorldClient client)
        {
            var characters = new List<CharacterRecord>();
            var ids = client.Account.GetWorldCharactersId(WorldServer.ServerInformation.Id)            ;
            
            characters.AddRange(
                ids.Select(delegate(uint id)
                {
                    try
                    {
                        return CharacterRecord.FindCharacterById((int)id);
                    }
                    catch (NotFoundException)
                    {
                        // character do not exist, then we remove it from the auth database
                        World.Instance.TaskPool.EnqueueTask(() =>
                                                            IpcAccessor.Instance.ProxyObject.DeleteAccountCharacter(
                                                                WorldServer.ServerInformation,
                                                                client.Account.Id,
                                                                id));
                        return null;
                    }
                }).Where(character => character != null));

            return characters;
        }

        public static bool CreateCharacterOnAccount(CharacterRecord character, WorldClient client)
        {
            if (client.Characters == null)
                client.Characters = new List<CharacterRecord>();

            character.Create();
            client.Characters.Insert(0, character);

            World.Instance.TaskPool.EnqueueTask(() => IpcAccessor.Instance.ProxyObject.AddAccountCharacter(WorldServer.ServerInformation,
                                                                                                           client.Account.Id,
                                                                                                           (uint)character.Id));
            return true;
        }

        public static void DeleteCharacterOnAccount(CharacterRecord character, WorldClient client)
        {
            client.Characters.Remove(character);

            World.Instance.TaskPool.EnqueueTask(() => IpcAccessor.Instance.ProxyObject.DeleteAccountCharacter(WorldServer.ServerInformation,
                                                                                                              client.Account.Id,
                                                                                                              (uint)character.Id));
        }

        public static int GetAccountDeletedCharactersNumber(uint accountId)
        {
            return IpcAccessor.Instance.ProxyObject.GetDeletedCharactersNumber(accountId);
        }

        public static ExtendedLook GetStuffedCharacterLook(CharacterRecord character)
        {
            var baseLook = new ExtendedLook(character.BaseLook);

            //TODO ADD ITEMS

            return baseLook;
        }


        #region Character Name Random Generation

        private const string Vowels = "aeiouy";

        private const string Consonants = "bcdfghjklmnpqrstvwxz";

        public static string GenerateName()
        {
            string name;

            do
            {
                var rand = new Random();
                int namelen = rand.Next(5, 10);
                name = string.Empty;

                name += char.ToUpper(RandomConsonant(rand));

                for (int i = 0; i < namelen - 1; i++)
                {
                    name += ((i & 1) != 1) ? RandomConsonant(rand) : RandomVowel(rand);
                }
            } while (CharacterRecord.IsNameExists(name));

            return name;
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