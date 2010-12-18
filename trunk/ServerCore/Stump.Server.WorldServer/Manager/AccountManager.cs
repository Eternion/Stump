/*************************************************************************
 *
 *  Copyright (C) 2009 - 2010  Tesla Team
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 *
 *************************************************************************/

using System;
using Stump.Database;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.IPC;

namespace Stump.Server.WorldServer.Database
{
    public static partial class AccountManager
    {
        public static AccountRecord GetAccountByName(string accName)
        {
            if (!IpcAccessor.Instance.Connected ||
                IpcAccessor.Instance.ProxyObject == null)
                throw new Exception("Cannot acces to AuthServer, check that the server is running");

            return IpcAccessor.Instance.ProxyObject.GetAccountRecordByName(WorldServer.ServerInformation, accName);
        }

        public static RoleEnum GetRoleByName(string accName)
        {
            var acc = GetAccountByName(accName);

            return acc == null ? RoleEnum.None : acc.Role;
        }

        public static RoleEnum GetRoleWithCheckPassword(string accName, string accPass)
        {
            var acc = GetAccountByName(accName);

            return acc == null || acc.Password != accPass ? RoleEnum.None : acc.Role;
        }
    }
}
