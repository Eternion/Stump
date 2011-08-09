using System;
using System.Linq;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Core.Reflection;
using Stump.Server.WorldServer.Core.IPC;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Characters;

namespace Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters
{
    public class CharacterManager : Singleton<CharacterManager>
    {
        public List<CharacterRecord> GetCharactersByAccount(WorldClient client)
        {
            var characters = new List<CharacterRecord>();

            characters.AddRange(
                client.Account.CharactersId.Select(delegate(uint id)
                {
                    try
                    {
                        return CharacterRecord.FindById((int) id);
                    }
                    catch (NotFoundException)
                    {
                        // character do not exist, then we remove it from the auth database
                        WorldServer.Instance.IOTaskPool.EnqueueTask(() =>
                                                            IpcAccessor.Instance.ProxyObject.DeleteAccountCharacter(
                                                                WorldServer.ServerInformation,
                                                                client.Account.Id,
                                                                id));
                        return null;
                    }
                }).Where(character => character != null));

            return characters;
        }

        public bool CreateCharacterOnAccount(CharacterRecord character, WorldClient client)
        {
            if (client.Characters == null)
                client.Characters = new List<CharacterRecord>();

            character.CreateAndFlush(); // we have to flush to get the id TODO : ID generator
            client.Characters.Insert(0, character);

            WorldServer.Instance.IOTaskPool.EnqueueTask(() => IpcAccessor.Instance.ProxyObject.AddAccountCharacter(WorldServer.ServerInformation,
                                                                                                           client.Account.Id,
                                                                                                           (uint)character.Id));
            return true;
        }

        public void DeleteCharacterOnAccount(CharacterRecord character, WorldClient client)
        {
            character.Delete();
            client.Characters.Remove(character);

            WorldServer.Instance.IOTaskPool.EnqueueTask(
                () => IpcAccessor.Instance.ProxyObject.DeleteAccountCharacter(WorldServer.ServerInformation, client.Account.Id, (uint) character.Id));

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

                name += char.ToUpper(RandomConsonant(rand));

                for (int i = 0; i < namelen - 1; i++)
                {
                    name += ( ( i & 1 ) != 1 ) ? RandomConsonant(rand) : RandomVowel(rand);
                }
            } while (CharacterRecord.DoesNameExists(name));

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