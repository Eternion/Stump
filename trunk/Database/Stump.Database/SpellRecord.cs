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
using Stump.DofusProtocol.Enums;

namespace Stump.Database
{
    [AttributeDatabase(DatabaseService.WorldServer)]
    [ActiveRecord("characters_spells")]
    public class SpellRecord : ActiveRecordBase<SpellRecord>
    {
        private static readonly IdGenerator IdGenerator = new IdGenerator(typeof (SpellRecord), "RecordId");

        public SpellRecord(uint id, uint ownerId, int position, int level)
        {
            SpellId = id;
            OwnerId = ownerId;
            Position = position;
            Level = level;
            RecordId = IdGenerator.Next();
        }

        public SpellRecord()
        {
        }

        [PrimaryKey(PrimaryKeyType.Assigned, "SpellRecordId")]
        public long RecordId
        {
            get;
            set;
        }

        [Property("OwnerId", NotNull = true)]
        public uint OwnerId
        {
            get;
            set;
        }

        [Property("SpellId", NotNull = true)]
        public uint SpellId
        {
            get;
            set;
        }

        [Property("Position", NotNull = true)]
        public int Position
        {
            get;
            set;
        }

        [Property("Level", NotNull = true)]
        public int Level
        {
            get;
            set;
        }

        public override string ToString()
        {
            return (SpellIdEnum) SpellId + " (" + SpellId + ")";
        }
    }
}