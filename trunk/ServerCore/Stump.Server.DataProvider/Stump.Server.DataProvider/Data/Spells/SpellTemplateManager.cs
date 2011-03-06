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
using Stump.DofusProtocol.D2oClasses;
using Stump.Server.DataProvider.Data.D2oTool;
using Stump.Server.WorldServer.Effects;
using Stump.Server.DataProvider.Core;

namespace Stump.Server.DataProvider.Data.Spells
{
    public class SpellTemplateManager : DataManager<uint,SpellTemplate>
    {

        private static Dictionary<int, Spell> m_spellTemplates = new Dictionary<int, Spell>();

        private static readonly Dictionary<int, SpellLevel> m_levelTemplates = new Dictionary<int, SpellLevel>();

        [StageStep(Stages.One, "Loaded Spells Templates")]
        public static void LoadSpellTemplates()
        {

            foreach (SpellLevel level in levelTemplates)
            {






                InitSpell(spell, spellslevels.Where(spellLevel => spellLevel.spellId == spell.id).ToArray());
            }          
        }

        private static void InitSpell(Spell spelltemp, IEnumerable<SpellLevel> spellleveltemp)
        {
            int level = 1;
            foreach (SpellLevel splevel in spellleveltemp)
            {
                var sl = new SpellLevel
                    {
                        Spell = spell,
                        Level = level,
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

                sl.SetEffects(splevel.effects.Select(EffectManager.GuessRealEffect));
                sl.SetCriticalEffects(splevel.criticalEffect.Select(EffectManager.GuessRealEffect));

                spell.ByLevel.Add(level, sl);
                level++;
            }

            WorldServer.Spells.Add((uint) spell.Id, spell);
        }

        protected override SpellTemplate InternalGetOne(uint id)
        {
            throw new NotImplementedException();
        }

        protected override Dictionary<uint, SpellTemplate> InternalGetAll()
        {
            m_spellTemplates = D2OLoader.LoadData<Spell>().ToDictionary(s => s.id);
            var levelTemplates = D2OLoader.LoadDataById<SpellLevel>(entry => (int)entry.id);




        }
    }
}



//        public void SetEffects(IEnumerable<EffectInstance> effects)
//        {
//            m_effects = EffectManager.ConvertExportedEffect(effects);
//        }
//
//        public void SetEffects(IEnumerable<EffectBase> effects)
//        {
//            m_effects = effects.ToArray();
//        }
//
//
//
//        public void SetCriticalEffects(IEnumerable<EffectInstance> effects)
//        {
//            m_criticalEffect = EffectManager.ConvertExportedEffect(effects);
//        }
//
//        public void SetCriticalEffects(IEnumerable<EffectBase> effects)
//        {
//            m_criticalEffect = effects.ToArray();
//        }

