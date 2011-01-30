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
using System.Linq;
using System.Collections.Generic;
using Stump.Database.WorldServer;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Spells
{
    public class SpellInventory
    {

        public SpellInventory(ISpellsOwner owner, uint spellPoints)
        {
            Owner = owner;
            SpellPoints = spellPoints;
        }

        public SpellInventory(ISpellsOwner owner, List<SpellRecord> spells,uint spellPoints)
        {
            Owner = owner;
            SpellPoints = spellPoints;
            foreach (var record in spells)
            {
                SpellsById.Add(record.SpellId)
            }
        }


        public readonly ISpellsOwner Owner;

        public uint SpellPoints
        {
            get;
            private set;
        }

        public Dictionary<uint, Spell> SpellsById
        {
            get;
            internal set;
        }

        public Spell this[SpellIdEnum id]
        {
            get { return this[(uint) id]; }
        }

        public Spell this[uint id]
        {
            get
            {
                Spell spell;
                SpellsById.TryGetValue(id, out spell);
                return spell;
            }
        }


    }
}