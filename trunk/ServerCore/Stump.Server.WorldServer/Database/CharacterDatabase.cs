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
    public static class CharacterDatabase
    {
        public static bool CharacterExists(string charactername)
        {
            CharacterRecord cr = CharacterRecord.FindCharacterByCharacterName(charactername);

            return cr != null;
        }


        public static int GetCharacterNumber(WorldClient client)
        {
            LoadCharacters(client);

            return client.Characters.Count;
        }

        public static void LoadCharacters(WorldClient client)
        {
            client.Characters =
                new List<CharacterRecord>(CharacterRecord.FindCharactersByAccountName(client.Account.Name));
        }

        public static bool CreateCharacter(CharacterRecord character, WorldClient client)
        {
            if (client.Characters == null)
                client.Characters = new List<CharacterRecord>();
            character.Save();

            // is there a better way ?
            client.Characters = new[] {character}.Concat(client.Characters).ToList();

            UpdateCharactersCount(client);

            return true;
        }

        public static void DeleteCharacter(CharacterRecord character, WorldClient client)
        {
            client.Characters.Remove(character);
            World.Instance.TaskPool.EnqueueTask(() =>
            {
                character.DeleteAssociatedRecords();
                character.Delete();
            });

            UpdateCharactersCount(client);
        }

        public static void UpdateCharactersCount(WorldClient client)
        {
            IpcAccessor.Instance.ProxyObject.SetAccountCharactersCount(WorldServer.ServerInformation,
                                                                       client.Account.Id,
                                                                       (byte) client.Characters.Count);
        }
    }
}