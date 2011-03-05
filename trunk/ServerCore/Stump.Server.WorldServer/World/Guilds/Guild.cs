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
using System.Linq;
using System.Collections.Generic;
using Stump.Database.WorldServer;
using Stump.DofusProtocol.Classes;
using Stump.Server.DataProvider.Data.Threshold;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.World.Entities.TaxCollectors;
using GuildEmblem = Stump.Server.WorldServer.World.Guilds.GuildEmblem;
using GuildMember = Stump.Server.WorldServer.World.Guilds.GuildMember;

namespace Stump.Server.WorldServer.World.Guilds
{
    public class Guild
    {

        public Guild(GuildRecord record)
        {
            Id = record.Id;
            Name = record.Name;
            Emblem = new GuildEmblem(record.Emblem);
            Experience = record.Experience;
            Members = record.Members.Select( m => new GuildMember(this,m)).ToList();
            TaxCollectors = record.TaxCollectors.Select(t => new TaxCollector(this,t)).ToList();
        }

        public uint Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public GuildEmblem Emblem
        {
            get;
            set;
        }

        public long Experience
        {
            get;
            set;
        }

        public uint Level
        {
            get { return ThresholdManager.Instance["GuildExp"].GetLevel(Experience); }
            set { Experience = ThresholdManager.Instance["GuildExp"].GetLowerBound(value); }
        }

        public List<GuildMember> Members
        {
            get;
            set;
        }

        public List<TaxCollector> TaxCollectors
        {
            get;
            set;
        }

        public List<Paddock> Paddocks;

        public List<House> Houses;

        public GuildInformations ToGuildInformations()
        {
            return new GuildInformations(Id, Name, Emblem.ToGuildEmblem());
        }
    }
}