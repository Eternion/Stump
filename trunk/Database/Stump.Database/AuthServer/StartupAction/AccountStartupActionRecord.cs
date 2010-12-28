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
using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.DofusProtocol.Enums;

namespace Stump.Database
{
    [Serializable]
    [AttributeDatabase(DatabaseService.AuthServer)]
    [ActiveRecord("accounts_startup_actions")]
    public sealed class AccountStartupActionRecord : ActiveRecordBase<AccountStartupActionRecord>
    {

        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public int Id
        {
            get;
            set;
        }

        [Property("AccountId", NotNull = true)]
        public uint AccountId
        {
            get;
            set;
        }

        [Property("StartupActionId", NotNull = true)]
        public uint StartupActionId
        {
            get;
            set;
        }


        public AccountRecord Account
        {
            get { return AccountRecord.FindAccountById(AccountId); }
        }

        public StartupActionRecord StartupAction
        {
            get { return StartupActionRecord.FindStartupActionById(StartupActionId); }
        }


        public static AccountStartupActionRecord FindAccountStartupActionById(int id)
        {
            return FindByPrimaryKey(id);
        }

        public static AccountStartupActionRecord[] FindAccountStartupActionsByAccountId(uint accountId)
        {
            return FindAll(Restrictions.Eq("AccountId", accountId));
        }

        public static AccountStartupActionRecord[] FindAccountStartupActionsByStartupActionId(int startupActionId)
        {
            return FindAll(Restrictions.Eq("StartupActionId", startupActionId));
        }

    }
}