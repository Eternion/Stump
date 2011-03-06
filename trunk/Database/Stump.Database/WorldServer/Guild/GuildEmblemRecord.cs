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

namespace Stump.Database.WorldServer.Guild
{
    [ActiveRecord("guilds_emblem")]
    public sealed class GuildEmblemRecord : WorldBaseRecord<GuildEmblemRecord>
    {
        [PrimaryKey(PrimaryKeyType.Foreign, "GuildId")]
        public uint GuildId { get; set; }

        [OneToOne(Cascade = CascadeEnum.Delete)]
        public GuildRecord Guild { get; set; }

        [Property("SymbolShape", NotNull = true, Default = "0")]
        public int SymbolShape { get; set; }

        [Property("SymbolColor", NotNull = true, Default = "0")]
        public int SymbolColor { get; set; }

        [Property("BackgroundShape", NotNull = true, Default = "0")]
        public int BackgroundShape { get; set; }

        [Property("BackgroundColor", NotNull = true, Default = "0")]
        public int BackgroundColor { get; set; }
    }
}