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
using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.Database.AuthServer;

namespace Stump.Database.WorldServer.StartupAction
{
    [Serializable]
    [AttributeDatabase(DatabaseService.WorldServer)]
    [ActiveRecord("startup_actions")]
    public sealed class StartupActionRecord : ActiveRecordBase<StartupActionRecord>
    {
        private IList<WorldAccountRecord> m_accounts;
        private IList<StartupActionItemRecord> m_items;

        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public uint Id
        {
            get;
            set;
        }

        [Property("Title", NotNull = true, Length = 25)]
        public string Title
        {
            get;
            set;
        }

        [Property("Text", NotNull = true, Length = 250)]
        public string Text
        {
            get;
            set;
        }

        [Property("DescUrl", NotNull = true, Length = 50)]
        public string DescUrl
        {
            get;
            set;
        }

        [Property("PictureUrl", NotNull = true, Length = 50)]
        public string PictureUrl
        {
            get;
            set;
        }

        [HasMany(typeof(StartupActionItemRecord), Cascade= ManyRelationCascadeEnum.Delete)]
        public IList<StartupActionItemRecord> Items
        {
            get { return m_items ?? new List<StartupActionItemRecord>(); }
            set { m_items = value; }
        }

        [HasAndBelongsToMany(typeof(WorldAccountRecord), Table = "accounts_startup_actions", ColumnKey = "StartupActionId", ColumnRef = "AccountId", Inverse=true)]
        public IList<WorldAccountRecord> Accounts
        {
            get { return m_accounts ?? new List<WorldAccountRecord>(); }
            set { m_accounts = value; }
        }


        public static StartupActionRecord FindStartupActionById(uint id)
        {
            return FindByPrimaryKey(id);
        }

        public static StartupActionRecord[] FindStartupActionByTitle(string title)
        {
            return FindAll(Restrictions.Eq("Title", title));
        }

    }
}