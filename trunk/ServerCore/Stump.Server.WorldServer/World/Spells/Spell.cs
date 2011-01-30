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
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Spells
{
    public class Spell
    {

        public Spell()
        {
            ByLevel = new Dictionary<int, SpellLevel>();
        }

        public SpellIdEnum Id
        {
            get;
            set;
        }

        public SpellTypeEnum SpellType
        {
            get;
            set;
        }

        public int CurrentLevel
        {
            get;
            set;
        }

        public SpellLevel CurrentSpellLevel
        {
            get
            {
                return !ByLevel.ContainsKey(CurrentLevel) ? ByLevel[1] : ByLevel[CurrentLevel];
            }
        }

        public int Position
        {
            get;
            set;
        }

        public Dictionary<int, SpellLevel> ByLevel
        {
            get;
            set;
        }


        public SpellItem ToNetworkSpell()
        {
            return new SpellItem((uint) Position, (int) Id, CurrentLevel);
        }

    }
}