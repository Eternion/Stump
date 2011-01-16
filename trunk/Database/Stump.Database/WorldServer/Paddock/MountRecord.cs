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
using Castle.ActiveRecord;
using NHibernate.Criterion;

namespace Stump.Database
{

    [AttributeDatabase(DatabaseService.WorldServer)]
    [ActiveRecord("mounts")]
    public class MountRecord : ActiveRecordBase<MountRecord>
    {
        private IList<MountRecord> m_ancestors;
        private IList<MountBehaviorRecord> m_behaviors;

        [PrimaryKey(PrimaryKeyType.Identity, "Id")]
        public uint Id
        {
            get;
            set;
        }

        [BelongsTo("OwnerId", NotNull=true)]
        public CharacterRecord Owner
        {
            get;
            set;
        }

        [Property("ModelId", NotNull = true, Default = "0")]
        public uint ModelId
        {
            get;
            set;
        }

        [HasAndBelongsToMany(typeof(MountRecord), Table = "mounts_ancestors", ColumnKey = "MountId", ColumnRef = "AncestorId")]
        public IList<MountRecord> Ancestors
        {
            get { return m_ancestors ?? new List<MountRecord>(); }
            set { m_ancestors = value; }
        }

        [HasMany(typeof(MountBehaviorRecord), Cascade= ManyRelationCascadeEnum.Delete)]
        public IList<MountBehaviorRecord> Behaviors
        {
            get { return m_behaviors ?? new List<MountBehaviorRecord>(); }
            set { m_behaviors = value; }
        }

        [Property("Name", NotNull = true, Default="'SansNom'")]
        public string Name
        {
            get;
            set;
        }

        [Property("Sex", NotNull = true, Default = "0")]
        public bool Sex
        {
            get;
            set;
        }

        [Property("Experience", NotNull = true, Default = "0")]
        public long Experience
        {
            get;
            set;
        }

        [Property("IsWild", NotNull = true, Default = "0")]
        public bool IsWild
        {
            get;
            set;
        }

        [Property("Stamina", NotNull = true, Default = "0")]
        public uint Stamina
        {
            get;
            set;
        }

        [Property("Maturity", NotNull = true, Default = "0")]
        public uint Maturity
        {
            get;
            set;
        }

        [Property("Energy", NotNull = true, Default = "0")]
        public uint Energy
        {
            get;
            set;
        }

        [Property("Serenity", NotNull = true, Default = "0")]
        public uint Serenity
        {
            get;
            set;
        }

        [Property("Love", NotNull = true, Default = "0")]
        public uint Love
        {
            get;
            set;
        }

        [Property("ReproductionCount", NotNull = true, Default = "0")]
        public int ReproductionCount
        {
            get;
            set;
        }
    }
}