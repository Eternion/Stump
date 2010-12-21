// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Database;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.IPC;

namespace Stump.Server.WorldServer.Manager
{
    public static class CharacterManager
    {
        public static List<CharacterRecord> GetCharactersByAccount(WorldClient client)
        {
            var characters = new List<CharacterRecord>();
            uint[] ids = IpcAccessor.Instance.ProxyObject.GetAccountCharacters(WorldServer.ServerInformation,
                                                                               client.Account.Id);
            characters.AddRange(
                ids.Select(id => CharacterRecord.FindCharacterById((int) id)).Where(character => character != null));
            return characters;
        }

        public static int GetCharactersNumberByAccount(WorldClient client)
        {
            return IpcAccessor.Instance.ProxyObject.GetAccountCharacterCount(WorldServer.ServerInformation,
                                                                             client.Account.Id);
        }

        public static bool CreateCharacter(CharacterRecord character, WorldClient client)
        {
            if (client.Characters == null)
                client.Characters = new List<CharacterRecord>(5);

            character.Id = (int) CharacterRecord.GetNextId();
            character.Create();

            client.Characters.Add(character);

            IpcAccessor.Instance.ProxyObject.AddAccountCharacter(WorldServer.ServerInformation,
                                                                 client.Account.Id,
                                                                 (uint) character.Id);

            return true;
        }

        public static void DeleteCharacter(CharacterRecord character, WorldClient client)
        {
            client.Characters.Remove(character);

            World.Instance.TaskPool.EnqueueTask(() =>
            {
                character.DeleteAssociatedRecords();
                character.Delete();

                IpcAccessor.Instance.ProxyObject.DeleteAccountCharacter(WorldServer.ServerInformation,
                                                                        client.Account.Id,
                                                                        (uint) character.Id);
            });
        }

        #region Character Name Random Generation

        private const string voyelles = "aeiouy";

        private const string consonnes = "bcdfghjklmnpqrstvwxz";

        public static string GenerateName()
        {
            string name;

            do
            {
                var rand = new Random();
                int namelen = rand.Next(5, 10);
                name = string.Empty;

                name += char.ToUpper(RandomConsonne(rand));

                for (int i = 0; i < namelen - 1; i++)
                {
                    name += (i%2 == 1) ? RandomConsonne(rand) : RandomVoyelle(rand);
                }
            } while (CharacterRecord.IsNameExists(name));

            return name;
        }

        private static char RandomVoyelle(Random rand)
        {
            return voyelles[rand.Next(0, voyelles.Length - 1)];
        }

        private static char RandomConsonne(Random rand)
        {
            return consonnes[rand.Next(0, consonnes.Length - 1)];
        }

        #endregion
    }
}