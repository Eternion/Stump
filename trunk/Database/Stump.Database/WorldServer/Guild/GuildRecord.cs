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

namespace Stump.Database.WorldServer
{
    [AttributeDatabase(DatabaseService.WorldServer)]
    [ActiveRecord("guilds")]
    public sealed class GuildRecord : ActiveRecordBase<GuildRecord>
    {
        private IList<GuildMemberRecord> m_members;
        private IList<GuildHouseRecord> m_houses;
        private IList<GuildPaddockRecord> m_paddocks;
        private IList<CollectorRecord> m_collectors;

        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public uint Id
        {
            get;
            set;
        }

        [Property("Name", NotNull=true)]
        public string Name
        {
            get;
            set;
        }

        [OneToOne(Cascade= CascadeEnum.All)]
        public GuildEmblemRecord Emblem
        {
            get;
            set;
        }

        [Property("Experience", NotNull = true, Default="0")]
        public int Experience
        {
            get;
            set;
        }

        
        [HasMany(typeof(GuildMemberRecord))]
        public IList<GuildMemberRecord> Members
        {
            get { return m_members ?? new List<GuildMemberRecord>(); }
            set { m_members = value; }
        }

        [HasMany(typeof(GuildHouseRecord))]
        public IList<GuildHouseRecord> Houses
        {
            get { return m_houses ?? new List<GuildHouseRecord>(); }
            set { m_houses = value; }
        }

        [HasMany(typeof(GuildPaddockRecord))]
        public IList<GuildPaddockRecord> Paddocks
        {
            get { return m_paddocks ?? new List<GuildPaddockRecord>(); }
            set { m_paddocks = value; }
        }

        [HasMany(typeof(CollectorRecord))]
        public IList<CollectorRecord> Perceptors
        {
            get { return m_collectors ?? new List<CollectorRecord>(); }
            set { m_collectors = value; }
        }   
    }
}