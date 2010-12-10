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
namespace Stump.DofusProtocol.Enums
{
    public enum EffectsEnum
    {
        Effect_Teleport = 4,
        /* Teleports over a maximum range of #1 cells. */
        Effect_PushBack = 5,
        /* Pushes the target back #1 cell(s) */
        Effect_MoveForward = 6,
        /* Makes you move forward #1 square(s) */
        Effect_Divorce = 7,
        /* Get a divorce */
        Effect_SwitchPosition = 8,
        /* Switch the position of 2 players */
        Effect_9 = 9,
        /* Avoids #1% of hits by moving back of #2 square(s) */
        Effect_10 = 10,
        /* Allows the use of emoticon #3 */
        Effect_13 = 13,
        /* Changes the playing time of a player */
        Effect_34 = 34,
        /* Begins a quest */
        Effect_50 = 50,
        /* Carry a player */
        Effect_51 = 51,
        /* Throw a player */
        Effect_StealMP_77 = 77,
        /* Steals #1{~1~2 to }#2 MP */
        Effect_AddMP = 78,
        /* Adds #1{~1~2 to }#2 MP */
        Effect_79 = 79,
        /* #3% damage received x#1, or else healed by x#2 */
        Effect_HealHP_81 = 81,
        /* HP restored #1{~1~2 to }#2 */
        Effect_StealHPFix = 82,
        /* Steals #1{~1~2 to }#2 HP (fixed) */
        Effect_StealAP_84 = 84,
        /* Ste_Steals #1{~1~2 to }#2 AP */
        Effect_DamagePercentWater = 85,
        /* Damage: #1{~1~2 to }#2% of the attacker's life (water) */
        Effect_DamagePercentEarth = 86,
        /* Damage: #1{~1~2 to }#2% of the attacker's life (earth) */
        Effect_DamagePercentAir = 87,
        /* Damage: #1{~1~2 to }#2% of the attacker's life (air) */
        Effect_DamagePercentFire = 88,
        /* Damage: #1{~1~2 to }#2% of the attacker's life (fire) */
        Effect_DamagePercentNeutral = 89,
        /* Damage: #1{~1~2 to }#2% of the attacker's life (neutral) */
        Effect_GiveHPPercent = 90,
        /* Gives #1{~1~2 to }#2 % of his own life */
        Effect_StealHPWater = 91,
        /* Steals #1{~1~2 to }#2 HP (water) */
        Effect_StealHPEarth = 92,
        /* Steals #1{~1~2 to }#2 HP (earth) */
        Effect_StealHPAir = 93,
        /* Steals #1{~1~2 to }#2 HP (air) */
        Effect_StealHPFire = 94,
        /* Steals #1{~1~2 to }#2 HP (fire) */
        Effect_StealHPNeutral = 95,
        /* Steals #1{~1~2 to }#2 HP (neutral) */
        Effect_DamageWater = 96,
        /* Damage: #1{~1~2 to }#2 (water) */
        Effect_DamageEarth = 97,
        /* Damage: #1{~1~2 to }#2 (earth) */
        Effect_DamageAir = 98,
        /* Damage: #1{~1~2 to }#2 (air) */
        Effect_DamageFire = 99,
        /* Damage: #1{~1~2 to }#2 (fire) */
        Effect_DamageNeutral = 100,
        /* Damage: #1{~1~2 to }#2 (neutral) */
        Effect_MakeLostAP = 101,
        /* Lost AP for the target: #1{~1~2 to }#2 */
        Effect_AddGlobalDamageReduction_105 = 105,
        /* Damage reduced by #1{~1~2 to }#2 */
        Effect_ReflectSpell = 106,
        /* Reflects a spell, max. of level #2 */
        Effect_AddDamageReflection = 107,
        /* Reflects #1{~1~2 to }#2 damage */
        Effect_HealHP_108 = 108,
        /* HP restored #1{~1~2 to }#2 */
        Effect_109 = 109,
        /* Damage to the caster: #1{~1~2 to }#2 */
        Effect_AddHealth = 110,
        /* +#1{~1~2 to }#2 life */
        Effect_AddAP_111 = 111,
        /* +#1{~1~2 to }#2 AP */
        Effect_AddDamageBonus = 112,
        /* +#1{~1~2 to }#2 damage */
        Effect_DoubleDamageOrRestoreHP = 113,
        /* Doubles damage or restores #1{~1~2 to }#2 HP */
        Effect_AddDamageMultiplicator = 114,
        /* Multiply damage by #1 */
        Effect_AddCriticalHit = 115,
        /* +#1{~1~2 to }#2 critical hits */
        Effect_SubRange = 116,
        /* -#1{~1~2 to }#2 range */
        Effect_AddRange = 117,
        /* +#1{~1~2 to }#2 range */
        Effect_AddStrength = 118,
        /* +#1{~1~2 to }#2 strength */
        Effect_AddAgility = 119,
        /* +#1{~1~2 to }#2 agility */
        Effect_AddAP_120 = 120,
        /* Adds +#1{~1~2 to }#2 AP */
        Effect_AddDamageBonus_121 = 121,
        /* +#1{~1~2 to }#2 damage */
        Effect_AddCriticalMiss = 122,
        /* Adds #1{~1~2 to }#2 to critical failures */
        Effect_AddChance = 123,
        /* +#1{~1~2 to }#2 chance */
        Effect_AddWisdom = 124,
        /* +#1{~1~2 to }#2 wisdom */
        Effect_AddVitality = 125,
        /* +#1{~1~2 to }#2 vitality */
        Effect_AddIntelligence = 126,
        /* +#1{~1~2 to }#2 intelligence */
        Effect_LostMP = 127,
        /* MP lost: #1{~1~2 to }#2 */
        Effect_AddMP_128 = 128,
        /* +#1{~1~2 to }#2 MP */
        Effect_StealKamas = 130,
        /* Steals #1{~1~2 to }#2 Kamas */
        Effect_LoseHPByUsingAP = 131,
        /* Using #1 AP makes you lose #2 HP */
        Effect_DispelMagicEffects = 132,
        /* Dispels magic effects */
        Effect_LosingAP = 133,
        /* Lost AP for caster: #1{~1~2 to }#2 */
        Effect_LosingMP = 134,
        /* Lost MP for caster: #1{~1~2 to }#2 */
        Effect_SubRange_135 = 135,
        /* Caster's range reduced by: #1{~1~2 to }#2 */
        Effect_AddRange_136 = 136,
        /* Caster's range increased by: #1{~1~2 to }#2 */
        Effect_AddPhysicalDamage_137 = 137,
        /* Caster's physical damage increased by : #1{~1~2 to }#2 */
        Effect_IncreaseDamage_138 = 138,
        /* Increases damage by #1{~1~2 to }#2% */
        Effect_RestoreEnergyPoints = 139,
        /* Restores #1{~1~2 to }#2 energy points */
        Effect_SkipTurn = 140,
        /* Makes you skip a turn */
        Effect_Kill = 141,
        /* Kills the target */
        Effect_AddPhysicalDamage_142 = 142,
        /* +#1{~1~2 to }#2 to physical damage */
        Effect_HealHP_143 = 143,
        /* HP restored: #1{~1~2 to }#2 */
        Effect_DamageFix = 144,
        /* Damage: #1{~1~2 to }#2 (unboosted) */
        Effect_SubDamageBonus = 145,
        /* -#1{~1~2 to }#2 to damage */
        Effect_ChangesWords = 146,
        /* Changes the words */
        Effect_ReviveAlly = 147,
        /* Revive an ally */
        Effect_Followed = 148,
        /* Someone's following you! */
        Effect_ChangeAppearance = 149,
        /* Changes appearance */
        Effect_Invisibility = 150,
        /* Makes the character invisible */
        Effect_SubChance = 152,
        /* -#1{~1~2 to }#2 chance */
        Effect_SubVitality = 153,
        /* -#1{~1~2 to }#2 vitality */
        Effect_SubAgility = 154,
        /* -#1{~1~2 to }#2 agility */
        Effect_SubIntelligence = 155,
        /* -#1{~1~2 to }#2 intelligence */
        Effect_SubWisdom = 156,
        /* -#1{~1~2 to }#2 wisdom */
        Effect_SubStrength = 157,
        /* -#1{~1~2 to }#2 strength */
        Effect_IncreaseWeight = 158,
        /* Increases load weight by #1{~1~2 to }#2 pods */
        Effect_DecreaseWeight = 159,
        /* Decreases load weight by #1{~1~2 to }#2 pods */
        Effect_IncreaseAPAvoid = 160,
        /* Increases chance of avoiding AP loss by #1{~1~2 to }#2% */
        Effect_IncreaseMPAvoid = 161,
        /* Increases chance of avoiding MP loss by #1{~1~2 to }#2% */
        Effect_SubDodgeAPProbability = 162,
        /* -#1{~1~2 to}#2 chance of avoiding AP losses */
        Effect_SubDodgeMPProbability = 163,
        /* -#1{~1~2 to}#2 chance of avoiding MP losses */
        Effect_AddGlobalDamageReduction = 164,
        /* Damage reduced by #1% */
        Effect_AddDamageBonusPercent = 165,
        /* Increases (#1) damage by #2% */
        Effect_166 = 166,
        /* AP given back: #1{~1~2 to }#2 */
        Effect_SubAP = 168,
        /* -#1{~1~2 to }#2 AP */
        Effect_SubMP = 169,
        /* -#1{~1~2 to }#2 MP */
        Effect_SubCriticalHit = 171,
        /* -#1{~1~2 to }#2 critical hits */
        Effect_SubMagicDamageReduction = 172,
        /* Magic reduction decreased by #1{~1~2 to }#2 */
        Effect_SubPhysicalDamageReduction = 173,
        /* Physical reduction decreased by #1{~1~2 to }#2 */
        Effect_AddInitiative = 174,
        /* +#1{~1~2 to }#2 initiative */
        Effect_SubInitiative = 175,
        /* -#1{~1~2 to }#2 initiative */
        Effect_AddProspecting = 176,
        /* +#1{~1~2 to }#2 prospecting */
        Effect_SubProspecting = 177,
        /* -#1{~1~2 to }#2 prospecting */
        Effect_AddHealBonus = 178,
        /* +#1{~1~2 to }#2 heals */
        Effect_SubHealBonus = 179,
        /* -#1{~1~2 to }#2 heals */
        Effect_Double = 180,
        /* Creates a double of the caster */
        Effect_Summon = 181,
        /* Summons: #1 */
        Effect_AddSummonLimit = 182,
        /* +#1{~1~2 to }#2 to summonable creatures */
        Effect_AddMagicDamageReduction = 183,
        /* Magic reduction of #1{~1~2 to }#2 */
        Effect_AddPhysicalDamageReduction = 184,
        /* Physical reduction of #1{~1~2 to }#2 */
        Effect_185 = 185,
        /* Summons a static creature */
        Effect_SubDamageBonusPercent = 186,
        /* Decreases damage by #1{~1~2 to }#2% */
        Effect_188 = 188,
        /* Switches alignment */
        Effect_194 = 194,
        /* Gain #1{~1~2 to }#2 Kamas */
        Effect_197 = 197,
        /* Transform into #1 */
        Effect_201 = 201,
        /* Put an item on the ground */
        Effect_202 = 202,
        /* Reveals all invisible items */
        Effect_206 = 206,
        /* Revive the target */
        Effect_AddEarthResistPercent = 210,
        /* #1{~1~2 to }#2 % earth resistance */
        Effect_AddWaterResistPercent = 211,
        /* #1{~1~2 to }#2 % water resistance */
        Effect_AddAirResistPercent = 212,
        /* #1{~1~2 to }#2 % air resistance */
        Effect_AddFireResistPercent = 213,
        /* #1{~1~2 to }#2 % fire resistance */
        Effect_AddNeutralResistPercent = 214,
        /* #1{~1~2 to }#2 % neutral resistance */
        Effect_SubEarthResistPercent = 215,
        /* #1{~1~2 to }#2 % earth weakness */
        Effect_SubWaterResistPercent = 216,
        /* #1{~1~2 to }#2 % water weakness */
        Effect_SubAirResistPercent = 217,
        /* #1{~1~2 to }#2 % air weakness */
        Effect_SubFireResistPercent = 218,
        /* #1{~1~2 to }#2 % fire weakness */
        Effect_SubNeutralResistPercent = 219,
        /* #1{~1~2 to }#2 % neutral weakness */
        Effect_220 = 220,
        /* Reflects #1 damage */
        Effect_221 = 221,
        /* What's in there? */
        Effect_222 = 222,
        /* What's in there? */
        Effect_AddTrapBonus = 225,
        /* Adds #1{~1~2 to }#2 to trap damage */
        Effect_AddTrapBonusPercent = 226,
        /* +#1{~1~2 to }#2% damage to traps */
        Effect_229 = 229,
        /* Get a mount! */
        Effect_230 = 230,
        /* +#1 of lost energy */
        Effect_239 = 239,
        /*  */
        Effect_AddEarthElementReduction = 240,
        /* +#1{~1~2 to }#2 earth resistance */
        Effect_AddWaterElementReduction = 241,
        /* +#1{~1~2 to }#2 water resistance */
        Effect_AddAirElementReduction = 242,
        /* +#1{~1~2 to }#2 air resistance */
        Effect_AddFireElementReduction = 243,
        /* +#1{~1~2 to }#2 fire resistance */
        Effect_AddNeutralElementReduction = 244,
        /* +#1{~1~2 to }#2 neutral resistance */
        Effect_SubEarthElementReduction = 245,
        /* -#1{~1~2 to }#2 earth resistance */
        Effect_SubWaterElementReduction = 246,
        /* -#1{~1~2 to }#2 water resistance */
        Effect_SubAirElementReduction = 247,
        /* -#1{~1~2 to }#2 air resistance */
        Effect_SubFireElementReduction = 248,
        /* -#1{~1~2 to }#2 fire resistance */
        Effect_SubNeutralElementReduction = 249,
        /* -#1{~1~2 to }#2 neutral resistance */
        Effect_AddPvpEarthResistPercent = 250,
        /* #1{~1~2 to }#2% earth resistance against fighters */
        Effect_AddPvpWaterResistPercent = 251,
        /* #1{~1~2 to }#2 % water resistance against fighters */
        Effect_AddPvpAirResistPercent = 252,
        /* #1{~1~2 to }#2 % air resistance against fighters */
        Effect_AddPvpFireResistPercent = 253,
        /* #1{~1~2 to }#2 % fire resistance against fighters */
        Effect_AddPvpNeutralResistPercent = 254,
        /* #1{~1~2 to }#2 % neutral resistance against fighters */
        Effect_SubPvpEarthResistPercent = 255,
        /* #1{~1~2 to }#2 % earth weakness against fighters */
        Effect_SubPvpWaterResistPercent = 256,
        /* #1{~1~2 to }#2 % water weakness against fighters */
        Effect_SubPvpAirResistPercent = 257,
        /* #1{~1~2 to }#2 % air weakness against fighters */
        Effect_SubPvpFireResistPercent = 258,
        /* #1{~1~2 to }#2 % fire weakness against fighters */
        Effect_SubPvpNeutralResistPercent = 259,
        /* #1{~1~2 to }#2 % neutral weakness against fighters */
        Effect_AddPvpEarthElementReduction = 260,
        /* +#1{~1~2 to }#2 earth resistance against fighters */
        Effect_AddPvpWaterElementReduction = 261,
        /* +#1{~1~2 to }#2 water resistance against fighters */
        Effect_AddPvpAirElementReduction = 262,
        /* Adds #1{~1~2 to }#2 air resistance against fighters */
        Effect_AddPvpFireElementReduction = 263,
        /* +#1{~1~2 to }#2 fire resistance against fighters */
        Effect_AddPvpNeutralElementReduction = 264,
        /* +#1{~1~2 to }#2 neutral resistance against fighters */
        Effect_AddGlobalDamageReduction_265 = 265,
        /* Damage reduced by #1{~1~2 to }#2 */
        Effect_266 = 266,
        /* #1{~1~2 to }#2 Chance theft */
        Effect_267 = 267,
        /* #1{~1~2 to }#2 Vitality theft */
        Effect_268 = 268,
        /* #1{~1~2 to }#2 Agility theft */
        Effect_269 = 269,
        /* #1{~1~2 to }#2 Intelligence theft */
        Effect_270 = 270,
        /* #1{~1~2 to }#2 Wisdom theft */
        Effect_271 = 271,
        /* #1{~1~2 to }#2 Strength theft */
        Effect_275 = 275,
        /* Damage: #1{~1~2 to }#2% of the attacker's lost HP (water) */
        Effect_276 = 276,
        /* Damage: #1{~1~2 to }#2% of the attacker's lost HP (earth) */
        Effect_277 = 277,
        /* Damage: #1{~1~2 to }#2% of the attacker's lost HP (air) */
        Effect_278 = 278,
        /* Damage: #1{~1~2 to }#2% of the attacker's lost HP (fire) */
        Effect_279 = 279,
        /* Damage: #1{~1~2 to }#2% of the attacker's lost HP (neutral) */
        Effect_281 = 281,
        /* Increases #1's range by #3 */
        Effect_282 = 282,
        /* Makes it possible to modify #1's range */
        Effect_283 = 283,
        /* Adds #3 to #1's damage */
        Effect_284 = 284,
        /* Adds #3 to #1's heals */
        Effect_285 = 285,
        /* Reduces #1's AP cost by #3 */
        Effect_286 = 286,
        /* Reduces #1's cooldown period by #3 */
        Effect_287 = 287,
        /* Adds #3 to #1's Critical Hits */
        Effect_288 = 288,
        /* #1 no longer has to be cast in a straight line */
        Effect_289 = 289,
        /* #1 no longer needs line of sight */
        Effect_290 = 290,
        /* Increases the maximum number of times #1 can be cast per turn by #3 */
        Effect_291 = 291,
        /* Increases the maximum number of times #1 can be cast per target by #3 */
        Effect_292 = 292,
        /* #1's cooldown period is set to #3 */
        Effect_293 = 293,
        /* Increases #1's basic damage by #3 */
        Effect_294 = 294,
        /* Reduces #1's range by #3 */
        Effect_310 = 310,
        /*  */
        Effect_320 = 320,
        /* Steals #1{~1~2 to }#2 range */
        Effect_333 = 333,
        /* Change a colour */
        Effect_335 = 335,
        /* Change appearance */
        Effect_400 = 400,
        /* Sets a grade #2 trap */
        Effect_401 = 401,
        /* Sets a grade #2 glyph */
        Effect_402 = 402,
        /* Sets a grade #2 glyph */
        Effect_405 = 405,
        /* Kills and replaces with a summon */
        Effect_406 = 406,
        /* Removes the effects of %1 */
        Effect_407 = 407,
        /* HP restored: #1{~1~2 to }#2 */
        Effect_410 = 410,
        /* +#1{~1~2 to }#2 AP attack */
        Effect_411 = 411,
        /* -#1{~1~2 to }#2 AP attack */
        Effect_412 = 412,
        /* +#1{~1~2 to }#2 MP attack */
        Effect_413 = 413,
        /* -#1{~1~2 to }#2 MP attack */
        Effect_AddPushDamageBonus = 414,
        /* +#1{~1~2 to }#2 pushback damage */
        Effect_SubPushDamageBonus = 415,
        /* -#1{~1~2 to }#2 pushback damage */
        Effect_AddPushDamageReduction = 416,
        /* +#1{~1~2 to }#2 pushback resistance */
        Effect_SubPushDamageReduction = 417,
        /* -#1{~1~2 to }#2 pushback resistance */
        Effect_AddCriticalDamageBonus = 418,
        /* +#1{~1~2 to }#2 critical damage */
        Effect_SubCriticalDamageBonus = 419,
        /* -#1{~1~2 to }#2 critical damage */
        Effect_AddCriticalDamageReduction = 420,
        /* +#1{~1~2 to }#2 critical resistance */
        Effect_SubCriticalDamageReduction = 421,
        /* -#1{~1~2 to }#2 critical resistance */
        Effect_AddEarthDamageBonus = 422,
        /* +#1{~1~2 to }#2 Earth damage */
        Effect_SubEarthDamageBonus = 423,
        /* -#1{~1~2 to }#2 Earth damage */
        Effect_AddFireDamageBonus = 424,
        /* +#1{~1~2 to }#2 Fire damage */
        Effect_SubFireDamageBonus = 425,
        /* -#1{~1~2 to }#2 Fire damage */
        Effect_AddWaterDamageBonus = 426,
        /* +#1{~1~2 to }#2 Water damage */
        Effect_SubWaterDamageBonus = 427,
        /* -#1{~1~2 to }#2 Water damage */
        Effect_AddAirDamageBonus = 428,
        /* +#1{~1~2 to }#2 Air damage */
        Effect_SubAirDamageBonus = 429,
        /* -#1{~1~2 to }#2 Air damage */
        Effect_AddNeutralDamageBonus = 430,
        /* +#1{~1~2 to }#2 Neutral damage */
        Effect_SubNeutralDamageBonus = 431,
        /* -#1{~1~2 to }#2 Neutral damage */
        Effect_StealAP_440 = 440,
        /* Steals #1{~1~2 to }#2 AP */
        Effect_StealMP_441 = 441,
        /* Steals #1{~1~2 to }#2 MP */
        Effect_513 = 513,
        /* Place a prism */
        Effect_600 = 600,
        /* Teleport to save point */
        Effect_601 = 601,
        /*  */
        Effect_602 = 602,
        /* Save your position */
        Effect_603 = 603,
        /* Learn the #3 profession */
        Effect_604 = 604,
        /* Learn the spell #3 */
        Effect_605 = 605,
        /* +#1{~1~2 to }#2 XP points */
        Effect_606 = 606,
        /* +#1{~1~2 to }#2 wisdom */
        Effect_607 = 607,
        /* +#1{~1~2 to }#2 strength */
        Effect_608 = 608,
        /* +#1{~1~2 to }#2 chance */
        Effect_609 = 609,
        /* +#1{~1~2 to }#2 agility */
        Effect_610 = 610,
        /* +#1{~1~2 to }#2 vitality */
        Effect_611 = 611,
        /* +#1{~1~2 to }#2 intelligence */
        Effect_612 = 612,
        /* +#1{~1~2 to }#2 characteristic points */
        Effect_613 = 613,
        /* +#1{~1~2 to }#2 spell points */
        Effect_614 = 614,
        /* +#1 XP for profession #2 */
        Effect_615 = 615,
        /* Makes you forget the profession #3 */
        Effect_616 = 616,
        /* Makes you forget one level of the spell #3 */
        Effect_620 = 620,
        /* Consult #3 */
        Effect_621 = 621,
        /* Summon: #3 (level #1) */
        Effect_622 = 622,
        /* Teleport to your house */
        Effect_623 = 623,
        /* Summons: #3 */
        Effect_624 = 624,
        /* Makes you forget one level of the spell #3 */
        Effect_625 = 625,
        /*  */
        Effect_626 = 626,
        /*  */
        Effect_627 = 627,
        /* Recreates original map */
        Effect_628 = 628,
        /* Summons: #3 */
        Effect_631 = 631,
        /*  */
        Effect_640 = 640,
        /* Adds #3 Honour points */
        Effect_641 = 641,
        /* Adds #3 Disgrace points */
        Effect_642 = 642,
        /* Withdraws #3 Honour points */
        Effect_643 = 643,
        /* Withdraws #3 Disgrace points */
        Effect_645 = 645,
        /* Resuscitates allies on your map */
        Effect_646 = 646,
        /* Restored HP: #1{~1~2 to }#2 */
        Effect_647 = 647,
        /* Frees enemy souls */
        Effect_648 = 648,
        /* Frees an enemy soul */
        Effect_649 = 649,
        /* Pretend to be #3 */
        Effect_654 = 654,
        /*  */
        Effect_666 = 666,
        /* No additional effects */
        Effect_669 = 669,
        /* Incarnation level #3 */
        Effect_670 = 670,
        /* Damage: #1{~1~2 to }#2% of the attacker's HP (Neutral-type) */
        Effect_671 = 671,
        /* Damage: #1{~1~2 to }#2% of the attacker's HP (Neutral-type) */
        Effect_672 = 672,
        /* Damage: #1{~1~2 to }#2% of the attacker's life (neutral) */
        Effect_699 = 699,
        /* Link a job: #1 */
        Effect_700 = 700,
        /* Change the attack element */
        Effect_701 = 701,
        /* Power: #1{~1~2 to }#2 */
        Effect_702 = 702,
        /* +#1{~1~2 to }#2 durability points */
        Effect_705 = 705,
        /* #1% chance of capturing a power #3 soul */
        Effect_706 = 706,
        /* #1% chance of capturing a mount */
        Effect_707 = 707,
        /* Use custom set nÂ°#3 */
        Effect_710 = 710,
        /* Additional cost */
        Effect_715 = 715,
        /* #1 : #3 */
        Effect_716 = 716,
        /* #1 : #3 */
        Effect_717 = 717,
        /* #1 : #3 */
        Effect_720 = 720,
        /* Number of victims: #2 */
        Effect_724 = 724,
        /* Unlock the title #3 */
        Effect_725 = 725,
        /* Rename guild: #4 */
        Effect_730 = 730,
        /* Teleport to the nearest allied prism */
        Effect_731 = 731,
        /* Attack players of the opposite alignment automatically */
        Effect_732 = 732,
        /* Resistance to automatic attacks from enemy players: #1{~1~2 to }#2 */
        Effect_740 = 740,
        /*  */
        Effect_741 = 741,
        /*  */
        Effect_742 = 742,
        /*  */
        Effect_750 = 750,
        /* Increases chance of capture by: #1{~1~2 to }#2% */
        Effect_751 = 751,
        /* Bonus to Dragoturkey XP: #1{~1~2 to }#2% */
        Effect_752 = 752,
        /* Dodge bonus: #1{~1~2 to }#2 */
        Effect_753 = 753,
        /* Lock bonus: #1{~1~2 to }#2 */
        Effect_754 = 754,
        /* Dodge penalty: #1{~1~2 to }#2 */
        Effect_755 = 755,
        /* Lock penalty: #1{~1~2 to }#2 */
        Effect_760 = 760,
        /* Disappear by moving */
        Effect_765 = 765,
        /* Switch the position of 2 players */
        Effect_770 = 770,
        /* Clockwise confusion: #1{~1~2 to }#2 degrees */
        Effect_771 = 771,
        /* Clockwise confusion: #1{~1~2 to }#2 Pi/2 */
        Effect_772 = 772,
        /* Clockwise confusion: #1{~1~2 to }#2 Pi/4 */
        Effect_773 = 773,
        /* Anticlockwise confusion: #1{~1~2 to }#2 degrees */
        Effect_774 = 774,
        /* Anticlockwise confusion: #1{~1~2 to }#2 Pi/2 */
        Effect_775 = 775,
        /* Anticlockwise confusion: #1{~1~2 to }#2 Pi/4 */
        Effect_776 = 776,
        /* Increases permanent damage taken by #1{~1~2 to }#2% */
        Effect_780 = 780,
        /* Summons the last ally who died with #1{~1~2 to }#2 % of their HP */
        Effect_781 = 781,
        /* Minimizes random effects */
        Effect_782 = 782,
        /* Maximizes random effects */
        Effect_783 = 783,
        /* Repels to the targeted cell */
        Effect_784 = 784,
        /* Return to original position */
        Effect_785 = 785,
        /*  */
        Effect_786 = 786,
        /* Heals upon attack */
        Effect_787 = 787,
        /* #1 */
        Effect_788 = 788,
        /* Punishment of #2 for #3 turn(s) */
        Effect_789 = 789,
        /*  */
        Effect_790 = 790,
        /*  */
        Effect_791 = 791,
        /* Prepare #1{~1~2 to }#2 mercenary scrolls */
        Effect_792 = 792,
        /* #1 */
        Effect_793 = 793,
        /* #1 */
        Effect_795 = 795,
        /* Hunting Weapon */
        Effect_800 = 800,
        /* Health points: #3 */
        Effect_805 = 805,
        /* Received on: #1 */
        Effect_806 = 806,
        /* State: #1 */
        Effect_807 = 807,
        /* Last meal: #1 */
        Effect_808 = 808,
        /* Last meal: #1 */
        Effect_810 = 810,
        /* Size: #3 squares */
        Effect_811 = 811,
        /* Remaining turn(s): #3 */
        Effect_812 = 812,
        /* Durability: #2 / #3 */
        Effect_813 = 813,
        /*  */
        Effect_814 = 814,
        /* #1 */
        Effect_815 = 815,
        /*  */
        Effect_816 = 816,
        /*  */
        Effect_825 = 825,
        /* Teleport */
        Effect_905 = 905,
        /* Start a fight against #2 */
        Effect_930 = 930,
        /* Increases serenity, decreases aggressiveness */
        Effect_931 = 931,
        /* Improves aggressiveness, decreases serenity */
        Effect_932 = 932,
        /* Increases stamina */
        Effect_933 = 933,
        /* Decreases stamina */
        Effect_934 = 934,
        /* Increases love */
        Effect_935 = 935,
        /* Decreases love */
        Effect_936 = 936,
        /* Speeds maturity */
        Effect_937 = 937,
        /* Slows down maturity */
        Effect_939 = 939,
        /* Increases the capacity of a pet #3 . */
        Effect_940 = 940,
        /* Improved abilities */
        Effect_946 = 946,
        /* Temporarily remove a Breeding item */
        Effect_947 = 947,
        /* Remove an item from a Paddock */
        Effect_948 = 948,
        /* Paddock Item */
        Effect_949 = 949,
        /* Get on/off a mount */
        Effect_950 = 950,
        /* #3 state */
        Effect_951 = 951,
        /* Removes #3 state */
        Effect_952 = 952,
        /* #3 state deactivated */
        Effect_960 = 960,
        /* Alignment: #3 */
        Effect_961 = 961,
        /* Rank: #3 */
        Effect_962 = 962,
        /* Level: #3 */
        Effect_963 = 963,
        /* Created #3 day(s) ago */
        Effect_964 = 964,
        /* Name: #4 */
        Effect_970 = 970,
        /*  */
        Effect_971 = 971,
        /*  */
        Effect_972 = 972,
        /*  */
        Effect_973 = 973,
        /*  */
        Effect_974 = 974,
        /*  */
        Effect_981 = 981,
        /* Non-exchangeable */
        Effect_982 = 982,
        /* Non-exchangeable */
        Effect_983 = 983,
        /* Can be exchanged from: #1 */
        Effect_984 = 984,
        /*  */
        Effect_985 = 985,
        /* Modified by: #4 */
        Effect_986 = 986,
        /* Prepares #1{~1~2 to }#2 scrolls */
        Effect_987 = 987,
        /* Belongs to: #4 */
        Effect_988 = 988,
        /* Made by: #4 */
        Effect_989 = 989,
        /* Seeks: #4 */
        Effect_990 = 990,
        /* #4 */
        Effect_994 = 994,
        /* !! Invalid Certificate !! */
        Effect_995 = 995,
        /* View mount characteristics */
        Effect_996 = 996,
        /* Belongs to: #4 */
        Effect_997 = 997,
        /* Name: #4 */
        Effect_998 = 998,
        /* Validity: #1d #2h #3m */
        Effect_999 = 999,
        /*  */
        Effect_1002 = 1002,
        /* 2 */
        End,
    }
}