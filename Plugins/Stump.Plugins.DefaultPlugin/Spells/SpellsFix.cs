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

            #region FECA

            // Reinforced Protection (422)
            FixEffectOnAllLevels(422, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL);
            FixEffectOnAllLevels(422, 2, (level, effect, critical) => effect.Targets = SpellTargetType.SELF);
            FixEffectOnAllLevels(422, 2, (level, effect, critical) => effect.Delay = 1);

            #endregion

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
            FixEffectOnAllLevels(158, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALL ^ SpellTargetType.ALL_SUMMONS);
            FixEffectOnAllLevels(158, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALL_SUMMONS);

            // bond (142)
            // #2 effect = enemies
            FixEffectOnAllLevels(142, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);

            // duel (144)
            // #3 effect = Only Self
            // #4 effect = Only Self
            FixEffectOnAllLevels(144, 3, (level, effect, critical) => effect.Targets = SpellTargetType.ONLY_SELF);
            FixEffectOnAllLevels(144, 4, (level, effect, critical) => effect.Targets = SpellTargetType.ONLY_SELF);

            // Sword of Fate (146)
            FixEffectOnAllLevels(146, EffectsEnum.Effect_SpellBoost, (level, effect, critical) => effect.Targets = SpellTargetType.ONLY_SELF);
            FixEffectOnAllLevels(146, EffectsEnum.Effect_SpellBoost, (level, effect, critical) => effect.Duration = 3);

            // Putsch (147)
            FixEffectOnAllLevels(147, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALL ^ SpellTargetType.SELF, false);

            // Precipitation (149)
            FixEffectOnAllLevels(149, 2, (level, effect, critical) => effect.Delay = 1, false);

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

            // Regeneration Word (131)
            //Nerf duration => -1
            FixEffectOnAllLevels(131, EffectsEnum.Effect_RestoreHPPercent, (level, effect, critical) => effect.Duration--);

            // Draining Word (123)
            // target CC => ONLY_SELF
            FixEffectOnAllLevels(123, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ONLY_SELF);

            #endregion

            #region ENUTROF

            // corruption (59)
            // effect #4 = only self (state exhausted)
            FixEffectOnAllLevels(59, 3, (level, effect, critical) => effect.Targets = SpellTargetType.ONLY_SELF);

            // Chance (42)
            FixEffectOnAllLevels(42, 1, (level, effect, critical) => effect.Delay = 1, false);

            #endregion

            #region OSAMODAS

            // whip (30)
            // kill effect target -> summons
            FixEffectOnAllLevels(30, EffectsEnum.Effect_Kill, (level, effect, critical) =>
                {
                    effect.Targets = 
                        SpellTargetType.ALLY_STATIC_SUMMONS |
                        SpellTargetType.ALLY_SUMMONS | (critical ? 
                        SpellTargetType.ENEMY_STATIC_SUMMONS |
                        SpellTargetType.ENEMY_SUMMONS : 0);
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

            // Fortune (106)
            FixEffectOnAllLevels(106, 0, (level, effect, critical) => effect.Delay = 3);

            // Odorat (115)
            FixEffectOnAllLevels(115, 2, (level, effect, critical) => effect.Delay = 1, false);
            FixEffectOnAllLevels(115, 3, (level, effect, critical) => effect.Delay = 1, false);
            FixEffectOnAllLevels(115, 4, (level, effect, critical) => effect.Targets = SpellTargetType.ONLY_SELF, false);

            // Tout ou rien (119)
            FixEffectOnAllLevels(119, 1, (level, effect, critical) => effect.Delay = 1);

            // Roulette (101)
            FixEffectOnAllLevels(101, 15, (level, effect, critical) => effect.Targets = SpellTargetType.ONLY_SELF);

            #endregion

            #region Korriandre

            // Glyphe Daivain (2700)
            // kill
            // target none -> only self
            FixEffectOnAllLevels(2700, EffectsEnum.Effect_Kill,
                (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL | SpellTargetType.SELF);

            // Glyphe Daidisse (2701)
            // kill
            // target none -> ALLY ALL
            FixEffectOnAllLevels(2701, EffectsEnum.Effect_Kill,
                (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL | SpellTargetType.SELF);

            #endregion

            #region TOFU

            // béco du tofu (1999)
            // steal agility
            // target only self -> all
            FixEffectOnAllLevels(1999, EffectsEnum.Effect_StealAgility,
                (level, effect, critical) => effect.Targets = SpellTargetType.ALL);

            #endregion

            #region SRAM

            // chakra concentration (62)
            // duration steal = 0
            FixEffectOnAllLevels(62, EffectsEnum.Effect_StealHPFire, (level, effect, critical) => effect.Duration = 0);

            #endregion

            #region PANDAWA

            #endregion

            #region ROUBLARD

            // 2822,2845,2830 bomb explosion spell
            // remove all Effect_ReduceEffectsDuration effects and the second damage effect
            // the kill effect is on the caster (the bomb)
            RemoveEffectOnAllLevels(2822, 0, false);
            RemoveEffectOnAllLevels(2822, 0, false);
            RemoveEffectOnAllLevels(2822, 0, false);
            RemoveEffectOnAllLevels(2822, 3, false);
            FixEffectOnAllLevels(2822, EffectsEnum.Effect_Kill, (level, effect, critical) => effect.Targets = SpellTargetType.ONLY_SELF, false);

            // same here and we remove the second LostMP effect
            RemoveEffectOnAllLevels(2845, 0, false);
            RemoveEffectOnAllLevels(2845, 0, false);
            RemoveEffectOnAllLevels(2845, 0, false);
            RemoveEffectOnAllLevels(2845, 3, false);
            RemoveEffectOnAllLevels(2845, 5, false);
            FixEffectOnAllLevels(2845, EffectsEnum.Effect_Kill, (level, effect, critical) => effect.Targets = SpellTargetType.ONLY_SELF, false);
            
            // same here and we remove the second LostAP effect
            RemoveEffectOnAllLevels(2830, 0, false);
            RemoveEffectOnAllLevels(2830, 0, false);
            RemoveEffectOnAllLevels(2830, 0, false);
            RemoveEffectOnAllLevels(2830, 3, false);
            RemoveEffectOnAllLevels(2830, 5, false);
            FixEffectOnAllLevels(2830, EffectsEnum.Effect_Kill, (level, effect, critical) => effect.Targets = SpellTargetType.ONLY_SELF, false);

            //2825,2829,2833 wall spell
            //Fire wall
            RemoveEffectOnAllLevels(2825, 0, false);

            //Air wall
            RemoveEffectOnAllLevels(2829, 0, false);
            RemoveEffectOnAllLevels(2829, 1, false);

            //Water wall
            RemoveEffectOnAllLevels(2833, 0, false);
            RemoveEffectOnAllLevels(2833, 1, false);

            // botte (2795)
            // 1 effect per shape size
            // 1 effect per ally or enemy
            //Remove Useless effects
            RemoveEffectOnAllLevels(2795, 4);
            RemoveEffectOnAllLevels(2795, 4);
            RemoveEffectOnAllLevels(2795, 5);
            RemoveEffectOnAllLevels(2795, 5);

            FixEffectOnAllLevels(2795, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_BOMBS);
            FixEffectOnAllLevels(2795, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL ^ SpellTargetType.ALLY_BOMBS);
            FixEffectOnAllLevels(2795, 2, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);
            FixEffectOnAllLevels(2795, 3, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_BOMBS);
            FixEffectOnAllLevels(2795, 4, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_BOMBS);
            
            // all allies but self
            FixEffectOnAllLevels(2795, EffectsEnum.Effect_AddDamageBonus, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL ^ SpellTargetType.ALLY_BOMBS);

            // Aimantation (2801)
            //RemoveEffectOnAllLevels(2801, 1, false);            
            // first effect for bombs only, second for all but self and bombs
            FixEffectOnAllLevels(2801, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_BOMBS, false);
            FixEffectOnAllLevels(2801, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL, false);
            FixEffectOnAllLevels(2801, 2, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL, false);

            // Entourloupe (2803)
            FixEffectOnAllLevels(2803, EffectsEnum.Effect_SwitchPosition, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_BOMBS);

            // Roublardise (2763)
            FixEffectOnAllLevels(2763, EffectsEnum.Effect_SkipTurn_1031, (level, effect, critical) => effect.Targets = SpellTargetType.ONLY_SELF);

            // Poudre (2805)
            FixEffectOnAllLevels(2805, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_BOMBS);
            FixEffectOnAllLevels(2805, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_BOMBS);
            RemoveEffectOnAllLevels(2805, 2);

            // Rémission (2809)
            FixEffectOnAllLevels(2809, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALL ^ SpellTargetType.ALLY_BOMBS ^ SpellTargetType.ENEMY_BOMBS);
            FixEffectOnAllLevels(2809, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_BOMBS);

            // Dernier Soufle (2810)
            FixEffectOnAllLevels(2810, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_BOMBS);
            FixEffectOnAllLevels(2810, 2, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL ^ SpellTargetType.ALLY_BOMBS);
            FixEffectOnAllLevels(2810, 3, (level, effect, critical) => effect.Targets = SpellTargetType.ONLY_SELF);

            // Rebours (2811)
            FixEffectOnAllLevels(2811, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_BOMBS);
            FixEffectOnAllLevels(2811, 0, (level, effect, critical) => effect.Delay = 1);
            FixEffectOnAllLevels(2811, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_BOMBS);
            FixEffectOnAllLevels(2811, 2, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL ^ SpellTargetType.ALLY_BOMBS);

            // Surcharge (2812)
            FixEffectOnAllLevels(2812, 0, (level, effect, critical) => effect.Delay = 1, false);
            FixEffectOnAllLevels(2812, 0, (level, effect, critical) => effect.Duration = 2, false);
            FixEffectOnAllLevels(2812, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_BOMBS | SpellTargetType.ENEMY_BOMBS, false);
            FixEffectOnAllLevels(2812, 1, (level, effect, critical) => effect.Delay = 2, false);
            FixEffectOnAllLevels(2812, 1, (level, effect, critical) => effect.Duration = 2, false);
            FixEffectOnAllLevels(2812, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_BOMBS | SpellTargetType.ENEMY_BOMBS, false);
            FixEffectOnAllLevels(2812, 2, (level, effect, critical) => effect.Delay = 3, false);
            FixEffectOnAllLevels(2812, 2, (level, effect, critical) => effect.Duration = 2, false);
            FixEffectOnAllLevels(2812, 2, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_BOMBS | SpellTargetType.ENEMY_BOMBS, false);

            #endregion
        }

        public static void FixEffectOnAllLevels(int spellId, int effectIndex, Action<SpellLevelTemplate, EffectDice, bool> fixer, bool critical=true)
        {
            var spellLevels = SpellManager.Instance.GetSpellLevels(spellId).ToArray();

            if (spellLevels.Length == 0)
                throw new Exception(string.Format("Cannot apply fix on spell {0} : spell do not exists", spellId));

            foreach (var level in spellLevels)
            {
                fixer(level, level.Effects[effectIndex], false);
                if (critical)
                    fixer(level, level.CriticalEffects[effectIndex], true);
            }
        }

        public static void FixEffectOnAllLevels(int spellId, EffectsEnum effect, Action<SpellLevelTemplate, EffectDice, bool> fixer, bool critical=true)
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

                if (!critical)
                    continue;

                foreach (var spellEffect in level.CriticalEffects.Where(entry => entry.EffectId == effect))
                {
                    fixer(level, spellEffect, true);
                }
            }
        }

        public static void FixEffectOnAllLevels(int spellId, Predicate<EffectDice> predicate, Action<SpellLevelTemplate, EffectDice, bool> fixer, bool critical = true)
        {
            var spellLevels = SpellManager.Instance.GetSpellLevels(spellId).ToArray();

            if (spellLevels.Length == 0)
            {
                logger.Error("Cannot apply fix on spell {0} : spell do not exists", spellId);
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

        public static void RemoveEffectOnAllLevels(int spellId, int effectIndex, bool critical=true)
        {
            var spellLevels = SpellManager.Instance.GetSpellLevels(spellId).ToArray();

            if (spellLevels.Length == 0)
            {
                logger.Error("Cannot apply fix on spell {0} : spell do not exists", spellId);
                return;
            }

            foreach (var level in spellLevels)
            {
                level.Effects.RemoveAt(effectIndex);
                if (critical)
                    level.CriticalEffects.RemoveAt(effectIndex);
                
            }
        }

        public static void RemoveEffectOnAllLevels(int spellId, EffectsEnum effect, bool critical=true)
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
                if (critical)
                    level.CriticalEffects.RemoveAll(entry => entry.EffectId == effect);
            }
        }
    }
}