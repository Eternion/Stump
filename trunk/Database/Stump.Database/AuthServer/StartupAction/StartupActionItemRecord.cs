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
    [ActiveRecord("startup_actions_objects")]
    public sealed class StartupActionItemRecord : ActiveRecordBase<StartupActionItemRecord>
    {

        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public uint Id
        {
            get;
            set;
        }

        [BelongsTo("StartupActionId", NotNull = true)]
        public StartupActionRecord StartupAction
        {
            get;
            set;
        }

        [Property("ItemGID", NotNull = true)]
        public uint ItemId
        {
            get;
            set;
        }

        [Property("JetMax", NotNull = true, Default="1")]
        public int JetMax
        {
            get;
            set;
        }



        public static StartupActionItemRecord[] FindItemsByStartupActionId(StartupActionRecord startupAction)
        {
            return FindAll(Restrictions.Eq("StartupAction", startupAction));
        }

    }
}