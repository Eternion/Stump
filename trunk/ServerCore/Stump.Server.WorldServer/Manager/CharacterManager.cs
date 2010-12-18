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
using System.Collections.Generic;
using System.Linq;
using Stump.Database;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.IPC;

namespace Stump.Server.WorldServer.Database
{
    public static class CharacterManager
    {
        public static bool CharacterExists(string charactername)
        {
            CharacterRecord cr = CharacterRecord.FindCharacterByCharacterName(charactername);

            return cr != null;
        }

        public static List<CharacterRecord> GetCharactersByAccount(WorldClient client)
        {
            var characters = new List<CharacterRecord>(5);
            var ids = IpcAccessor.Instance.ProxyObject.GetAccountCharacters(WorldServer.ServerInformation,
                                                                            client.Account.Id);
            foreach(uint id in ids)
            {
                var character = CharacterRecord.FindCharacterById((int)id);
                if (character != null)
                    characters.Add(character);
            }

            return characters;
        }

        public static bool CreateCharacter(CharacterRecord character, WorldClient client)
        {
            if (client.Characters == null)
                client.Characters = new List<CharacterRecord>(5);
            character.Save();

            client.Characters.Insert(0, character);

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
    }
}