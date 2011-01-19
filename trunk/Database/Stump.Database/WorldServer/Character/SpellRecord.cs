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

namespace Stump.Database.WorldServer
{
    [AttributeDatabase(DatabaseService.WorldServer)]
    [ActiveRecord("characters_spells")]
    public class SpellRecord : ActiveRecordBase<SpellRecord>
    {

        [PrimaryKey(PrimaryKeyType.Identity)]
        public long Id
        {
            get;
            set;
        }

        [BelongsTo("CharacterId", NotNull=true)]
        public CharacterRecord Character
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

        [Property("Position", NotNull = true, Default="0")]
        public int Position
        {
            get;
            set;
        }

        [Property("Level", NotNull = true, Default="1")]
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