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

namespace Stump.Database.WorldServer.Paddock
{
    [ActiveRecord("mounts_behaviors")]
    public sealed class MountBehaviorRecord : WorldBaseRecord<MountBehaviorRecord>
    {
        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public long Id { get; set; }

        [BelongsTo("MountId", NotNull = true)]
        public MountRecord Mount { get; set; }

        [Property("Behavior", NotNull = true)]
        public byte Behavior { get; set; }
    }
}