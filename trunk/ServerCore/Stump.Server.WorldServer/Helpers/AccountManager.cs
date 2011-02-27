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
using Stump.BaseCore.Framework.Pool;
using Stump.Database.AuthServer;
using Stump.Database.WorldServer;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.IPC;

namespace Stump.Server.WorldServer.Helpers
{
    public static class AccountManager
    {
        public static List<CharacterRecord> GetCharactersOnAccount(WorldClient client)
        {
            var characters = new List<CharacterRecord>(Handlers.CharacterHandler.MaxCharacterSlot);
            var ids = client.Account.GetWorldCharactersId(WorldServer.ServerInformation.Id);

            foreach (var id in ids)
            {
                /* don't exists */
                if (!CharacterRecord.Exists(id))
                {
                    TaskPool.Instance.EnqueueTask(() =>
                                                  IpcAccessor.Instance.ProxyObject.DeleteAccountCharacter(
                                                      WorldServer.ServerInformation, client.Account.Id, id));
                }
                else
                {
                    characters.Add(CharacterRecord.FindById((int)id));
                }
            }
            return characters;
        }

        public static void AddCharacterOnAccount(CharacterRecord character, WorldClient client)
        {
            if (client.Characters == null)
                client.Characters = new List<CharacterRecord>(Handlers.CharacterHandler.MaxCharacterSlot);

            client.Characters.Insert(0, character);

            TaskPool.Instance.EnqueueTask(() => IpcAccessor.Instance.ProxyObject.AddAccountCharacter(
                WorldServer.ServerInformation, client.Account.Id, (uint)character.Id));
        }

        public static void RemoveCharacterFromAccount(CharacterRecord character, WorldClient client)
        {
            client.Characters.Remove(character);

            TaskPool.Instance.EnqueueTask(() => IpcAccessor.Instance.ProxyObject.DeleteAccountCharacter(
                WorldServer.ServerInformation, client.Account.Id, (uint)character.Id));
        }

        public static int GetNumberOfDayDeletedCharacter(uint accountId)
        {
            return IpcAccessor.Instance.ProxyObject.GetDeletedCharactersNumber(accountId);
        }

        public static AccountRecord GetAccountByTicket(string ticket)
        {
            return IpcAccessor.Instance.ProxyObject.GetAccountRecordByTicket(WorldServer.ServerInformation, ticket);
        }

        public static AccountRecord GetAccountByNickname(string nickName)
        {
            if (!IpcAccessor.Instance.Connected ||  IpcAccessor.Instance.ProxyObject == null)
                throw new Exception("Cannot acces to AuthServer, check that the server is running");

            return IpcAccessor.Instance.ProxyObject.GetAccountRecordByNickname(WorldServer.ServerInformation, nickName);
        }

        public static RoleEnum GetRoleByNickName(string nickName)
        {
            var acc = GetAccountByNickname(nickName);

            return acc == null ? RoleEnum.None : acc.Role;
        }

        public static RoleEnum GetRoleWithCheckPassword(string nickName, string accPass)
        {
            var acc = GetAccountByNickname(nickName);

            return acc == null || acc.Password != accPass ? RoleEnum.None : acc.Role;
        }
    }
}
