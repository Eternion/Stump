using System;
using System.Linq;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Plugins.DefaultPlugin.Spells
{
    public static class SpellsFix
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Initialization(typeof(SpellManager), Silent = true)]
        public static void ApplyFix()
        {
            logger.Debug("Apply spells fix");

            // iop's wrath (159)
            // increase buff duration to 5
            FixEffectOnAllLevels(159, EffectsEnum.Effect_SpellBoost, (level, effect, critical) => effect.Duration = 5);

            // iop's vitality (155)
            // effect #1 Target = allies (not self)
            // effect #2 Target = self
            FixEffectOnAllLevels(155, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_1);
            FixEffectOnAllLevels(155, 1, (level, effect, critical) => effect.Targets = SpellTargetType.SELF);

            // sacrifice dool
            // target Kill = Only Self
            FixEffectOnAllLevels(233, EffectsEnum.Effect_Kill, (level, effect, critical) => effect.Targets = SpellTargetType.ONLY_SELF);
        
            // punitive arrow (171)
            // duration buff = 3
            FixEffectOnAllLevels(171, EffectsEnum.Effect_SpellBoost, (level, effect, critical) => effect.Duration = 3);
        }

        public static void FixEffectOnAllLevels(int spellId, int effectIndex, Action<SpellLevelTemplate, EffectDice, bool> fixer)
        {
            var spellLevels = SpellManager.Instance.GetSpellLevels(spellId).ToArray();

            if (spellLevels.Length == 0)
                throw new Exception(string.Format("Cannot apply fix on spell {0} : spell do not exists", spellId));

            foreach (var level in spellLevels)
            {
                fixer(level, level.Effects[effectIndex], false);
                fixer(level, level.CritialEffects[effectIndex], true);
            }
        }

        public static void FixEffectOnAllLevels(int spellId, EffectsEnum effect, Action<SpellLevelTemplate, EffectDice, bool> fixer)
        {
            var spellLevels = SpellManager.Instance.GetSpellLevels(spellId).ToArray();

            if (spellLevels.Length == 0)
                throw new Exception(string.Format("Cannot apply fix on spell {0} : spell do not exists", spellId));

            foreach (var level in spellLevels)
            {
                foreach (var spellEffect in level.Effects.Where(entry => entry.EffectId == effect))
                {
                    fixer(level, spellEffect, false);
                }

                foreach (var spellEffect in level.CritialEffects.Where(entry => entry.EffectId == effect))
                {
                    fixer(level, spellEffect, true);
                }
            }
        }
    }
}