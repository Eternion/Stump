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
using Stump.Database;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.IPC;

namespace Stump.Server.WorldServer.Manager
{
    public static partial class AccountManager
    {
        public static AccountRecord GetAccountByTicket(string ticket)
        {
            return IpcAccessor.Instance.ProxyObject.GetAccountRecordByTicket(WorldServer.ServerInformation, ticket);
        }

        public static AccountRecord GetAccountByNickname(string accName)
        {
            if (!IpcAccessor.Instance.Connected ||
                IpcAccessor.Instance.ProxyObject == null)
                throw new Exception("Cannot acces to AuthServer, check that the server is running");

            return IpcAccessor.Instance.ProxyObject.GetAccountRecordByNickname(WorldServer.ServerInformation, accName);
        }

        public static RoleEnum GetRoleByName(string accName)
        {
            var acc = GetAccountByNickname(accName);

            return acc == null ? RoleEnum.None : acc.Role;
        }

        public static RoleEnum GetRoleWithCheckPassword(string accName, string accPass)
        {
            var acc = GetAccountByNickname(accName);

            return acc == null || acc.Password != accPass ? RoleEnum.None : acc.Role;
        }

        public static bool ExceedsDeletedCharactersQuota(uint accountId)
        {
            return IpcAccessor.Instance.ProxyObject.ExceedsDeletedCharactersQuota(accountId);
        }

    }
}
