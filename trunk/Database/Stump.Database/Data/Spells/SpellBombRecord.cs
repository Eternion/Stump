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
using Stump.Database.Types;

namespace Stump.Database.Data.Spells
{
    [Serializable]
    [ActiveRecord("spell_bombs")]
    [AttributeAssociatedFile("SpellBombs")]
    public sealed class SpellBombRecord : DataBaseRecord<SpellBombRecord>
    {
        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public int Id
        {
            get;
            set;
        }

        [Property("ChainReactionSpellId")]
        public int ChainReactionSpellId
        {
            get;
            set;
        }

        [Property("ExplodSpellId")]
        public int ExplodSpellId
        {
            get;
            set;
        }

        [Property("WallId")]
        public int WallId
        {
            get;
            set;
        }

        [Property("InstantSpellId")]
        public int InstantSpellId
        {
            get;
            set;
        }

        [Property("ComboCoeff")]
        public int ComboCoeff
        {
            get;
            set;
        }
    }
}