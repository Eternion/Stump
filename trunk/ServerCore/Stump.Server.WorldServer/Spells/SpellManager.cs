
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Data;
using Stump.Server.BaseServer.Initializing;
using Stump.Server.WorldServer.Effects;
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
            var spellslevels = DataLoader.LoadDataById<SpellLevelTemplate>(entry => (int)entry.id);
            var spells = DataLoader.LoadData<SpellTemplate>();

            foreach (SpellTemplate spell in spells)
            {
                InitSpell(spell, spell.spellLevels.Select(entry => spellslevels[entry]));
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

        public static List<Spell> GetSpells(uint[] spellids)
        {
            return (from spellid in spellids
                    where !Spells.ContainsKey(spellid)
                    select Spells[spellid]).ToList();
        }
    }
}