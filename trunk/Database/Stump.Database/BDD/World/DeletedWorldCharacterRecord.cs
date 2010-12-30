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
using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.DofusProtocol.Enums;

namespace Stump.Database
{
    [Serializable]
    [AttributeDatabase(DatabaseService.AuthServer)]
    [ActiveRecord("bans")]
    public sealed class DeletedWorldCharacterRecord : ActiveRecordBase<DeletedWorldCharacterRecord>
    {

        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public long Id
        {
            get;
            set;
        }

        [BelongsTo(Column = "AccountId")]
        public AccountRecord Account
        {
            get;
            set;
        }

        [BelongsTo(Column = "WorldId")]
        public WorldRecord World
        {
            get;
            set;
        }

        [KeyProperty(Column = "CharacterId")]
        public uint CharacterId
        {
            get;
            set;
        }

        [KeyProperty(Column = "DeletionDate", NotNull = true, Default = "CURRENT_TIMESTAMP")]
        public DateTime DeletionDate
        {
            get;
            set;
        }

    }
}