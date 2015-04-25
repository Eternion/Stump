using System;
using System.Linq;
using NLog;
using Stump.Core.Extensions;
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

            // Epée divine (145)
            // #1 effect = SELF, ALLY_ALL
            FixEffectOnAllLevels(145, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL | SpellTargetType.SELF);

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

            // Mot Lotof (427)
            // new skin 672 => 923 (todo find relation)
            FixEffectOnAllLevels(427, EffectsEnum.Effect_ChangeAppearance, (level, effect, critical) => effect.Value = 923);

            // Combustion Spontanée (1679)
            // new skin -672 => -923 (todo find relation)
            FixEffectOnAllLevels(1679, EffectsEnum.Effect_ChangeAppearance, (level, effect, critical) => effect.Value = -923);

            #endregion

            #region ENUTROF

            // corruption (59)
            // effect #4 = only self (state exhausted)
            FixEffectOnAllLevels(59, 3, (level, effect, critical) => effect.Targets = SpellTargetType.ONLY_SELF);

            // Chance (42)
            FixEffectOnAllLevels(42, 1, (level, effect, critical) => effect.Delay = 1, false);

            // Retraite anticipée (425)
            // Delay -> 1
            // NONE -> ONLY_SELF
            FixEffectOnAllLevels(425, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALL ^ SpellTargetType.SELF, false);
            FixEffectOnAllLevels(425, 1, (level, effect, critical) => effect.Delay = 1, false);
            FixEffectOnAllLevels(425, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ONLY_SELF, false);
            FixEffectOnAllLevels(425, 2, (level, effect, critical) => effect.Targets = SpellTargetType.ALL ^ SpellTargetType.SELF, false);
            FixEffectOnAllLevels(425, 3, (level, effect, critical) => effect.Delay = 1, false);
            FixEffectOnAllLevels(425, 3, (level, effect, critical) => effect.Targets = SpellTargetType.ONLY_SELF, false);

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

            #region SRAM

            // chakra concentration (62)
            // duration steal = 0
            FixEffectOnAllLevels(62, EffectsEnum.Effect_StealHPFire, (level, effect, critical) => effect.Duration = 0);

            #endregion

            #region SACRIEUR

            // Douleur Partagée (421)
            // ALLY_ALL -> ALLY_ALL | SELF
            FixEffectOnAllLevels(421, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL | SpellTargetType.SELF);
            FixEffectOnAllLevels(421, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL | SpellTargetType.SELF);

            // Coopération (445)
            FixEffectOnAllLevels(445, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL, false);
            FixEffectOnAllLevels(445, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL, false);

            #endregion

            #region PANDAWA

            // Picole (686)
            // new skin 667 => 44 (todo find relation)
            FixEffectOnAllLevels(686, EffectsEnum.Effect_ChangeAppearance_335, (level, effect, critical) => effect.Value = 44);

            // Epouvante (689)
            // Move push effect to first exec debuff
            FixEffectOnAllLevels(689, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALL);
            FixEffectOnAllLevels(689, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALL);
            FixEffectOnAllLevels(689, 0, (level, effect, critical) => level.Effects.Move(effect, 1), false);
            FixCriticalEffectOnAllLevels(689, 0, (level, effect, critical) => level.CriticalEffects.Move(effect, 1));


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

            // Resquille (2807)
            FixEffectOnAllLevels(2807, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALL ^ SpellTargetType.SELF);
            FixEffectOnAllLevels(2807, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALL ^ SpellTargetType.SELF);

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
            FixEffectOnAllLevels(2811, 1, (level, effect, critical) => level.Effects.Move(effect, 2), false);
            FixEffectOnAllLevels(2811, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL ^ SpellTargetType.ALLY_BOMBS);
            FixEffectOnAllLevels(2811, 2, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_BOMBS);

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

            // Kaboom (2815)
            FixEffectOnAllLevels(2815, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL | SpellTargetType.SELF, false);

            #endregion

            #region ZOBAL

            // Masque de classe (2872)
            // NONE -> ALLY_ALL
            FixEffectOnAllLevels(2872, EffectsEnum.Effect_AddLock, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL | SpellTargetType.SELF);

            // Masque du pleutre (2879)
            // new skin 103 => 1576 (todo find relation)
            // new skin 106 => 1576 (todo find relation)
            FixEffectOnAllLevels(2879, EffectsEnum.Effect_ChangeAppearance_335, (level, effect, critical) => effect.Value = 1576);
            FixEffectOnAllLevels(2879, EffectsEnum.Effect_AddDodge, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL | SpellTargetType.SELF);

            // Masque du Psychopathe (2880)
            // new skin 102 => 1575 (todo find relation)
            // new skin 105 => 1575 (todo find relation)
            FixEffectOnAllLevels(2880, EffectsEnum.Effect_ChangeAppearance_335, (level, effect, critical) => effect.Value = 1575);
            FixEffectOnAllLevels(2880, EffectsEnum.Effect_IncreaseDamage_138, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL | SpellTargetType.SELF);

            // Appeau (2883)
            // NONE -> ENEMY_ALL
            FixEffectOnAllLevels(2883, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);

            // Distance (2885)
            // NONE -> ENEMY_ALL
            FixEffectOnAllLevels(2885, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);

            // Furia (2887)
            // NONE -> ONLY_SELF
            // NONE -> ENEMY_ALL
            FixEffectOnAllLevels(2887, EffectsEnum.Effect_AddDamageBonus, (level, effect, critical) => effect.Targets = SpellTargetType.ONLY_SELF);
            FixEffectOnAllLevels(2887, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);

            // Cabriole (2888)
            // NONE -> ENEMY_ALL
            // NONE -> ONLY_SELF
            FixEffectOnAllLevels(2888, EffectsEnum.Effect_DamageAir, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);
            FixEffectOnAllLevels(2888, EffectsEnum.Effect_IncreaseDamage_138, (level, effect, critical) => effect.Targets = SpellTargetType.ONLY_SELF);

            // Boliche (2889)
            // NONE -> ONLY_SELF
            // NONE -> ENEMY_ALL
            // Swap Effects index
            FixEffectOnAllLevels(2889, EffectsEnum.Effect_AddPushDamageBonus, (level, effect, critical) => effect.Targets = SpellTargetType.ONLY_SELF);
            FixEffectOnAllLevels(2889, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);
            FixEffectOnAllLevels(2889, 3, (level, effect, critical) => level.Effects.Move(effect, 0), false);
            FixCriticalEffectOnAllLevels(2889, 3, (level, effect, critical) => level.CriticalEffects.Move(effect, 0));

            // Plastron (2890)
            // NONE -> ALLY_ALL && SELF
            FixEffectOnAllLevels(2890, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL | SpellTargetType.SELF);

            // Tortoruga (2891)
            // NONE -> ALLY_ALL
            FixEffectOnAllLevels(2891, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL);

            // Transe (2892)
            // NONE -> ALLY_ALL && SELF
            // NONE -> ALLY_ALL && SELF
            // NONE -> ONLY_SELF
            FixEffectOnAllLevels(2892, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL | SpellTargetType.SELF);
            FixEffectOnAllLevels(2892, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL | SpellTargetType.SELF);
            FixEffectOnAllLevels(2892, 2, (level, effect, critical) => effect.Targets = SpellTargetType.ONLY_SELF);

            // Appui (2896)
            // NONE -> ENEMY_ALL
            FixEffectOnAllLevels(2896, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);
            FixEffectOnAllLevels(2896, 2, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);
            FixEffectOnAllLevels(2896, 3, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);

            #endregion

            #region STEAMER

            // Marée (3203)
            FixEffectOnAllLevels(3203, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);
            FixEffectOnAllLevels(3203, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL);
            FixEffectOnAllLevels(3203, 0, (level, effect, critical) => effect.Targets = SpellTargetType.NONE, false);
            FixEffectOnAllLevels(3203, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL, false);
            FixEffectOnAllLevels(3203, 2, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL, false);

            // Ressac (3204)
            FixEffectOnAllLevels(3204, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);
            FixEffectOnAllLevels(3204, 2, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);
            FixEffectOnAllLevels(3204, 3, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);
            FixEffectOnAllLevels(3204, 5, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);

            // Courant (3205)
            FixEffectOnAllLevels(3205, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);
            FixEffectOnAllLevels(3205, 2, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);
            FixEffectOnAllLevels(3205, 4, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);
            FixEffectOnAllLevels(3205, 5, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);

            // Flibuste (3206)
            FixEffectOnAllLevels(3206, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);
            FixEffectOnAllLevels(3206, 2, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);
            FixEffectOnAllLevels(3206, 3, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);

            // Selpâtre (3207)
            FixEffectOnAllLevels(3207, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);
            FixEffectOnAllLevels(3207, 2, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);
            FixEffectOnAllLevels(3207, 4, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);

            // Ecume (3208)
            FixEffectOnAllLevels(3208, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);
            FixEffectOnAllLevels(3208, 2, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);
            FixEffectOnAllLevels(3208, 4, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);

            // Vapor (3209)
            FixEffectOnAllLevels(3209, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);
            FixEffectOnAllLevels(3209, 2, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);
            FixEffectOnAllLevels(3209, 3, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);

            // Ancrage (3210)
            FixEffectOnAllLevels(3210, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);
            FixEffectOnAllLevels(3210, 2, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);
            FixEffectOnAllLevels(3210, 3, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);

            // Foène (3211)
            FixEffectOnAllLevels(3211, 2, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);
            FixEffectOnAllLevels(3211, 3, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);
            FixEffectOnAllLevels(3211, 4, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);

            // Harponneuse (3212)
            //Remove Kill Effect
            RemoveEffectOnAllLevels(3212, 1, false);

            // Gardienne (3213)
            //Remove Kill Effect
            RemoveEffectOnAllLevels(3213, 1, false);

            // Tactirelle (3214)
            //Remove Kill Effect
            RemoveEffectOnAllLevels(3214, 1, false);

            // Evolution (3215)
            RemoveEffectOnAllLevels(3215, 0, false);
            FixEffectOnAllLevels(3215, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS, false);
            FixEffectOnAllLevels(3215, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS, false);
            FixEffectOnAllLevels(3215, 2, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL ^ SpellTargetType.ALLY_TURRETS, false);

            // Cuirasse (3216)
            FixEffectOnAllLevels(3216, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL ^ SpellTargetType.ENEMY_TURRETS, false);
            FixEffectOnAllLevels(3216, 1, (level, effect, critical) => effect.Targets = (SpellTargetType.ALLY_ALL ^ SpellTargetType.ALLY_TURRETS) | SpellTargetType.SELF, false);

            // Armure de Sel (3217)
            RemoveEffectOnAllLevels(3217, 0);
            RemoveEffectOnAllLevels(3217, 0);
            FixEffectOnAllLevels(3217, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);
            FixEffectOnAllLevels(3217, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL ^ SpellTargetType.ALLY_TURRETS);

            // Embuscade (3218)
            FixEffectOnAllLevels(3218, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);
            FixEffectOnAllLevels(3218, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);
            FixEffectOnAllLevels(3218, 2, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);
            FixEffectOnAllLevels(3218, 3, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);

            // Longue Vue (3220)
            FixEffectOnAllLevels(3220, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);
            FixEffectOnAllLevels(3220, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);
            FixEffectOnAllLevels(3220, 2, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);
            FixEffectOnAllLevels(3220, 5, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL | SpellTargetType.SELF);
            FixEffectOnAllLevels(3220, 6, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS);

            // Aspiration (3221)
            RemoveEffectOnAllLevels(3221, 0, false);
            RemoveEffectOnAllLevels(3221, 0, false);
            FixEffectOnAllLevels(3221, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS, false);

            // Boumf I (3222)
            FixEffectOnAllLevels(3222, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);
            FixEffectOnAllLevels(3222, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL ^ SpellTargetType.ALLY_TURRETS);

            // Boume I (3223)
            FixEffectOnAllLevels(3223, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);
            FixEffectOnAllLevels(3223, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL ^ SpellTargetType.ALLY_TURRETS);

            // Boumt I (3224)
            FixEffectOnAllLevels(3224, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);
            FixEffectOnAllLevels(3224, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL ^ SpellTargetType.ALLY_TURRETS);

            // Boumf II (3225)
            FixEffectOnAllLevels(3225, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);
            FixEffectOnAllLevels(3225, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL ^ SpellTargetType.ALLY_TURRETS);

            // Boume II (3226)
            FixEffectOnAllLevels(3226, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);
            FixEffectOnAllLevels(3226, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL ^ SpellTargetType.ALLY_TURRETS);

            // Boumt II (3227)
            FixEffectOnAllLevels(3227, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);
            FixEffectOnAllLevels(3227, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL ^ SpellTargetType.ALLY_TURRETS);

            // Boumf III (3228)
            FixEffectOnAllLevels(3228, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);
            FixEffectOnAllLevels(3228, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL ^ SpellTargetType.ALLY_TURRETS);

            // Boume III (3229)
            FixEffectOnAllLevels(3229, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);
            FixEffectOnAllLevels(3229, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL ^ SpellTargetType.ALLY_TURRETS);

            // Boumt III (3230)
            FixEffectOnAllLevels(3230, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);
            FixEffectOnAllLevels(3230, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL ^ SpellTargetType.ALLY_TURRETS);

            // Cinétik I (3233)
            RemoveEffectOnAllLevels(3233, 0, false);
            RemoveEffectOnAllLevels(3233, 0, false);
            FixEffectOnAllLevels(3233, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALL ^ SpellTargetType.ALLY_TURRETS ^ SpellTargetType.SELF, false);
            FixEffectOnAllLevels(3233, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALL ^ SpellTargetType.ALLY_TURRETS ^ SpellTargetType.SELF, false);

            // Cinétik II (3234)
            RemoveEffectOnAllLevels(3234, 0, false);
            RemoveEffectOnAllLevels(3234, 0, false);
            FixEffectOnAllLevels(3234, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALL ^ SpellTargetType.ALLY_TURRETS ^ SpellTargetType.SELF, false);
            FixEffectOnAllLevels(3234, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALL ^ SpellTargetType.ALLY_TURRETS ^ SpellTargetType.SELF, false);

            // Cinétik III (3235)
            RemoveEffectOnAllLevels(3235, 0, false);
            RemoveEffectOnAllLevels(3235, 0, false);
            FixEffectOnAllLevels(3235, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALL ^ SpellTargetType.ALLY_TURRETS ^ SpellTargetType.SELF, false);
            FixEffectOnAllLevels(3235, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALL ^ SpellTargetType.ALLY_TURRETS ^ SpellTargetType.SELF, false);

            // Magnétor I (3236)
            RemoveEffectOnAllLevels(3236, 0, false);
            RemoveEffectOnAllLevels(3236, 0, false);
            FixEffectOnAllLevels(3236, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALL ^ SpellTargetType.ALLY_TURRETS ^ SpellTargetType.SELF, false);
            FixEffectOnAllLevels(3236, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALL ^ SpellTargetType.ALLY_TURRETS ^ SpellTargetType.SELF, false);

            // Magnétor II (3237)
            RemoveEffectOnAllLevels(3237, 0, false);
            RemoveEffectOnAllLevels(3237, 0, false);
            FixEffectOnAllLevels(3237, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALL ^ SpellTargetType.ALLY_TURRETS ^ SpellTargetType.SELF, false);
            FixEffectOnAllLevels(3237, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALL ^ SpellTargetType.ALLY_TURRETS ^ SpellTargetType.SELF, false);

            // Magnétor III (3238)
            RemoveEffectOnAllLevels(3238, 0, false);
            RemoveEffectOnAllLevels(3238, 0, false);
            FixEffectOnAllLevels(3238, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALL ^ SpellTargetType.ALLY_TURRETS ^ SpellTargetType.SELF, false);
            FixEffectOnAllLevels(3238, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALL ^ SpellTargetType.ALLY_TURRETS ^ SpellTargetType.SELF, false);

            // Transko (3240)
            RemoveEffectOnAllLevels(3240, 0, false);
            RemoveEffectOnAllLevels(3240, 0, false);
            RemoveEffectOnAllLevels(3240, 0, false);
            FixEffectOnAllLevels(3240, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALL ^ SpellTargetType.ENEMY_TURRETS ^ SpellTargetType.SELF, false);
            //FixEffectOnAllLevels(3240, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALL ^ SpellTargetType.SELF, false);
            FixEffectOnAllLevels(3240, 0, (level, effect, critical) => level.MaxCastPerTarget = 1, false);

            //Maintenance I (3241)
            FixEffectOnAllLevels(3241, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL ^ SpellTargetType.ALLY_TURRETS);
            FixEffectOnAllLevels(3241, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);
            FixEffectOnAllLevels(3241, 2, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL ^ SpellTargetType.ALLY_TURRETS);
            FixEffectOnAllLevels(3241, 3, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);

            //Maintenance II (3242)
            FixEffectOnAllLevels(3242, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL ^ SpellTargetType.ALLY_TURRETS);
            FixEffectOnAllLevels(3242, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);
            FixEffectOnAllLevels(3242, 2, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL ^ SpellTargetType.ALLY_TURRETS);
            FixEffectOnAllLevels(3242, 3, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);

            //Maintenance III (3243)
            FixEffectOnAllLevels(3243, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL ^ SpellTargetType.ALLY_TURRETS);
            FixEffectOnAllLevels(3243, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);
            FixEffectOnAllLevels(3243, 2, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL ^ SpellTargetType.ALLY_TURRETS);
            FixEffectOnAllLevels(3243, 3, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);

            // Brise l'âme (3277)
            FixEffectOnAllLevels(3277, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_TURRETS, false);

            // Convergence (3280)
            FixEffectOnAllLevels(3280, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL ^ SpellTargetType.ALLY_TURRETS, false);

            // Evolution II (3281)
            FixEffectOnAllLevels(3281, 0, (level, effect, critical) => effect.Value = 3282, false);

            // Evolution III (3282)
            FixEffectOnAllLevels(3282, 0, (level, effect, critical) => effect.Value = 3281, false);
            FixEffectOnAllLevels(3282, 1, (level, effect, critical) => effect.Value = 3282, false);

            #endregion

            #region Monsters

            #region Boss

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

            #region Mansot Royal

            // Mansomure (2607)
            // remove effect
            RemoveEffectOnAllLevels(2607, 1, false);

            #endregion

            #region Glourséleste

            // Rattrapage (2261)
            // ENEMY_ALL -> ALLY_ALL
            FixEffectOnAllLevels(2261, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL, false);
            FixEffectOnAllLevels(2261, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL | SpellTargetType.SELF, false);
            FixEffectOnAllLevels(2261, 2, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL | SpellTargetType.SELF, false);

            #endregion

            #endregion

            #region Summon

            #region Chaton

            // Guigne (487)
            FixEffectOnAllLevels(487, EffectsEnum.Effect_HealHP_108, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL | SpellTargetType.SELF, false);

            #endregion

            #region Lapino

            // Lapino Boost (582)
            FixEffectOnAllLevels(582, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_SUMMONER);

            #endregion

            #region Tonneau

            // Beuverie (1674)
            FixEffectOnAllLevels(1674, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_SUMMONER, false);

            #endregion

            #region LifeTree

            // Soin Sylvestre (1687)
            FixEffectOnAllLevels(1687, 0, (level, effect, critical) => effect.Duration = -1, false);

            #endregion

            #region LivingChest

            // Prospection (495)
            FixEffectOnAllLevels(495, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL, false);
            FixEffectOnAllLevels(495, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL, false);
            FixEffectOnAllLevels(495, 2, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL, false);

            #endregion

            #endregion

            #region Monsters

            #region TOFU

            // béco du tofu (1999)
            // steal agility
            // target only self -> all
            FixEffectOnAllLevels(1999, EffectsEnum.Effect_StealAgility,
                (level, effect, critical) => effect.Targets = SpellTargetType.ALL);

            #endregion

            #region Boulglours

            // Saccharose (2255)
            // ENEMY_ALL => ONLY_SELF
            FixEffectOnAllLevels(2255, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ONLY_SELF, false);

            // Invertase (2485)
            // ENEMY_ALL => ALLY_ALL
            FixEffectOnAllLevels(2485, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL | SpellTargetType.SELF);

            #endregion

            #region Glouragan

            // Gloursonde (2487)
            // ENEMY_ALL => ALLY_ALL
            FixEffectOnAllLevels(2487, 0, (level, effect, critical) => effect.Targets = SpellTargetType.NONE);
            FixEffectOnAllLevels(2487, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL);

            // Glourdavu (2488)
            // ENEMY_ALL => ALLY_ALL
            FixEffectOnAllLevels(2488, 0, (level, effect, critical) => effect.Targets = SpellTargetType.NONE);
            FixEffectOnAllLevels(2488, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL);

            // Glours poursuite (2489)
            // ENEMY_ALL => ONLY_SELF
            FixEffectOnAllLevels(2489, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ONLY_SELF, false);

            // Gloursculade (2490)
            // ENEMY_ALL => ONLY_SELF
            FixEffectOnAllLevels(2490, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ONLY_SELF, false);

            #endregion

            #region Glourmand

            // Gloursbi-boulga (2510)
            // ENEMY_ALL => ONLY_SELF
            FixEffectOnAllLevels(2510, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ONLY_SELF, false);

            #endregion

            #region Gloursaya

            // Propolis (2258)
            // ENEMY_ALL => ALLY_ALL
            FixEffectOnAllLevels(2258, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL);

            #endregion

            #region Meliglours

            // Gloursombilical (2492)
            // ENEMY_ALL => ALLY_ALL
            FixEffectOnAllLevels(2492, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL);
            FixEffectOnAllLevels(2492, 1, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL);

            #endregion

            #region Fistulor

            // Ami Célium (2688)
            // NONE => ENEMY_ALL
            FixEffectOnAllLevels(2688, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ENEMY_ALL);

            // Spore Héole (2689)
            // NONE => ALLY_ALL
            FixEffectOnAllLevels(2689, 0, (level, effect, critical) => effect.Targets = SpellTargetType.ALLY_ALL, false);

            #endregion

            #endregion

            #endregion

        }

        public static void FixEffectOnAllLevels(int spellId, int effectIndex, Action<SpellLevelTemplate, EffectDice, bool> fixer, bool critical = true)
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

        public static void FixEffectOnAllLevels(int spellId, EffectsEnum effect, Action<SpellLevelTemplate, EffectDice, bool> fixer, bool critical = true)
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

        public static void FixCriticalEffectOnAllLevels(int spellId, int effectIndex, Action<SpellLevelTemplate, EffectDice, bool> fixer)
        {
            var spellLevels = SpellManager.Instance.GetSpellLevels(spellId).ToArray();

            if (spellLevels.Length == 0)
                throw new Exception(string.Format("Cannot apply fix on spell {0} : spell do not exists", spellId));

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

        public static void RemoveEffectOnAllLevels(int spellId, int effectIndex, bool critical = true)
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

        public static void RemoveEffectOnAllLevels(int spellId, EffectsEnum effect, bool critical = true)
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