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
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Data;
using Stump.Server.BaseServer.Initializing;
using SpellLevelTemplate = Stump.DofusProtocol.D2oClasses.SpellLevel;
using SpellTemplate = Stump.DofusProtocol.D2oClasses.Spell;

namespace Stump.Server.WorldServer.Spells
{
    public static class SpellManager
    {
        #region Fields

        /// <summary>
        ///   All spells handled by manager.
        /// </summary>
        private static readonly Dictionary<uint, Spell> Spells = new Dictionary<uint, Spell>();

        #endregion

        [StageStep(Stages.One, "Loaded Spells")]
        public static void LoadSpells()
        {
            var spellslevels = DataLoader.LoadData<SpellLevelTemplate>();

            var spells = DataLoader.LoadData<SpellTemplate>();

            foreach (SpellTemplate spell in spells)
            {
                InitSpell(spell, spellslevels.Where(spellLevel => spellLevel.spellId == spell.id));
            }
        }

        private static void InitSpell(SpellTemplate spelltemp, IEnumerable<SpellLevelTemplate> spellleveltemp)
        {
            var spell = new Spell {Id = (SpellIdEnum) spelltemp.id, SpellType = (SpellTypeEnum) spelltemp.typeId};

            int level = 1;
            foreach (SpellLevelTemplate splevel in spellleveltemp)
            {
                var sl = new SpellLevel
                    {
                        Spell = spell,
                        ApCost = splevel.apCost,
                        CastInLine = splevel.castInLine,
                        CastTestLos = splevel.castTestLos,
                        CriticalFailureEndsTurn = splevel.criticalFailureEndsTurn,
                        CriticalFailureProbability = splevel.criticalFailureProbability,
                        CriticalHitProbability = splevel.criticalHitProbability,
                        HideEffects = splevel.hideEffects,
                        MaxCastPerTarget = splevel.maxCastPerTarget,
                        MaxCastPerTurn = splevel.maxCastPerTurn,
                        MinCastInterval = splevel.minCastInterval,
                        MinPlayerLevel = splevel.minPlayerLevel,
                        MinRange = splevel.minRange,
                        NeedFreeCell = splevel.needFreeCell,
                        NeedFreeTrapCell = splevel.needFreeTrapCell,
                        Range = splevel.range,
                        RangeCanBeBoosted = splevel.rangeCanBeBoosted
                    };

                sl.SetEffects(splevel.effects);
                sl.SetCriticalEffects(splevel.criticalEffect);

                spell.ByLevel.Add(level, sl);
                level++;
            }
            if (spell.ByLevel.Count == 0)
                Console.Write("");

            Spells.Add((uint) spell.Id, spell);
        }

        public static Spell GetSpell(SpellIdEnum spellid)
        {
            return Spells[(uint) spellid];
        }

        public static Spell GetSpell(uint spellid)
        {
            return Spells[spellid];
        }

        public static List<Spell> GetSpells(SpellIdEnum[] spellids)
        {
            return (from spellid in spellids
                    where !Spells.ContainsKey((uint) spellid)
                    select Spells[(uint) spellid]).ToList();
        }

        public static List<Spell> GetSpells(int[] spellids)
        {
            return (from spellid in spellids
                    where !Spells.ContainsKey((uint) spellid)
                    select Spells[(uint) spellid]).ToList();
        }
    }
}