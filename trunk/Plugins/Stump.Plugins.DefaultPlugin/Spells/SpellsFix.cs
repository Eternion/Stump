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

            #region IOP
            // iop's wrath (159)
            // increase buff duration to 5
            FixEffectOnAllLevels(159, EffectsEnum.Effect_SpellBoost, (level, effect, critical) => effect.Duration = 5);

            // iop's vitality (155)
            // effect #1 Target = allies (not self)
            // effect #2 Target = self
            FixEffectOnAllLevels(155, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_1);
            FixEffectOnAllLevels(155, 1, (level, effect, critical) => effect.Targets = SpellTargetType.SELF);

            // concentration (158)
            // #2 effect = summons
            FixEffectOnAllLevels(158, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALL_SUMMONS);
            #endregion

            #region SADIDA
            // sacrifice dool
            // target Kill = Only Self
            FixEffectOnAllLevels(2006, EffectsEnum.Effect_Kill, (level, effect, critical) => effect.Targets = SpellTargetType.ONLY_SELF);

            // sylvan power (197)
            // new skin 671 => 893 (todo find relation)
            FixEffectOnAllLevels(197, EffectsEnum.Effect_ChangeAppearance, (level, effect, critical) => effect.Value = 893);
            #endregion

            #region CRA
            // punitive arrow (171)
            // duration buff = 3
            FixEffectOnAllLevels(171, EffectsEnum.Effect_SpellBoost, (level, effect, critical) => effect.Duration = 3);

            // atonement arrow (167)
            // duration buff = 4
            FixEffectOnAllLevels(167, EffectsEnum.Effect_SpellBoost, (level, effect, critical) => effect.Duration = 4);
            #endregion

            #region XELOR
            // mummification (99)
            // new skin 729 => 113 (todo find relation)
            FixEffectOnAllLevels(99, EffectsEnum.Effect_ChangeAppearance_335, (level, effect, critical) => effect.Value = 113);
            #endregion

            #region ENIRIPSA
            // stimulatin word (126)
            // target ally => all
            FixEffectOnAllLevels(126, EffectsEnum.Effect_AddAP_111, (level, effect, critical) => effect.Targets = SpellTargetType.ALL);
            #endregion

            #region ENUTROF
            // living chest (60)
            // kill effect -> removed
            RemoveEffectOnAllLevels(60, EffectsEnum.Effect_Kill);
            #endregion

            #region OSAMODAS
            // whip (30)
            // kill effect target -> summons
            FixEffectOnAllLevels(30, EffectsEnum.Effect_Kill, (level, effect, critical) =>
                {
                    effect.Targets = 
                        SpellTargetType.ALLY_STATIC_SUMMONS |
                        SpellTargetType.ALLY_SUMMONS | (critical ? 
                        SpellTargetType.ENNEMY_STATIC_SUMMONS |
                        SpellTargetType.ENNEMY_SUMMONS : 0);
                });
            #endregion

            #region ECAFLIP
            // heads or tails (102)
            // #1 + #3 = enemies
            // #2 + #4 = allies
            FixEffectOnAllLevels(102, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);
            FixEffectOnAllLevels(102, 2, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);
            FixEffectOnAllLevels(102, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL);
            FixEffectOnAllLevels(102, 3, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL);

            #region TOFU
            // béco du tofu (1999)
            // steal agility
            // target only self -> all
            FixEffectOnAllLevels(1999, EffectsEnum.Effect_StealAgility,
                (level, effect, critical) => effect.Targets = SpellTargetType.ALL);

            #endregion

            #endregion
        }

        public static void FixEffectOnAllLevels(int spellId, int effectIndex, Action<SpellLevelTemplate, EffectDice, bool> fixer)
        {
            var spellLevels = SpellManager.Instance.GetSpellLevels(spellId).ToArray();

            if (spellLevels.Length == 0)
                throw new Exception(string.Format("Cannot apply fix on spell {0} : spell do not exists", spellId));

            foreach (var level in spellLevels)
            {
                fixer(level, level.Effects[effectIndex], false);
                fixer(level, level.CriticalEffects[effectIndex], true);
            }
        }

        public static void FixEffectOnAllLevels(int spellId, EffectsEnum effect, Action<SpellLevelTemplate, EffectDice, bool> fixer)
        {
            var spellLevels = SpellManager.Instance.GetSpellLevels(spellId).ToArray();

            if (spellLevels.Length == 0)
            {
                logger.Error("Cannot apply fix on spell {0} : spell do not exists", spellId);
                return;
            }

            foreach (var level in spellLevels)
            {
                foreach (var spellEffect in level.Effects.Where(entry => entry.EffectId == effect))
                {
                    fixer(level, spellEffect, false);
                }

                foreach (var spellEffect in level.CriticalEffects.Where(entry => entry.EffectId == effect))
                {
                    fixer(level, spellEffect, true);
                }
            }
        }

        public static void RemoveEffectOnAllLevels(int spellId, EffectsEnum effect)
        {
            var spellLevels = SpellManager.Instance.GetSpellLevels(spellId).ToArray();

            if (spellLevels.Length == 0)
            {
                logger.Error("Cannot apply fix on spell {0} : spell do not exists", spellId);
                return;
            }

            foreach (var level in spellLevels)
            {
                level.Effects.RemoveAll(entry => entry.EffectId == effect);
                level.CriticalEffects.RemoveAll(entry => entry.EffectId == effect);
                
            }
        }
    }
}