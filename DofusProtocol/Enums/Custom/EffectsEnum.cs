namespace Stump.DofusProtocol.Enums
{
    public enum EffectsEnum : short
    {
        /// <summary>
        /// Teleports over a maximum range of #1 cells.
        /// </summary>
        Effect_Teleport = 4,
        /// <summary>
        /// Pushes the target back #1 cell(s)
        /// </summary>
        Effect_PushBack = 5,
        /// <summary>
        /// Makes you move forward #1 square(s)
        /// </summary>
        Effect_PullForward = 6,
        /// <summary>
        /// Get a divorce
        /// </summary>
        Effect_Divorce = 7,
        /// <summary>
        /// Switch the position of 2 players
        /// </summary>
        Effect_SwitchPosition = 8,
        /// <summary>
        /// Avoids #1% of hits by moving back of #2 square(s)
        /// </summary>
        Effect_Dodge = 9,
        /// <summary>
        /// Allows the use of emoticon #3
        /// </summary>
        Effect_10 = 10,
        /// <summary>
        /// Changes the playing time of a player
        /// </summary>
        Effect_13 = 13,
        /// <summary>
        /// Begins a quest
        /// </summary>
        Effect_34 = 34,
        /// <summary>
        /// Carry a player
        /// </summary>
        Effect_Carry = 50,
        /// <summary>
        /// Throw a player
        /// </summary>
        Effect_Throw = 51,
        /// <summary>
        /// Steals #1{~1~2 to }#2 MP
        /// </summary>
        Effect_StealMP_77 = 77,
        /// <summary>
        /// Adds #1{~1~2 to }#2 MP
        /// </summary>
        Effect_AddMP = 78,
        /// <summary>
        /// #3% damage received x#1, or else healed by x#2
        /// </summary>
        Effect_HealOrMultiply = 79,
        /// <summary>
        /// HP restored #1{~1~2 to }#2
        /// </summary>
        Effect_HealHP_81 = 81,
        /// <summary>
        /// Steals #1{~1~2 to }#2 HP (fixed)
        /// </summary>
        Effect_StealHPFix = 82,
        /// <summary>
        /// Ste_Steals #1{~1~2 to }#2 AP
        /// </summary>
        Effect_StealAP_84 = 84,
        /// <summary>
        /// Damage: #1{~1~2 to }#2% of the attacker's life (water)
        /// </summary>
        Effect_DamagePercentWater = 85,
        /// <summary>
        /// Damage: #1{~1~2 to }#2% of the attacker's life (earth)
        /// </summary>
        Effect_DamagePercentEarth = 86,
        /// <summary>
        /// Damage: #1{~1~2 to }#2% of the attacker's life (air)
        /// </summary>
        Effect_DamagePercentAir = 87,
        /// <summary>
        /// Damage: #1{~1~2 to }#2% of the attacker's life (fire)
        /// </summary>
        Effect_DamagePercentFire = 88,
        /// <summary>
        /// Damage: #1{~1~2 to }#2% of the attacker's life (neutral)
        /// </summary>
        Effect_DamagePercentNeutral = 89,
        /// <summary>
        /// Gives #1{~1~2 to }#2 % of his own life
        /// </summary>
        Effect_GiveHPPercent = 90,
        /// <summary>
        /// Steals #1{~1~2 to }#2 HP (water)
        /// </summary>
        Effect_StealHPWater = 91,
        /// <summary>
        /// Steals #1{~1~2 to }#2 HP (earth)
        /// </summary>
        Effect_StealHPEarth = 92,
        /// <summary>
        /// Steals #1{~1~2 to }#2 HP (air)
        /// </summary>
        Effect_StealHPAir = 93,
        /// <summary>
        /// Steals #1{~1~2 to }#2 HP (fire)
        /// </summary>
        Effect_StealHPFire = 94,
        /// <summary>
        /// Steals #1{~1~2 to }#2 HP (neutral)
        /// </summary>
        Effect_StealHPNeutral = 95,
        /// <summary>
        /// Damage: #1{~1~2 to }#2 (water)
        /// </summary>
        Effect_DamageWater = 96,
        /// <summary>
        /// Damage: #1{~1~2 to }#2 (earth)
        /// </summary>
        Effect_DamageEarth = 97,
        /// <summary>
        /// Damage: #1{~1~2 to }#2 (air)
        /// </summary>
        Effect_DamageAir = 98,
        /// <summary>
        /// Damage: #1{~1~2 to }#2 (fire)   
        /// </summary>
        Effect_DamageFire = 99,
        /// <summary>
        /// Damage: #1{~1~2 to }#2 (neutral)
        /// </summary>
        Effect_DamageNeutral = 100,
        /// <summary>
        /// Lost AP for the target: #1{~1~2 to }#2
        /// </summary>
        Effect_RemoveAP = 101,
        /// <summary>
        /// Damage reduced by #1{~1~2 to }#2
        /// </summary>
        Effect_AddGlobalDamageReduction_105 = 105,
        /// <summary>
        /// Reflects a spell, max. of level #2
        /// </summary>
        Effect_ReflectSpell = 106,
        /// <summary>
        /// Reflects #1{~1~2 to }#2 damage
        /// </summary>
        Effect_AddDamageReflection = 107,
        /// <summary>
        /// HP restored #1{~1~2 to }#2
        /// </summary>
        Effect_HealHP_108 = 108,
        /// <summary>
        /// Damage to the caster: #1{~1~2 to }#2
        /// </summary>
        Effect_109 = 109,
        /// <summary>
        /// +#1{~1~2 to }#2 life
        /// </summary>
        Effect_AddHealth = 110,
        /// <summary>
        /// +#1{~1~2 to }#2 AP
        /// </summary>
        Effect_AddAP_111 = 111,
        /// <summary>
        /// +#1{~1~2 to }#2 damage
        /// </summary>
        Effect_AddDamageBonus = 112,
        /// <summary>
        /// Doubles damage or restores #1{~1~2 to }#2 HP
        /// </summary>
        Effect_DoubleDamageOrRestoreHP = 113,
        /// <summary>
        /// Multiply damage by #1
        /// </summary>
        Effect_AddDamageMultiplicator = 114,
        /// <summary>
        /// +#1{~1~2 to }#2 critical hits
        /// </summary>
        Effect_AddCriticalHit = 115,
        /// <summary>
        /// -#1{~1~2 to }#2 range
        /// </summary>
        Effect_SubRange = 116,
        /// <summary>
        /// +#1{~1~2 to }#2 range
        /// </summary>
        Effect_AddRange = 117,
        /// <summary>
        /// +#1{~1~2 to }#2 strength
        /// </summary>
        Effect_AddStrength = 118,
        /// <summary>
        /// +#1{~1~2 to }#2 agility
        /// </summary>
        Effect_AddAgility = 119,
        /// <summary>
        /// Adds +#1{~1~2 to }#2 AP
        /// </summary>
        Effect_RegainAP = 120,
        /// <summary>
        /// +#1{~1~2 to }#2 damage
        /// </summary>
        Effect_AddDamageBonus_121 = 121,
        /// <summary>
        /// Adds #1{~1~2 to }#2 to critical failures
        /// </summary>
        Effect_AddCriticalMiss = 122,
        /// <summary>
        /// +#1{~1~2 to }#2 chance
        /// </summary>
        Effect_AddChance = 123,
        /// <summary>
        /// +#1{~1~2 to }#2 wisdom
        /// </summary>
        Effect_AddWisdom = 124,
        /// <summary>
        /// +#1{~1~2 to }#2 vitality
        /// </summary>
        Effect_AddVitality = 125,
        /// <summary>
        /// +#1{~1~2 to }#2 intelligence
        /// </summary>
        Effect_AddIntelligence = 126,
        /// <summary>
        /// MP lost: #1{~1~2 to }#2
        /// </summary>
        Effect_LostMP = 127,
        /// <summary>
        /// +#1{~1~2 to }#2 MP
        /// </summary>
        Effect_AddMP_128 = 128,
        /// <summary>
        /// Steals #1{~1~2 to }#2 Kamas
        /// </summary>
        Effect_StealKamas = 130,
        /// <summary>
        /// Using #1 AP makes you lose #2 HP
        /// </summary>
        Effect_LoseHPByUsingAP = 131,
        /// <summary>
        /// Dispels magic effects
        /// </summary>
        Effect_DispelMagicEffects = 132,
        /// <summary>
        /// Lost AP for caster: #1{~1~2 to }#2
        /// </summary>
        Effect_LosingAP = 133,
        /// <summary>
        /// Lost MP for caster: #1{~1~2 to }#2
        /// </summary>
        Effect_LosingMP = 134,
        /// <summary>
        /// Caster's range reduced by: #1{~1~2 to }#2
        /// </summary>
        Effect_SubRange_135 = 135,
        /// <summary>
        /// Caster's range increased by: #1{~1~2 to }#2
        /// </summary>
        Effect_AddRange_136 = 136,
        /// <summary>
        /// Caster's physical damage increased by : #1{~1~2 to }#2
        /// </summary>
        Effect_AddPhysicalDamage_137 = 137,
        /// <summary>
        /// Increases damage by #1{~1~2 to }#2%
        /// </summary>
        Effect_IncreaseDamage_138 = 138,
        /// <summary>
        /// Restores #1{~1~2 to }#2 energy points
        /// </summary>
        Effect_RestoreEnergyPoints = 139,
        /// <summary>
        /// Makes you skip a turn
        /// </summary>
        Effect_SkipTurn = 140,
        /// <summary>
        /// Kills the target
        /// </summary>
        Effect_Kill = 141,
        /// <summary>
        /// +#1{~1~2 to }#2 to physical damage
        /// </summary>
        Effect_AddPhysicalDamage_142 = 142,
        /// <summary>
        /// HP restored: #1{~1~2 to }#2
        /// </summary>
        Effect_HealHP_143 = 143,
        /// <summary>
        /// Damage: #1{~1~2 to }#2 (unboosted)
        /// </summary>
        Effect_DamageFix = 144,
        /// <summary>
        /// -#1{~1~2 to }#2 to damage
        /// </summary>
        Effect_SubDamageBonus = 145,
        /// <summary>
        /// Changes the words
        /// </summary>
        Effect_ChangesWords = 146,
        /// <summary>
        /// Revive an ally
        /// </summary>
        Effect_ReviveAlly = 147,
        /// <summary>
        /// Someone's following you!
        /// </summary>
        Effect_Followed = 148,
        /// <summary>
        /// Changes appearance
        /// </summary>
        Effect_ChangeAppearance = 149,
        /// <summary>
        /// Makes the character invisible
        /// </summary>
        Effect_Invisibility = 150,
        /// <summary>
        /// -#1{~1~2 to }#2 chance
        /// </summary>
        Effect_SubChance = 152,
        /// <summary>
        /// -#1{~1~2 to }#2 vitality
        /// </summary>
        Effect_SubVitality = 153,
        /// <summary>
        /// -#1{~1~2 to }#2 agility
        /// </summary>
        Effect_SubAgility = 154,
        /// <summary>
        /// -#1{~1~2 to }#2 intelligence
        /// </summary>
        Effect_SubIntelligence = 155,
        /// <summary>
        /// -#1{~1~2 to }#2 wisdom
        /// </summary>
        Effect_SubWisdom = 156,
        /// <summary>
        /// -#1{~1~2 to }#2 strength
        /// </summary>
        Effect_SubStrength = 157,
        /// <summary>
        /// Increases load weight by #1{~1~2 to }#2 pods
        /// </summary>
        Effect_IncreaseWeight = 158,
        /// <summary>
        /// Decreases load weight by #1{~1~2 to }#2 pods
        /// </summary>
        Effect_DecreaseWeight = 159,
        /// <summary>
        /// Increases chance of avoiding AP loss by #1{~1~2 to }#2%
        /// </summary>
        Effect_AddDodgeAPProbability = 160,
        /// <summary>
        /// Increases chance of avoiding MP loss by #1{~1~2 to }#2%
        /// </summary>
        Effect_AddDodgeMPProbability = 161,
        /// <summary>
        /// -#1{~1~2 to}#2 chance of avoiding AP losses
        /// </summary>
        Effect_SubDodgeAPProbability = 162,
        /// <summary>
        /// -#1{~1~2 to}#2 chance of avoiding MP losses
        /// </summary>
        Effect_SubDodgeMPProbability = 163,
        /// <summary>
        /// Damage reduced by #1%
        /// </summary>
        Effect_AddGlobalDamageReduction = 164,
        /// <summary>
        /// Increases (#1) damage by #2%
        /// </summary>
        Effect_AddDamageBonusPercent = 165,
        /// <summary>
        /// AP given back: #1{~1~2 to }#2
        /// </summary>
        Effect_166 = 166,
        /// <summary>
        /// -#1{~1~2 to }#2 AP
        /// </summary>
        Effect_SubAP = 168,
        /// <summary>
        /// -#1{~1~2 to }#2 MP
        /// </summary>
        Effect_SubMP = 169,
        /// <summary>
        /// -#1{~1~2 to }#2 critical hits
        /// </summary>
        Effect_SubCriticalHit = 171,
        /// <summary>
        /// Magic reduction decreased by #1{~1~2 to }#2
        /// </summary>
        Effect_SubMagicDamageReduction = 172,
        /// <summary>
        /// Physical reduction decreased by #1{~1~2 to }#2
        /// </summary>
        Effect_SubPhysicalDamageReduction = 173,
        /// <summary>
        /// +#1{~1~2 to }#2 initiative
        /// </summary>
        Effect_AddInitiative = 174,
        /// <summary>
        /// -#1{~1~2 to }#2 initiative
        /// </summary>
        Effect_SubInitiative = 175,
        /// <summary>
        /// +#1{~1~2 to }#2 prospecting
        /// </summary>
        Effect_AddProspecting = 176,
        /// <summary>
        /// -#1{~1~2 to }#2 prospecting
        /// </summary>
        Effect_SubProspecting = 177,
        /// <summary>
        /// +#1{~1~2 to }#2 heals
        /// </summary>
        Effect_AddHealBonus = 178,
        /// <summary>
        /// -#1{~1~2 to }#2 heals
        /// </summary>
        Effect_SubHealBonus = 179,
        /// <summary>
        /// Creates a double of the caster
        /// </summary>
        Effect_Double = 180,
        /// <summary>
        /// Summons: #1
        /// </summary>
        Effect_Summon = 181,
        /// <summary>
        /// +#1{~1~2 to }#2 to summonable creatures
        /// </summary>
        Effect_AddSummonLimit = 182,
        /// <summary>
        /// Magic reduction of #1{~1~2 to }#2
        /// </summary>
        Effect_AddMagicDamageReduction = 183,
        /// <summary>
        /// Physical reduction of #1{~1~2 to }#2
        /// </summary>
        Effect_AddPhysicalDamageReduction = 184,
        /// <summary>
        /// Summons a static creature
        /// </summary>
        Effect_185 = 185,
        /// <summary>
        /// Decreases damage by #1{~1~2 to }#2%
        /// </summary>
        Effect_SubDamageBonusPercent = 186,
        /// <summary>
        /// Switches alignment
        /// </summary>
        Effect_188 = 188,
        /// <summary>
        /// Gain #1{~1~2 to }#2 Kamas
        /// </summary>
        Effect_GiveKamas = 194,
        /// <summary>
        /// Transform into #1
        /// </summary>
        Effect_197 = 197,
        /// <summary>
        /// Put an item on the ground
        /// </summary>
        Effect_201 = 201,
        /// <summary>
        /// Reveals all invisible items
        /// </summary>
        Effect_RevealsInvisible = 202,
        /// <summary>
        /// Revive the target
        /// </summary>  
        Effect_206 = 206,
        /// <summary>
        /// #1{~1~2 to }#2 % earth resistance
        /// </summary>
        Effect_AddEarthResistPercent = 210,
        /// <summary>
        /// #1{~1~2 to }#2 % water resistance
        /// </summary>
        Effect_AddWaterResistPercent = 211,
        /// <summary>
        /// #1{~1~2 to }#2 % air resistance
        /// </summary>
        Effect_AddAirResistPercent = 212,
        /// <summary>
        /// #1{~1~2 to }#2 % fire resistance
        /// </summary>
        Effect_AddFireResistPercent = 213,
        /// <summary>
        /// #1{~1~2 to }#2 % neutral resistance
        /// </summary>
        Effect_AddNeutralResistPercent = 214,
        /// <summary>
        /// #1{~1~2 to }#2 % earth weakness
        /// </summary>
        Effect_SubEarthResistPercent = 215,
        /// <summary>
        /// #1{~1~2 to }#2 % water weakness
        /// </summary>
        Effect_SubWaterResistPercent = 216,
        /// <summary>
        /// #1{~1~2 to }#2 % air weakness
        /// </summary>
        Effect_SubAirResistPercent = 217,
        /// <summary>
        /// #1{~1~2 to }#2 % fire weakness
        /// </summary>
        Effect_SubFireResistPercent = 218,
        /// <summary>
        /// #1{~1~2 to }#2 % neutral weakness
        /// </summary>
        Effect_SubNeutralResistPercent = 219,
        /// <summary>
        /// Reflects #1 damage
        /// </summary>
        Effect_220 = 220,
        /// <summary>
        /// What's in there?
        /// </summary>
        Effect_221 = 221,
        /// <summary>
        /// What's in there?
        /// </summary>
        Effect_222 = 222,
        /// <summary>
        /// Adds #1{~1~2 to }#2 to trap damage
        /// </summary>
        Effect_AddTrapBonus = 225,
        /// <summary>
        /// +#1{~1~2 to }#2% damage to traps
        /// </summary>
        Effect_AddTrapBonusPercent = 226,
        /// <summary>
        /// Get a mount!
        /// </summary>
        Effect_229 = 229,
        /// <summary>
        /// +#1 of lost energy
        /// </summary>
        Effect_230 = 230,
        Effect_239 = 239,
        /*  */
        /// <summary>
        /// +#1{~1~2 to }#2 earth resistance
        /// </summary>
        Effect_AddEarthElementReduction = 240,
        /// <summary>
        /// +#1{~1~2 to }#2 water resistance
        /// </summary>
        Effect_AddWaterElementReduction = 241,
        /// <summary>
        /// +#1{~1~2 to }#2 air resistance
        /// </summary>
        Effect_AddAirElementReduction = 242,
        /// <summary>
        /// +#1{~1~2 to }#2 fire resistance
        /// </summary>
        Effect_AddFireElementReduction = 243,
        /// <summary>
        /// +#1{~1~2 to }#2 neutral resistance
        /// </summary>
        Effect_AddNeutralElementReduction = 244,
        /// <summary>
        /// -#1{~1~2 to }#2 earth resistance
        /// </summary>
        Effect_SubEarthElementReduction = 245,
        /// <summary>
        /// -#1{~1~2 to }#2 water resistance
        /// </summary>
        Effect_SubWaterElementReduction = 246,
        /// <summary>
        /// -#1{~1~2 to }#2 air resistance
        /// </summary>
        Effect_SubAirElementReduction = 247,
        /// <summary>
        /// -#1{~1~2 to }#2 fire resistance
        /// </summary>
        Effect_SubFireElementReduction = 248,
        /// <summary>
        /// -#1{~1~2 to }#2 neutral resistance
        /// </summary>
        Effect_SubNeutralElementReduction = 249,
        /// <summary>
        /// #1{~1~2 to }#2% earth resistance against fighters
        /// </summary>
        Effect_AddPvpEarthResistPercent = 250,
        /// <summary>
        /// #1{~1~2 to }#2 % water resistance against fighters
        /// </summary>
        Effect_AddPvpWaterResistPercent = 251,
        /// <summary>
        /// #1{~1~2 to }#2 % air resistance against fighters
        /// </summary>
        Effect_AddPvpAirResistPercent = 252,
        /// <summary>
        /// #1{~1~2 to }#2 % fire resistance against fighters
        /// </summary>
        Effect_AddPvpFireResistPercent = 253,
        /// <summary>
        /// #1{~1~2 to }#2 % neutral resistance against fighters
        /// </summary>
        Effect_AddPvpNeutralResistPercent = 254,
        /// <summary>
        /// #1{~1~2 to }#2 % earth weakness against fighters
        /// </summary>
        Effect_SubPvpEarthResistPercent = 255,
        /// <summary>
        /// #1{~1~2 to }#2 % water weakness against fighters
        /// </summary>
        Effect_SubPvpWaterResistPercent = 256,
        /// <summary>
        /// #1{~1~2 to }#2 % air weakness against fighters
        /// </summary>
        Effect_SubPvpAirResistPercent = 257,
        /// <summary>
        /// #1{~1~2 to }#2 % fire weakness against fighters
        /// </summary>
        Effect_SubPvpFireResistPercent = 258,
        /// <summary>
        /// #1{~1~2 to }#2 % neutral weakness against fighters
        /// </summary>
        Effect_SubPvpNeutralResistPercent = 259,
        /// <summary>
        /// +#1{~1~2 to }#2 earth resistance against fighters
        /// </summary>
        Effect_AddPvpEarthElementReduction = 260,
        /// <summary>
        /// +#1{~1~2 to }#2 water resistance against fighters
        /// </summary>
        Effect_AddPvpWaterElementReduction = 261,
        /// <summary>
        /// Adds #1{~1~2 to }#2 air resistance against fighters
        /// </summary>
        Effect_AddPvpAirElementReduction = 262,
        /// <summary>
        /// +#1{~1~2 to }#2 fire resistance against fighters
        /// </summary>
        Effect_AddPvpFireElementReduction = 263,
        /// <summary>
        /// +#1{~1~2 to }#2 neutral resistance against fighters
        /// </summary>
        Effect_AddPvpNeutralElementReduction = 264,
        /// <summary>
        /// Damage reduced by #1{~1~2 to }#2
        /// </summary>
        Effect_AddArmorDamageReduction = 265,
        /// <summary>
        /// #1{~1~2 to }#2 Chance theft
        /// </summary>
        Effect_StealChance = 266,
        /// <summary>
        /// #1{~1~2 to }#2 Vitality theft
        /// </summary>
        Effect_StealVitality = 267,
        /// <summary>
        /// #1{~1~2 to }#2 Agility theft
        /// </summary>
        Effect_StealAgility = 268,
        /// <summary>
        /// #1{~1~2 to }#2 Intelligence theft
        /// </summary>
        Effect_StealIntelligence = 269,
        /// <summary>
        /// #1{~1~2 to }#2 Wisdom theft
        /// </summary>
        Effect_StealWisdom = 270,
        /// <summary>
        /// #1{~1~2 to }#2 Strength theft
        /// </summary>
        Effect_StealStrength = 271,
        /// <summary>
        /// Damage: #1{~1~2 to }#2% of the attacker's lost HP (water)
        /// </summary>
        Effect_275 = 275,
        /// <summary>
        /// Damage: #1{~1~2 to }#2% of the attacker's lost HP (earth)
        /// </summary>
        Effect_276 = 276,
        /// <summary>
        /// Damage: #1{~1~2 to }#2% of the attacker's lost HP (air)
        /// </summary>
        Effect_277 = 277,
        /// <summary>
        /// Damage: #1{~1~2 to }#2% of the attacker's lost HP (fire)
        /// </summary>
        Effect_278 = 278,
        /// <summary>
        /// Damage: #1{~1~2 to }#2% of the attacker's lost HP (neutral)
        /// </summary>
        Effect_279 = 279,
        /// <summary>
        /// Increases #1's range by #3
        /// </summary>
        Effect_281 = 281,
        /// <summary>
        /// Makes it possible to modify #1's range
        /// </summary>
        Effect_282 = 282,
        /// <summary>
        /// Adds #3 to #1's damage
        /// </summary>
        Effect_283 = 283,
        /// <summary>
        /// Adds #3 to #1's heals
        /// </summary>
        Effect_284 = 284,
        /// <summary>
        /// Reduces #1's AP cost by #3
        /// </summary>
        Effect_285 = 285,
        /// <summary>
        /// Reduces #1's cooldown period by #3
        /// </summary>
        Effect_286 = 286,
        /// <summary>
        /// Adds #3 to #1's Critical Hits
        /// </summary>
        Effect_287 = 287,
        /// <summary>
        /// #1 no longer has to be cast in a straight line
        /// </summary>
        Effect_288 = 288,
        /// <summary>
        /// #1 no longer needs line of sight
        /// </summary>
        Effect_289 = 289,
        /// <summary>
        /// Increases the maximum number of times #1 can be cast per turn by #3
        /// </summary>
        Effect_290 = 290,
        /// <summary>
        /// Increases the maximum number of times #1 can be cast per target by #3
        /// </summary>
        Effect_291 = 291,
        /// <summary>
        /// #1's cooldown period is set to #3
        /// </summary>
        Effect_292 = 292,
        /// <summary>
        /// Increases #1's basic damage by #3
        /// </summary>
        Effect_SpellBoost = 293,
        /// <summary>
        /// Reduces #1's range by #3
        /// </summary>
        Effect_294 = 294,
        Effect_310 = 310,
        /*  */
        /// <summary>
        /// Steals #1{~1~2 to }#2 range
        /// </summary>
        Effect_StealRange = 320,
        /// <summary>
        /// Change a colour
        /// </summary>
        Effect_333 = 333,
        /// <summary>
        /// Change appearance
        /// </summary>
        Effect_ChangeAppearance_335 = 335,
        /// <summary>
        /// Sets a grade #2 trap
        /// </summary>
        Effect_Trap = 400,
        /// <summary>
        /// Sets a grade #2 glyph
        /// </summary>
        Effect_Glyph = 401,
        /// <summary>
        /// Sets a grade #2 glyph
        /// </summary>
        Effect_Glyph_402 = 402,
        /// <summary>
        /// Kills and replaces with a summon
        /// </summary>
        Effect_405 = 405,
        /// <summary>
        /// Removes the effects of %1
        /// </summary>
        Effect_RemoveSpellEffects = 406,
        /// <summary>
        /// HP restored: #1{~1~2 to }#2
        /// </summary>
        Effect_407 = 407,
        /// <summary>
        /// +#1{~1~2 to }#2 AP attack
        /// </summary>
        Effect_AddAPAttack = 410,
        /// <summary>
        /// -#1{~1~2 to }#2 AP attack
        /// </summary>
        Effect_SubAPAttack = 411,
        /// <summary>
        /// +#1{~1~2 to }#2 MP attack
        /// </summary>
        Effect_AddMPAttack = 412,
        /// <summary>
        /// -#1{~1~2 to }#2 MP attack
        /// </summary>
        Effect_SubMPAttack = 413,
        /// <summary>
        /// +#1{~1~2 to }#2 pushback damage
        /// </summary>
        Effect_AddPushDamageBonus = 414,
        /// <summary>
        /// -#1{~1~2 to }#2 pushback damage
        /// </summary>
        Effect_SubPushDamageBonus = 415,
        /// <summary>
        /// +#1{~1~2 to }#2 pushback resistance
        /// </summary>
        Effect_AddPushDamageReduction = 416,
        /// <summary>
        /// -#1{~1~2 to }#2 pushback resistance
        /// </summary>
        Effect_SubPushDamageReduction = 417,
        /// <summary>
        /// +#1{~1~2 to }#2 critical damage
        /// </summary>
        Effect_AddCriticalDamageBonus = 418,
        /// <summary>
        /// -#1{~1~2 to }#2 critical damage
        /// </summary>
        Effect_SubCriticalDamageBonus = 419,
        /// <summary>
        /// +#1{~1~2 to }#2 critical resistance
        /// </summary>
        Effect_AddCriticalDamageReduction = 420,
        /// <summary>
        /// -#1{~1~2 to }#2 critical resistance
        /// </summary>
        Effect_SubCriticalDamageReduction = 421,
        /// <summary>
        /// +#1{~1~2 to }#2 Earth damage
        /// </summary>
        Effect_AddEarthDamageBonus = 422,
        /// <summary>
        /// -#1{~1~2 to }#2 Earth damage
        /// </summary>
        Effect_SubEarthDamageBonus = 423,
        /// <summary>
        /// +#1{~1~2 to }#2 Fire damage
        /// </summary>
        Effect_AddFireDamageBonus = 424,
        /// <summary>
        /// -#1{~1~2 to }#2 Fire damage
        /// </summary>
        Effect_SubFireDamageBonus = 425,
        /// <summary>
        /// +#1{~1~2 to }#2 Water damage
        /// </summary>
        Effect_AddWaterDamageBonus = 426,
        /// <summary>
        /// -#1{~1~2 to }#2 Water damage
        /// </summary>
        Effect_SubWaterDamageBonus = 427,
        /// <summary>
        /// +#1{~1~2 to }#2 Air damage
        /// </summary>
        Effect_AddAirDamageBonus = 428,
        /// <summary>
        /// -#1{~1~2 to }#2 Air damage
        /// </summary>
        Effect_SubAirDamageBonus = 429,
        /// <summary>
        /// +#1{~1~2 to }#2 Neutral damage
        /// </summary>
        Effect_AddNeutralDamageBonus = 430,
        /// <summary>
        /// -#1{~1~2 to }#2 Neutral damage
        /// </summary>
        Effect_SubNeutralDamageBonus = 431,
        /// <summary>
        /// Steals #1{~1~2 to }#2 AP
        /// </summary>
        Effect_StealAP_440 = 440,
        /// <summary>
        /// Steals #1{~1~2 to }#2 MP
        /// </summary>
        Effect_StealMP_441 = 441,
        /// <summary>
        /// Place a prism
        /// </summary>
        Effect_513 = 513,
        /// <summary>
        /// Teleport to save point
        /// </summary>
        Effect_600 = 600,
        Effect_601 = 601,
        /*  */
        /// <summary>
        /// Save your position
        /// </summary>
        Effect_602 = 602,
        /// <summary>
        /// Learn the #3 profession
        /// </summary>
        Effect_603 = 603,
        /// <summary>
        /// Learn the spell #3
        /// </summary>
        Effect_LearnSpell = 604,
        /// <summary>
        /// +#1{~1~2 to }#2 XP points
        /// </summary>
        Effect_605 = 605,
        /// <summary>
        /// +#1{~1~2 to }#2 wisdom
        /// </summary>
        Effect_AddPermanentWisdom = 606,
        /// <summary>
        /// +#1{~1~2 to }#2 strength
        /// </summary>
        Effect_AddPermanentStrength = 607,
        /// <summary>
        /// +#1{~1~2 to }#2 chance
        /// </summary>
        Effect_AddPermanentChance = 608,
        /// <summary>
        /// +#1{~1~2 to }#2 agility
        /// </summary>
        Effect_AddPermanentAgility = 609,
        /// <summary>
        /// +#1{~1~2 to }#2 vitality
        /// </summary>
        Effect_AddPermanentVitality = 610,
        /// <summary>
        /// +#1{~1~2 to }#2 intelligence
        /// </summary>
        Effect_AddPermanentIntelligence = 611,
        /// <summary>
        /// +#1{~1~2 to }#2 characteristic points
        /// </summary>
        Effect_612 = 612,
        /// <summary>
        /// +#1{~1~2 to }#2 spell points
        /// </summary>
        Effect_AddSpellPoints = 613,
        /// <summary>
        /// +#1 XP for profession #2
        /// </summary>
        Effect_614 = 614,
        /// <summary>
        /// Makes you forget the profession #3
        /// </summary>
        Effect_615 = 615,
        /// <summary>
        /// Makes you forget one level of the spell #3
        /// </summary>
        Effect_616 = 616,
        /// <summary>
        /// Consult #3
        /// </summary>
        Effect_620 = 620,
        /// <summary>
        /// Summon: #3 (level #1)
        /// </summary>
        Effect_621 = 621,
        /// <summary>
        /// Teleport to your house
        /// </summary>
        Effect_622 = 622,
        /// <summary>
        /// Summons: #3
        /// </summary>
        Effect_623 = 623,
        /// <summary>
        /// Makes you forget one level of the spell #3
        /// </summary>
        Effect_624 = 624,
        Effect_625 = 625,
        /*  */
        Effect_626 = 626,
        /*  */
        /// <summary>
        /// Recreates original map
        /// </summary>
        Effect_627 = 627,
        /// <summary>
        /// Summons: #3
        /// </summary>
        Effect_628 = 628,
        Effect_631 = 631,
        /*  */
        /// <summary>
        /// Adds #3 Honour points
        /// </summary>
        Effect_640 = 640,
        /// <summary>
        /// Adds #3 Disgrace points
        /// </summary>
        Effect_641 = 641,
        /// <summary>
        /// Withdraws #3 Honour points
        /// </summary>
        Effect_642 = 642,
        /// <summary>
        /// Withdraws #3 Disgrace points
        /// </summary>
        Effect_643 = 643,
        /// <summary>
        /// Resuscitates allies on your map
        /// </summary>
        Effect_645 = 645,
        /// <summary>
        /// Restored HP: #1{~1~2 to }#2
        /// </summary>
        Effect_646 = 646,
        /// <summary>
        /// Frees enemy souls
        /// </summary>
        Effect_647 = 647,
        /// <summary>
        /// Frees an enemy soul
        /// </summary>
        Effect_648 = 648,
        /// <summary>
        /// Pretend to be #3
        /// </summary>
        Effect_649 = 649,
        Effect_654 = 654,
        /*  */
        /// <summary>
        /// No additional effects
        /// </summary>
        Effect_666 = 666,
        /// <summary>
        /// Incarnation level #3
        /// </summary>
        Effect_669 = 669,
        /// <summary>
        /// Damage: #1{~1~2 to }#2% of the attacker's HP (Neutral-type)
        /// </summary>
        Effect_670 = 670,
        /// <summary>
        /// Damage: #1{~1~2 to }#2% of the attacker's HP (Neutral-type)
        /// </summary>
        Effect_671 = 671,
        /// <summary>
        /// Damage: #1{~1~2 to }#2% of the attacker's life (neutral)
        /// </summary>
        Effect_Punishment_Damage = 672,
        /// <summary>
        /// Link a job: #1
        /// </summary>
        Effect_699 = 699,
        /// <summary>
        /// Change the attack element
        /// </summary>
        Effect_700 = 700,
        /// <summary>
        /// Power: #1{~1~2 to }#2
        /// </summary>
        Effect_701 = 701,
        /// <summary>
        /// +#1{~1~2 to }#2 durability points
        /// </summary>
        Effect_702 = 702,
        /// <summary>
        /// #1% chance of capturing a power #3 soul
        /// </summary>
        Effect_705 = 705,
        /// <summary>
        /// #1% chance of capturing a mount
        /// </summary>
        Effect_706 = 706,
        /// <summary>
        /// Use custom set nÂ°#3
        /// </summary>
        Effect_707 = 707,
        /// <summary>
        /// Additional cost
        /// </summary>
        Effect_710 = 710,
        /// <summary>
        /// #1 : #3
        /// </summary>
        Effect_715 = 715,
        /// <summary>
        /// #1 : #3
        /// </summary>
        Effect_716 = 716,
        /// <summary>
        /// #1 : #3
        /// </summary>
        Effect_717 = 717,
        /// <summary>
        /// Number of victims: #2
        /// </summary>
        Effect_720 = 720,
        /// <summary>
        /// Unlock the title #3
        /// </summary>
        Effect_AddTitle = 724,
        /// <summary>
        /// Unlock the ornament #3
        /// </summary>
        Effect_AddOrnament = 726,
        /// <summary>
        /// Rename guild: #4
        /// </summary>
        Effect_725 = 725,
        /// <summary>
        /// Teleport to the nearest allied prism
        /// </summary>
        Effect_730 = 730,
        /// <summary>
        /// Attack players of the opposite alignment automatically
        /// </summary>
        Effect_731 = 731,
        /// <summary>
        /// Resistance to automatic attacks from enemy players: #1{~1~2 to }#2
        /// </summary>
        Effect_732 = 732,
        Effect_740 = 740,
        /*  */
        Effect_741 = 741,
        /*  */
        Effect_742 = 742,
        /*  */
        /// <summary>
        /// Increases chance of capture by: #1{~1~2 to }#2%
        /// </summary>
        Effect_750 = 750,
        /// <summary>
        /// Bonus to Dragoturkey XP: #1{~1~2 to }#2%
        /// </summary>
        Effect_751 = 751,
        /// <summary>
        /// Dodge bonus: #1{~1~2 to }#2
        /// </summary>
        Effect_AddDodge = 752,
        /// <summary>
        /// Lock bonus: #1{~1~2 to }#2
        /// </summary>
        Effect_AddLock = 753,
        /// <summary>
        /// Dodge penalty: #1{~1~2 to }#2
        /// </summary>
        Effect_SubDodge = 754,
        /// <summary>
        /// Lock penalty: #1{~1~2 to }#2
        /// </summary>
        Effect_SubLock = 755,
        /// <summary>
        /// Disappear by moving
        /// </summary>
        Effect_760 = 760,
        /// <summary>
        /// Intercept damages
        /// </summary>
        Effect_DamageIntercept = 765,
        /// <summary>
        /// Clockwise confusion: #1{~1~2 to }#2 degrees
        /// </summary>
        Effect_770 = 770,
        /// <summary>
        /// Clockwise confusion: #1{~1~2 to }#2 Pi/2
        /// </summary>
        Effect_771 = 771,
        /// <summary>
        /// Clockwise confusion: #1{~1~2 to }#2 Pi/4
        /// </summary>
        Effect_772 = 772,
        /// <summary>
        /// Anticlockwise confusion: #1{~1~2 to }#2 degrees
        /// </summary>
        Effect_773 = 773,
        /// <summary>
        /// Anticlockwise confusion: #1{~1~2 to }#2 Pi/2
        /// </summary>
        Effect_774 = 774,
        /// <summary>
        /// Anticlockwise confusion: #1{~1~2 to }#2 Pi/4
        /// </summary>
        Effect_775 = 775,
        /// <summary>
        /// Increases permanent damage taken by #1{~1~2 to }#2%
        /// </summary>
        Effect_AddErosion = 776,
        /// <summary>
        /// Summons the last ally who died with #1{~1~2 to }#2 % of their HP
        /// </summary>
        Effect_ReviveAndGiveHPToLastDiedAlly = 780,
        /// <summary>
        /// Minimizes random effects
        /// </summary>
        Effect_RandDownModifier = 781,
        /// <summary>
        /// Maximizes random effects
        /// </summary>
        Effect_RandUpModifier = 782,
        /// <summary>
        /// Repels to the targeted cell
        /// </summary>
        Effect_RepelsTo = 783,
        /// <summary>
        /// Return to original position
        /// </summary>
        Effect_Rollback = 784,
        Effect_785 = 785,
        /*  */
        /// <summary>
        /// Heals upon attack
        /// </summary>
        Effect_786 = 786,
        /// <summary>
        /// #1
        /// </summary>
        Effect_787 = 787,
        /// <summary>
        /// Punishment of #2 for #3 turn(s)
        /// </summary>
        Effect_Punishment = 788,
        Effect_789 = 789,
        /*  */
        Effect_790 = 790,
        /*  */
        /// <summary>
        /// Prepare #1{~1~2 to }#2 mercenary scrolls
        /// </summary>
        Effect_791 = 791,
        /// <summary>
        /// #1
        /// </summary>
        Effect_Frikt = 792,
        /// <summary>
        /// #1
        /// </summary>
        Effect_Putsch = 793,
        /// <summary>
        /// Hunting Weapon
        /// </summary>
        Effect_795 = 795,
        /// <summary>
        /// Health points: #3
        /// </summary>
        Effect_800 = 800,
        /// <summary>
        /// Received on: #1
        /// </summary>
        Effect_805 = 805,
        /// <summary>
        /// State: #1
        /// </summary>
        Effect_806 = 806,
        /// <summary>
        /// Last meal: #1
        /// </summary>
        Effect_807 = 807,
        /// <summary>
        /// Last meal: #1
        /// </summary>
        Effect_LastMeal = 808,
        /// <summary>
        /// Size: #3 squares
        /// </summary>
        Effect_810 = 810,
        /// <summary>
        /// Remaining turn(s): #3
        /// </summary>
        Effect_RemainingFights = 811,
        /// <summary>
        /// Durability: #2 / #3
        /// </summary>
        Effect_812 = 812,
        Effect_813 = 813,
        /*  */
        /// <summary>
        /// #1
        /// </summary>
        Effect_814 = 814,
        Effect_815 = 815,
        /*  */
        Effect_816 = 816,
        /*  */
        /// <summary>
        /// Teleport
        /// </summary>
        Effect_825 = 825,
        /// <summary>
        /// Start a fight against #2
        /// </summary>
        Effect_905 = 905,
        /// <summary>
        /// Increases serenity, decreases aggressiveness
        /// </summary>
        Effect_930 = 930,
        /// <summary>
        /// Improves aggressiveness, decreases serenity
        /// </summary>
        Effect_931 = 931,
        /// <summary>
        /// Increases stamina
        /// </summary>
        Effect_932 = 932,
        /// <summary>
        /// Decreases stamina
        /// </summary>
        Effect_933 = 933,
        /// <summary>
        /// Increases love
        /// </summary>
        Effect_934 = 934,
        /// <summary>
        /// Decreases love
        /// </summary>
        Effect_935 = 935,
        /// <summary>
        /// Speeds maturity
        /// </summary>
        Effect_936 = 936,
        /// <summary>
        /// Slows down maturity
        /// </summary>
        Effect_937 = 937,
        /// <summary>
        /// Increases the capacity of a pet #3 .
        /// </summary>
        Effect_939 = 939,
        /// <summary>
        /// Improved abilities
        /// </summary>
        Effect_940 = 940,
        /// <summary>
        /// Temporarily remove a Breeding item
        /// </summary>
        Effect_946 = 946,
        /// <summary>
        /// Remove an item from a Paddock
        /// </summary>
        Effect_947 = 947,
        /// <summary>
        /// Paddock Item
        /// </summary>
        Effect_948 = 948,
        /// <summary>
        /// Get on/off a mount
        /// </summary>
        Effect_949 = 949,
        /// <summary>
        /// #3 state
        /// </summary>
        Effect_AddState = 950,
        /// <summary>
        /// Removes #3 state
        /// </summary>
        Effect_DispelState = 951,
        /// <summary>
        /// #3 state deactivated
        /// </summary>
        Effect_DisableState = 952,
        /// <summary>
        /// Alignment: #3
        /// </summary>
        Effect_960 = 960,
        /// <summary>
        /// Rank: #3
        /// </summary>
        Effect_961 = 961,
        /// <summary>
        /// Level: #3
        /// </summary>
        Effect_962 = 962,
        /// <summary>
        /// Created #3 day(s) ago
        /// </summary>
        Effect_963 = 963,
        /// <summary>
        /// Name: #4
        /// </summary>
        Effect_964 = 964,
        Effect_LivingObjectId = 970,
        /*  */
        Effect_LivingObjectMood = 971,
        /*  */
        Effect_LivingObjectSkin = 972,
        /*  */
        Effect_LivingObjectCategory = 973,
        /*  */
        Effect_LivingObjectLevel = 974,
        /*  */
        /// <summary>
        /// Non-exchangeable
        /// </summary>
        Effect_NonExchangeable_981 = 981,
        /// <summary>
        /// Non-exchangeable
        /// </summary>
        Effect_NonExchangeable_982 = 982,
        /// <summary>
        /// Can be exchanged from: #1
        /// </summary>
        Effect_983 = 983,
        Effect_984 = 984,
        /*  */
        /// <summary>
        /// Modified by: #4
        /// </summary>
        Effect_985 = 985,
        /// <summary>
        /// Prepares #1{~1~2 to }#2 scrolls
        /// </summary>
        Effect_986 = 986,
        /// <summary>
        /// Belongs to: #4
        /// </summary>
        Effect_BelongsTo = 987,
        /// <summary>
        /// Made by: #4
        /// </summary>
        Effect_988 = 988,
        /// <summary>
        /// Seeks: #4
        /// </summary>
        Effect_989 = 989,
        /// <summary>
        /// #4
        /// </summary>
        Effect_990 = 990,
        /// <summary>
        /// !! Invalid Certificate !!
        /// </summary>
        Effect_InvalidCertificate = 994,
        /// <summary>
        /// View mount characteristics
        /// </summary>
        Effect_ViewMountCharacteristics = 995,
        /// <summary>
        /// Belongs to: #4
        /// </summary>
        Effect_996 = 996,
        /// <summary>
        /// Name: #4
        /// </summary>
        Effect_Name = 997,
        /// <summary>
        /// Validity: #1d #2h #3m
        /// </summary>
        Effect_Validity = 998,
        Effect_999 = 999,
        /// <summary>
        /// 2
        /// </summary>
        Effect_1002 = 1002,
        /// <summary>
        /// Reduces the maximum bonus by #1{~1~2 to }#2
        /// </summary>
        Effect_1003 = 1003,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect_1004 = 1004,
        /// <summary>
        /// Reduces the maximum bonus by #1{~1~2 to }#2
        /// </summary>
        Effect_1005 = 1005,
        /// <summary>
        /// Reduces the minimum bonus by #1{~1~2 to }#2
        /// </summary>
        Effect_1006 = 1006,
        /// <summary>
        /// Effectiveness: #1{~1~2 to }#2
        /// </summary>
        Effect_1007 = 1007,
        /// <summary>
        /// Summons #1
        /// </summary>
        Effect_SummonsBomb = 1008,
        /// <summary>
        /// Activate a bomb
        /// </summary>
        Effect_TriggerBomb = 1009,
        /// <summary>
        /// Places a grade #2 Glyph
        /// </summary>
        Effect_1010 = 1010,
        /// <summary>
        /// Summons #1
        /// </summary>
        Effect_1011 = 1011,
        /// <summary>
        /// #1{~1~2 to }#2 (Neutral damage)
        /// </summary>
        Effect_DamageNeutralRemainingMP = 1012,
        /// <summary>
        /// #1{~1~2 to }#2 (Air damage)
        /// </summary>
        Effect_DamageAirRemainingMP = 1013,
        /// <summary>
        /// #1{~1~2 to }#2 (Water damage)
        /// </summary>
        Effect_DamageWaterRemainingMP = 1014,
        /// <summary>
        /// #1{~1~2 to }#2 (Fire damage)
        /// </summary>
        Effect_DamageFireRemainingMP = 1015,
        /// <summary>
        /// #1{~1~2 to }#2 (Earth damage)
        /// </summary>
        Effect_DamageEarthRemainingMP = 1016,
        /// <summary>
        /// #1
        /// </summary>
        Effect_1017 = 1017,
        /// <summary>
        /// #1
        /// </summary>
        Effect_1018 = 1018,
        /// <summary>
        /// #1
        /// </summary>
        Effect_1019 = 1019,
        /// <summary>
        /// Pushes back #1 cell(s)
        /// </summary>
        Effect_1021 = 1021,
        /// <summary>
        /// Attracts #1 cell(s)
        /// </summary>
        Effect_1022 = 1022,
        /// <summary>
        /// Switches positions
        /// </summary>
        Effect_1023 = 1023,
        /// <summary>
        /// Creates illusions
        /// </summary>
        Effect_1024 = 1024,
        /// <summary>
        /// Trigger traps
        /// </summary>
        Effect_1025 = 1025,
        /// <summary>
        /// Trigger glyphs
        /// </summary>
        Effect_1026 = 1026,
        /// <summary>
        /// #1{~1~2 to }#2% Combo Damage
        /// </summary>
        Effect_AddComboBonus = 1027,
        /// <summary>
        /// Trigger powders
        /// </summary>
        Effect_1028 = 1028,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect_1029 = 1029,
        /// <summary>
        /// Place grade #2 powder
        /// </summary>
        Effect_1030 = 1030,
        /// <summary>
        /// Ends turn
        /// </summary>
        Effect_SkipTurn_1031 = 1031,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect_1032 = 1032,
        /// <summary>
        /// -#1{~1~2 to -}#2% Vitality
        /// </summary>
        Effect_SubVitalityPercent = 1033,
        /// <summary>
        /// Summons the last ally killed with #1{~1~2 to }#2% of their HP
        /// </summary>
        Effect_1034 = 1034,
        /// <summary>
        /// #1: +#3 turns until recast
        /// </summary>
        Effect_1035 = 1035,
        /// <summary>
        /// #1: -#3 turns until recast
        /// </summary>
        Effect_1036 = 1036,
        /// <summary>
        /// HP restored: #1 {~1~2 to}#2
        /// </summary>
        Effect_1037 = 1037,
        /// <summary>
        /// Aura: #1
        /// </summary>
        Effect_1038 = 1038,
        /// <summary>
        /// #1{~1~2 to }#2% of HP to shield
        /// </summary>
        Effect_1039 = 1039,
        /// <summary>
        /// #1{~1~2 to }#2 Shield
        /// </summary>
        Effect_1040 = 1040,
        /// <summary>
        /// Retreats #1 cell(s)
        /// </summary>
        Effect_1041 = 1041,
        /// <summary>
        /// Advances #1 cell(s)
        /// </summary>
        Effect_Advance = 1042,
        /// <summary>
        /// Attract to the selected cell
        /// </summary>
        Effect_1043 = 1043,
        /// <summary>
        /// Immunity: #1
        /// </summary>
        Effect_1044 = 1044,
        /// <summary>
        /// #1: #3 turns until recast
        /// </summary>
        Effect_1045 = 1045,
        /// <summary>
        /// Using #1 MP will cause a loss of #2 HP
        /// </summary>
        Effect_1046 = 1046,
        /// <summary>
        /// -#1{~1~2 to }#2 HP
        /// </summary>
        Effect_1047 = 1047,
        /// <summary>
        /// -#1{~1~2 to }#2% HP
        /// </summary>
        Effect_1048 = 1048,
        /// <summary>
        /// +#1{~1~2 to}level #2
        /// </summary>
        Effect_1049 = 1049,
        /// <summary>
        /// + #1 level in the #2 profession
        /// </summary>
        Effect_1050 = 1050,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect_1051 = 1051,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect_1052 = 1052,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect_1053 = 1053,
        /// <summary>
        /// #1{~1~2 to }#2 Power (spells)
        /// </summary>
        Effect_IncreaseDamage_1054 = 1054,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect_1055 = 1055,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect_1057 = 1057,
        /// <summary>765
        /// (not found)
        /// </summary>
        Effect_1058 = 1058,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect_1059 = 1059,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect_1060 = 1060,
        /// <summary>
        /// Damage sharing
        /// </summary>
        Effect_DamageSharing = 1061,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect_1062 = 1062,
        /// <summary>
        /// #1{~1~2 to }#2 (fixed Earth damage)
        /// </summary>
        Effect_1063 = 1063,
        /// <summary>
        /// #1{~1~2 to }#2 (fixed Air damage)
        /// </summary>
        Effect_1064 = 1064,
        /// <summary>
        /// #1{~1~2 to }#2 (fixed Water damage)
        /// </summary>
        Effect_1065 = 1065,
        /// <summary>
        /// #1{~1~2 to }#2 (fixed Fire damage)
        /// </summary>
        Effect_1066 = 1066,
        /// <summary>
        /// #1{~1~2 to }#2% of the target's HP (Air damage)
        /// </summary>
        Effect_1067 = 1067,
        /// <summary>
        /// #1{~1~2 to }#2% of the target's HP (Water damage)
        /// </summary>
        Effect_1068 = 1068,
        /// <summary>
        /// #1{~1~2 to }#2% of the target's HP (Fire damage)
        /// </summary>
        Effect_1069 = 1069,
        /// <summary>
        /// #1{~1~2 to }#2% of the target's HP (Earth damage)
        /// </summary>
        Effect_1070 = 1070,
        /// <summary>
        /// #1{~1~2 to }#2% of the target's HP (Neutral damage)
        /// </summary>
        Effect_1071 = 1071,
        /// <summary>
        /// Provocation
        /// </summary>
        Effect_1072 = 1072,
        /// <summary>
        /// Change attack element
        /// </summary>
        Effect_1073 = 1073,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect_1074 = 1074,
        /// <summary>
        /// -#1{~1~2 to }#2 duration of effects
        /// </summary>
        Effect_ReduceEffectsDuration = 1075,
        /// <summary>
        /// #1{~1~2 to }#2% Resistance
        /// </summary>
        Effect_AddResistances = 1076,
        /// <summary>
        /// -#1{~1~2 to -}#2% Resistance
        /// </summary>
        Effect_SubResistances = 1077,
        /// <summary>
        /// #1{~1~2 to }#2% Vitality
        /// </summary>
        Effect_AddVitalityPercent = 1078,
        /// <summary>
        /// -#1{~1~2 to -}#2 AP
        /// See Effect_SubAP
        /// </summary>
        Effect_SubAP_1079 = 1079,
        /// <summary>
        /// -#1{~1~2 to -}#2 MP
        /// </summary>
        Effect_SubMP_1080 = 1080,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect_1081 = 1081,
        /// <summary>
        /// Wrapped by: #4
        /// </summary>
        Effect_1082 = 1082,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect_1083 = 1083,
        /// <summary>
        /// #1
        /// </summary>
        Effect_1084 = 1084,
        /// <summary>
        /// Quantity: #1
        /// </summary>
        Effect_1085 = 1085,
        /// <summary>
        /// For: #4
        /// </summary>
        Effect_1086 = 1086,
        /// <summary>
        /// Write a character's name
        /// </summary>
        Effect_1087 = 1087,
        /// <summary>
        /// Places a grade #2 Glyph
        /// </summary>
        Effect_1091 = 1091,
        /// <summary>
        /// #1{~1~2 to }#2% of the target's eroded HP inflicted as Neutral damage
        /// </summary>
        Effect_DamageNeutralPerHPEroded = 1092,
        /// <summary>
        /// #1{~1~2 to }#2% of the target's eroded HP inflicted as Air damage
        /// </summary>
        Effect_DamageAirPerHPEroded = 1093,
        /// <summary>
        /// #1{~1~2 to }#2% of the target's eroded HP inflicted as Fire damage
        /// </summary>
        Effect_DamageFirePerHPEroded = 1094,
        /// <summary>
        /// #1{~1~2 to }#2% of the target's eroded HP inflicted as Water damage
        /// </summary>
        Effect_DamageWaterPerHPEroded = 1095,
        /// <summary>
        /// #1{~1~2 to }#2% of the target's eroded HP inflicted as Earth damage
        /// </summary>
        Effect_DamageEarthPerHPEroded = 1096,
        /// <summary>
        /// Creates illusions
        /// </summary>
        Effect_Illusions = 1097,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect_1098 = 1098,
        /// <summary>
        /// Teleports the target to the cell where they started their turn
        /// </summary>
        Effect_Rewind = 1099,
        /// <summary>
        /// 
        /// </summary>
        Effect_1100 = 1100,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect_1101 = 1101,
        /// <summary>
        /// 
        /// </summary>
        Effect_1102 = 1102,
        /// <summary>
        /// Pushes back #1 cell(s)
        /// </summary>
        Effect_PushBack_1103 = 1103,
        /// <summary>
        /// 
        /// </summary>
        Effect_1104 = 1104,
        /// <summary>
        /// 
        /// </summary>
        Effect_1105 = 1105,
        /// <summary>
        /// 
        /// </summary>
        Effect_1106 = 1106,
        /// <summary>
        /// Rename the guild
        /// </summary>
        Effect_1107 = 1107,
        /// <summary>
        /// Change the guild's emblem
        /// </summary>
        Effect_1108 = 1108,
        /// <summary>
        /// #1{~1~2 to }#2% (HP restored)
        /// </summary>
        Effect_RestoreHPPercent = 1109,
        /// <summary>
        /// #3 loot
        /// </summary>
        Effect_1111 = 1111,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect_1118 = 1118,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect_1119 = 1119,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect_1120 = 1120,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect_1121 = 1121,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect_1122 = 1122,
        /// <summary>
        /// [!]
        /// </summary>
        Effect_1123 = 1123,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect_1124 = 1124,
        /// <summary>
        /// [!]
        /// </summary>
        Effect_1125 = 1125,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect_1126 = 1126,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect_1127 = 1127,
        /// <summary>
        /// (not found)
        /// </summary>
        Effect_1128 = 1128,
        /// <summary>
        /// Send to Krosmaster
        /// </summary>
        Effect_1129 = 1129,
        /// <summary>
        /// #2 HP (Air) lost for every #1 AP used
        /// </summary>
        Effect_DamageAirPerAP = 1131,
        /// <summary>
        /// #2 HP (Water) lost for every #1 AP used
        /// </summary>
        Effect_DamageWaterPerAP = 1132,
        /// <summary>
        /// #2 HP (Fire) lost for every #1 AP used
        /// </summary>
        Effect_DamageFirePerAP = 1133,
        /// <summary>
        /// #2 HP (Neutral) lost for every #1 AP used
        /// </summary>
        Effect_DamageNeutralPerAP = 1134,
        /// <summary>
        /// #2 HP (Earth) lost for every #1 AP used
        /// </summary>
        Effect_DamageEarthPerAP = 1135,
        /// <summary>
        /// #2 HP (Air) lost for every #1 MP used
        /// </summary>
        Effect_DamageAirPerMP = 1136,
        /// <summary>
        /// #2 HP (Water) lost for every #1 MP used
        /// </summary>
        Effect_DamageWaterPerMP = 1137,
        /// <summary>
        /// #2 HP (Fire) lost for every #1 MP used
        /// </summary>
        Effect_DamageFirePerMP = 1138,
        /// <summary>
        /// #2 HP (Neutral) lost for every #1 MP used
        /// </summary>
        Effect_DamageNeutralPerMP = 1139,
        /// <summary>
        /// #2 HP (Earth) lost for every #1 MP used
        /// </summary>
        Effect_DamageEarthPerMP = 1140,
        /// <summary>
        /// [!]
        /// </summary>
        Effect_1141 = 1141,
        /// <summary>
        /// [!]
        /// </summary>
        Effect_1142 = 1142,
        
        
        End,
    }
}