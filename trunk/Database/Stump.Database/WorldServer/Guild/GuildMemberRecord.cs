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
using Castle.ActiveRecord;
using Stump.Database.Types;

namespace Stump.Database.WorldServer
{
    [ActiveRecord("guilds_members")]
    public sealed class GuildMemberRecord : WorldRecord<GuildMemberRecord>
    {
        [PrimaryKey(PrimaryKeyType.Foreign, "CharacterId")]
        public uint CharacterId { get; set; }

        [OneToOne]
        public CharacterRecord Character { get; set; }


        [BelongsTo("GuildId", NotNull = true)]
        public GuildRecord Guild { get; set; }

        [Property("Rank", NotNull = true)]
        public uint Rank { get; set; }

        [Property("Rights", NotNull = true)]
        public uint Rights { get; set; }

        [Property("GivenExperience", NotNull = true, Default = "0")]
        public uint GivenExperience { get; set; }

        [Property("GivenPercent", NotNull = true, Default = "0")]
        public byte GivenPercent { get; set; }
    }
}