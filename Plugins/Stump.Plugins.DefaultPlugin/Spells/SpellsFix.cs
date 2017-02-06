using System;
using System.Linq;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Targets;

namespace Stump.Plugins.DefaultPlugin.Spells
{
    public static class SpellsFix
    {
        static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Initialization(typeof(SpellManager), Silent = true)]
        public static void ApplyFix()
        {
            logger.Debug("Apply spells fix");

            #region COMMON

            FixEffectOnAllLevels((int)SpellIdEnum.COUP_DE_POING, 0, (level, effect, critical)
                => effect.Targets = new[] { new TargetTypeCriterion(SpellTargetType.ALLY_ALL, false), new TargetTypeCriterion(SpellTargetType.ENEMY_ALL, false) });

            #endregion COMMON

            #region SADIDA

            // Fix 5667 'Arbre' subVitality All -> new tree only
            FixEffectOnAllLevels((int)SpellIdEnum.ARBRE_5667, EffectsEnum.Effect_SubVitalityPercent, (level, effect, critical) => effect.ZoneShape = SpellShapeEnum.P);

            #endregion

            #region OSAMODAS

            RemoveEffectOnAllLevels((int)SpellIdEnum.LIEN_ANIMAL_26, EffectsEnum.Effect_AddDodge);
            RemoveEffectOnAllLevels((int)SpellIdEnum.LIEN_ANIMAL_26, EffectsEnum.Effect_AddLock);
            RemoveEffectOnAllLevels((int)SpellIdEnum.LIEN_ANIMAL_26, EffectsEnum.Effect_AddHealBonus);

            #endregion OSAMODAS

            #region HUPPERMAGE

            FixEffectOnAllLevels((int)SpellIdEnum.TRAVERSÉE, 0, (level, effect, critical) => effect.ZoneSize = 4);
            FixEffectOnAllLevels((int)SpellIdEnum.TRAVERSÉE, 1, (level, effect, critical) => effect.ZoneSize = 4);
            FixEffectOnAllLevels((int)SpellIdEnum.TRAVERSÉE, 2, (level, effect, critical) => effect.ZoneSize = 4);
            FixEffectOnAllLevels((int)SpellIdEnum.TRAVERSÉE, 3, (level, effect, critical) => effect.ZoneSize = 4);
            FixEffectOnAllLevels((int)SpellIdEnum.TRAVERSÉE, 4, (level, effect, critical) => effect.ZoneSize = 4);

            #endregion HUPPERMAGE
        }

        #region Methods

        public static void FixEffectOnAllLevels(int spellId, int effectIndex, Action<SpellLevelTemplate, EffectDice, bool> fixer, bool critical = true)
        {
            var spellLevels = SpellManager.Instance.GetSpellLevels(spellId).ToArray();

            if (spellLevels.Length == 0)
                throw new Exception($"Cannot apply fix on spell {spellId} : spell do not exists");

            foreach (var level in spellLevels)
            {
                fixer(level, level.Effects[effectIndex], false);
                if (critical && level.CriticalEffects.Count > effectIndex)
                    fixer(level, level.CriticalEffects[effectIndex], true);
            }
        }

        public static void FixEffectOnAllLevels(int spellId, EffectsEnum effect, Action<SpellLevelTemplate, EffectDice, bool> fixer, bool critical = true)
        {
            var spellLevels = SpellManager.Instance.GetSpellLevels(spellId).ToArray();

            if (spellLevels.Length == 0)
            {
                logger.Error($"Cannot apply fix on spell {spellId} : spell do not exists");
                return;
            }

            foreach (var level in spellLevels)
            {
                foreach (var spellEffect in level.Effects.Where(entry => entry.EffectId == effect))
                {
                    fixer(level, spellEffect, false);
                }

                if (!critical)
                    continue;

                foreach (var spellEffect in level.CriticalEffects.Where(entry => entry.EffectId == effect))
                {
                    fixer(level, spellEffect, true);
                }
            }
        }

        public static void FixCriticalEffectOnAllLevels(int spellId, int effectIndex, Action<SpellLevelTemplate, EffectDice, bool> fixer)
        {
            var spellLevels = SpellManager.Instance.GetSpellLevels(spellId).ToArray();

            if (spellLevels.Length == 0)
                throw new Exception($"Cannot apply fix on spell {spellId} : spell do not exists");

            foreach (var level in spellLevels)
            {
                fixer(level, level.CriticalEffects[effectIndex], true);
            }
        }

        public static void FixEffectOnAllLevels(int spellId, Predicate<EffectDice> predicate, Action<SpellLevelTemplate, EffectDice, bool> fixer, bool critical = true)
        {
            var spellLevels = SpellManager.Instance.GetSpellLevels(spellId).ToArray();

            if (spellLevels.Length == 0)
            {
                logger.Error($"Cannot apply fix on spell {spellId} : spell do not exists");
                return;
            }

            foreach (var level in spellLevels)
            {
                foreach (var spellEffect in level.Effects.Where(entry => predicate(entry)))
                {
                    fixer(level, spellEffect, false);
                }

                if (!critical)
                    continue;

                foreach (var spellEffect in level.CriticalEffects.Where(entry => predicate(entry)))
                {
                    fixer(level, spellEffect, true);
                }
            }
        }

        public static void FixEffectOnLevel(int spellId, int level, int effectIndex, Action<SpellLevelTemplate, EffectDice, bool> fixer, bool critical = true)
        {
            var spellLevels = SpellManager.Instance.GetSpellLevels(spellId).ToArray();

            if (spellLevels.Length == 0)
                throw new Exception($"Cannot apply fix on spell {spellId} : spell do not exists");

            var spell = spellLevels[level - 1];

            fixer(spell, spell.Effects[effectIndex], false);
            if (critical && spell.CriticalEffects.Count > effectIndex)
                fixer(spell, spell.CriticalEffects[effectIndex], true);
        }

        public static void RemoveEffectOnAllLevels(int spellId, int effectIndex, bool critical = true)
        {
            var spellLevels = SpellManager.Instance.GetSpellLevels(spellId).ToArray();

            if (spellLevels.Length == 0)
            {
                logger.Error($"Cannot apply fix on spell {spellId} : spell do not exists");
                return;
            }

            foreach (var level in spellLevels)
            {
                level.Effects.RemoveAt(effectIndex);
                if (critical)
                    level.CriticalEffects.RemoveAt(effectIndex);

            }
        }

        public static void RemoveEffectOnAllLevels(int spellId, EffectsEnum effect, bool critical = true)
        {
            var spellLevels = SpellManager.Instance.GetSpellLevels(spellId).ToArray();

            if (spellLevels.Length == 0)
            {
                logger.Error($"Cannot apply fix on spell {spellId} : spell do not exists");
                return;
            }

            foreach (var level in spellLevels)
            {
                level.Effects.RemoveAll(entry => entry.EffectId == effect);
                if (critical)
                    level.CriticalEffects.RemoveAll(entry => entry.EffectId == effect);
            }
        }

        #endregion Methods
    }
}