using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class CharacterCharacteristicsInformations : Object
	{
		public const uint protocolId = 8;
		public uint maxLifePoints = 0;
		public uint energyPoints = 0;
		public uint maxEnergyPoints = 0;
		public int actionPointsCurrent = 0;
		public int movementPointsCurrent = 0;
		public CharacterBaseCharacteristic initiative;
		public CharacterBaseCharacteristic prospecting;
		public CharacterBaseCharacteristic actionPoints;
		public CharacterBaseCharacteristic movementPoints;
		public CharacterBaseCharacteristic strength;
		public CharacterBaseCharacteristic vitality;
		public CharacterBaseCharacteristic wisdom;
		public CharacterBaseCharacteristic chance;
		public CharacterBaseCharacteristic agility;
		public CharacterBaseCharacteristic intelligence;
		public CharacterBaseCharacteristic range;
		public CharacterBaseCharacteristic summonableCreaturesBoost;
		public CharacterBaseCharacteristic reflect;
		public CharacterBaseCharacteristic criticalHit;
		public uint criticalHitWeapon = 0;
		public CharacterBaseCharacteristic criticalMiss;
		public CharacterBaseCharacteristic healBonus;
		public CharacterBaseCharacteristic allDamagesBonus;
		public CharacterBaseCharacteristic weaponDamagesBonusPercent;
		public CharacterBaseCharacteristic damagesBonusPercent;
		public CharacterBaseCharacteristic trapBonus;
		public CharacterBaseCharacteristic trapBonusPercent;
		public CharacterBaseCharacteristic permanentDamagePercent;
		public CharacterBaseCharacteristic tackleBlock;
		public CharacterBaseCharacteristic tackleEvade;
		public CharacterBaseCharacteristic PAAttack;
		public CharacterBaseCharacteristic PMAttack;
		public CharacterBaseCharacteristic AddDamageBonus;
		public CharacterBaseCharacteristic criticalDamageBonus;
		public CharacterBaseCharacteristic neutralDamageBonus;
		public CharacterBaseCharacteristic earthDamageBonus;
		public CharacterBaseCharacteristic waterDamageBonus;
		public CharacterBaseCharacteristic airDamageBonus;
		public CharacterBaseCharacteristic fireDamageBonus;
		public CharacterBaseCharacteristic dodgePALostProbability;
		public CharacterBaseCharacteristic dodgePMLostProbability;
		public CharacterBaseCharacteristic neutralElementResistPercent;
		public CharacterBaseCharacteristic earthElementResistPercent;
		public CharacterBaseCharacteristic waterElementResistPercent;
		public CharacterBaseCharacteristic airElementResistPercent;
		public CharacterBaseCharacteristic fireElementResistPercent;
		public CharacterBaseCharacteristic neutralElementReduction;
		public CharacterBaseCharacteristic earthElementReduction;
		public CharacterBaseCharacteristic waterElementReduction;
		public CharacterBaseCharacteristic airElementReduction;
		public CharacterBaseCharacteristic fireElementReduction;
		public double experience = 0;
		public CharacterBaseCharacteristic criticalDamageReduction;
		public CharacterBaseCharacteristic pvpNeutralElementResistPercent;
		public CharacterBaseCharacteristic pvpEarthElementResistPercent;
		public CharacterBaseCharacteristic pvpWaterElementResistPercent;
		public CharacterBaseCharacteristic pvpAirElementResistPercent;
		public CharacterBaseCharacteristic pvpFireElementResistPercent;
		public CharacterBaseCharacteristic pvpNeutralElementReduction;
		public CharacterBaseCharacteristic pvpEarthElementReduction;
		public CharacterBaseCharacteristic pvpWaterElementReduction;
		public CharacterBaseCharacteristic AddDamageReduction;
		public CharacterBaseCharacteristic pvpFireElementReduction;
		public List<CharacterSpellModification> spellModifications;
		public double experienceLevelFloor = 0;
		public double experienceNextLevelFloor = 0;
		public uint kamas = 0;
		public uint statsPoints = 0;
		public uint spellsPoints = 0;
		public ActorExtendedAlignmentInformations alignmentInfos;
		public uint lifePoints = 0;
		public CharacterBaseCharacteristic pvpAirElementReduction;
		
		public CharacterCharacteristicsInformations()
		{
			this.alignmentInfos = new ActorExtendedAlignmentInformations();
			this.initiative = new CharacterBaseCharacteristic();
			this.prospecting = new CharacterBaseCharacteristic();
			this.actionPoints = new CharacterBaseCharacteristic();
			this.movementPoints = new CharacterBaseCharacteristic();
			this.strength = new CharacterBaseCharacteristic();
			this.vitality = new CharacterBaseCharacteristic();
			this.wisdom = new CharacterBaseCharacteristic();
			this.chance = new CharacterBaseCharacteristic();
			this.agility = new CharacterBaseCharacteristic();
			this.intelligence = new CharacterBaseCharacteristic();
			this.range = new CharacterBaseCharacteristic();
			this.summonableCreaturesBoost = new CharacterBaseCharacteristic();
			this.reflect = new CharacterBaseCharacteristic();
			this.criticalHit = new CharacterBaseCharacteristic();
			this.criticalMiss = new CharacterBaseCharacteristic();
			this.healBonus = new CharacterBaseCharacteristic();
			this.allDamagesBonus = new CharacterBaseCharacteristic();
			this.weaponDamagesBonusPercent = new CharacterBaseCharacteristic();
			this.damagesBonusPercent = new CharacterBaseCharacteristic();
			this.trapBonus = new CharacterBaseCharacteristic();
			this.trapBonusPercent = new CharacterBaseCharacteristic();
			this.permanentDamagePercent = new CharacterBaseCharacteristic();
			this.tackleBlock = new CharacterBaseCharacteristic();
			this.tackleEvade = new CharacterBaseCharacteristic();
			this.PAAttack = new CharacterBaseCharacteristic();
			this.PMAttack = new CharacterBaseCharacteristic();
			this.AddDamageBonus = new CharacterBaseCharacteristic();
			this.criticalDamageBonus = new CharacterBaseCharacteristic();
			this.neutralDamageBonus = new CharacterBaseCharacteristic();
			this.earthDamageBonus = new CharacterBaseCharacteristic();
			this.waterDamageBonus = new CharacterBaseCharacteristic();
			this.airDamageBonus = new CharacterBaseCharacteristic();
			this.fireDamageBonus = new CharacterBaseCharacteristic();
			this.dodgePALostProbability = new CharacterBaseCharacteristic();
			this.dodgePMLostProbability = new CharacterBaseCharacteristic();
			this.neutralElementResistPercent = new CharacterBaseCharacteristic();
			this.earthElementResistPercent = new CharacterBaseCharacteristic();
			this.waterElementResistPercent = new CharacterBaseCharacteristic();
			this.airElementResistPercent = new CharacterBaseCharacteristic();
			this.fireElementResistPercent = new CharacterBaseCharacteristic();
			this.neutralElementReduction = new CharacterBaseCharacteristic();
			this.earthElementReduction = new CharacterBaseCharacteristic();
			this.waterElementReduction = new CharacterBaseCharacteristic();
			this.airElementReduction = new CharacterBaseCharacteristic();
			this.fireElementReduction = new CharacterBaseCharacteristic();
			this.AddDamageReduction = new CharacterBaseCharacteristic();
			this.criticalDamageReduction = new CharacterBaseCharacteristic();
			this.pvpNeutralElementResistPercent = new CharacterBaseCharacteristic();
			this.pvpEarthElementResistPercent = new CharacterBaseCharacteristic();
			this.pvpWaterElementResistPercent = new CharacterBaseCharacteristic();
			this.pvpAirElementResistPercent = new CharacterBaseCharacteristic();
			this.pvpFireElementResistPercent = new CharacterBaseCharacteristic();
			this.pvpNeutralElementReduction = new CharacterBaseCharacteristic();
			this.pvpEarthElementReduction = new CharacterBaseCharacteristic();
			this.pvpWaterElementReduction = new CharacterBaseCharacteristic();
			this.pvpAirElementReduction = new CharacterBaseCharacteristic();
			this.pvpFireElementReduction = new CharacterBaseCharacteristic();
			this.spellModifications = new List<CharacterSpellModification>();
		}
		
		public CharacterCharacteristicsInformations(double arg1, double arg2, double arg3, uint arg4, uint arg5, uint arg6, ActorExtendedAlignmentInformations arg7, uint arg8, uint arg9, uint arg10, uint arg11, int arg12, int arg13, CharacterBaseCharacteristic arg14, CharacterBaseCharacteristic arg15, CharacterBaseCharacteristic arg16, CharacterBaseCharacteristic arg17, CharacterBaseCharacteristic arg18, CharacterBaseCharacteristic arg19, CharacterBaseCharacteristic arg20, CharacterBaseCharacteristic arg21, CharacterBaseCharacteristic arg22, CharacterBaseCharacteristic arg23, CharacterBaseCharacteristic arg24, CharacterBaseCharacteristic arg25, CharacterBaseCharacteristic arg26, CharacterBaseCharacteristic arg27, uint arg28, CharacterBaseCharacteristic arg29, CharacterBaseCharacteristic arg30, CharacterBaseCharacteristic arg31, CharacterBaseCharacteristic arg32, CharacterBaseCharacteristic arg33, CharacterBaseCharacteristic arg34, CharacterBaseCharacteristic arg35, CharacterBaseCharacteristic arg36, CharacterBaseCharacteristic arg37, CharacterBaseCharacteristic arg38, CharacterBaseCharacteristic arg39, CharacterBaseCharacteristic arg40, CharacterBaseCharacteristic arg41, CharacterBaseCharacteristic arg42, CharacterBaseCharacteristic arg43, CharacterBaseCharacteristic arg44, CharacterBaseCharacteristic arg45, CharacterBaseCharacteristic arg46, CharacterBaseCharacteristic arg47, CharacterBaseCharacteristic arg48, CharacterBaseCharacteristic arg49, CharacterBaseCharacteristic arg50, CharacterBaseCharacteristic arg51, CharacterBaseCharacteristic arg52, CharacterBaseCharacteristic arg53, CharacterBaseCharacteristic arg54, CharacterBaseCharacteristic arg55, CharacterBaseCharacteristic arg56, CharacterBaseCharacteristic arg57, CharacterBaseCharacteristic arg58, CharacterBaseCharacteristic arg59, CharacterBaseCharacteristic arg60, CharacterBaseCharacteristic arg61, CharacterBaseCharacteristic arg62, CharacterBaseCharacteristic arg63, CharacterBaseCharacteristic arg64, CharacterBaseCharacteristic arg65, CharacterBaseCharacteristic arg66, CharacterBaseCharacteristic arg67, CharacterBaseCharacteristic arg68, CharacterBaseCharacteristic arg69, CharacterBaseCharacteristic arg70, CharacterBaseCharacteristic arg71, List<CharacterSpellModification> arg72)
			: this()
		{
			initCharacterCharacteristicsInformations(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20, arg21, arg22, arg23, arg24, arg25, arg26, arg27, arg28, arg29, arg30, arg31, arg32, arg33, arg34, arg35, arg36, arg37, arg38, arg39, arg40, arg41, arg42, arg43, arg44, arg45, arg46, arg47, arg48, arg49, arg50, arg51, arg52, arg53, arg54, arg55, arg56, arg57, arg58, arg59, arg60, arg61, arg62, arg63, arg64, arg65, arg66, arg67, arg68, arg69, arg70, arg71, arg72);
		}
		
		public virtual uint getTypeId()
		{
			return 8;
		}
		
		public CharacterCharacteristicsInformations initCharacterCharacteristicsInformations(double arg1 = 0, double arg2 = 0, double arg3 = 0, uint arg4 = 0, uint arg5 = 0, uint arg6 = 0, ActorExtendedAlignmentInformations arg7 = null, uint arg8 = 0, uint arg9 = 0, uint arg10 = 0, uint arg11 = 0, int arg12 = 0, int arg13 = 0, CharacterBaseCharacteristic arg14 = null, CharacterBaseCharacteristic arg15 = null, CharacterBaseCharacteristic arg16 = null, CharacterBaseCharacteristic arg17 = null, CharacterBaseCharacteristic arg18 = null, CharacterBaseCharacteristic arg19 = null, CharacterBaseCharacteristic arg20 = null, CharacterBaseCharacteristic arg21 = null, CharacterBaseCharacteristic arg22 = null, CharacterBaseCharacteristic arg23 = null, CharacterBaseCharacteristic arg24 = null, CharacterBaseCharacteristic arg25 = null, CharacterBaseCharacteristic arg26 = null, CharacterBaseCharacteristic arg27 = null, uint arg28 = 0, CharacterBaseCharacteristic arg29 = null, CharacterBaseCharacteristic arg30 = null, CharacterBaseCharacteristic arg31 = null, CharacterBaseCharacteristic arg32 = null, CharacterBaseCharacteristic arg33 = null, CharacterBaseCharacteristic arg34 = null, CharacterBaseCharacteristic arg35 = null, CharacterBaseCharacteristic arg36 = null, CharacterBaseCharacteristic arg37 = null, CharacterBaseCharacteristic arg38 = null, CharacterBaseCharacteristic arg39 = null, CharacterBaseCharacteristic arg40 = null, CharacterBaseCharacteristic arg41 = null, CharacterBaseCharacteristic arg42 = null, CharacterBaseCharacteristic arg43 = null, CharacterBaseCharacteristic arg44 = null, CharacterBaseCharacteristic arg45 = null, CharacterBaseCharacteristic arg46 = null, CharacterBaseCharacteristic arg47 = null, CharacterBaseCharacteristic arg48 = null, CharacterBaseCharacteristic arg49 = null, CharacterBaseCharacteristic arg50 = null, CharacterBaseCharacteristic arg51 = null, CharacterBaseCharacteristic arg52 = null, CharacterBaseCharacteristic arg53 = null, CharacterBaseCharacteristic arg54 = null, CharacterBaseCharacteristic arg55 = null, CharacterBaseCharacteristic arg56 = null, CharacterBaseCharacteristic arg57 = null, CharacterBaseCharacteristic arg58 = null, CharacterBaseCharacteristic arg59 = null, CharacterBaseCharacteristic arg60 = null, CharacterBaseCharacteristic arg61 = null, CharacterBaseCharacteristic arg62 = null, CharacterBaseCharacteristic arg63 = null, CharacterBaseCharacteristic arg64 = null, CharacterBaseCharacteristic arg65 = null, CharacterBaseCharacteristic arg66 = null, CharacterBaseCharacteristic arg67 = null, CharacterBaseCharacteristic arg68 = null, CharacterBaseCharacteristic arg69 = null, CharacterBaseCharacteristic arg70 = null, CharacterBaseCharacteristic arg71 = null, List<CharacterSpellModification> arg72 = null)
		{
			this.experience = arg1;
			this.experienceLevelFloor = arg2;
			this.experienceNextLevelFloor = arg3;
			this.kamas = arg4;
			this.statsPoints = arg5;
			this.spellsPoints = arg6;
			this.alignmentInfos = arg7;
			this.lifePoints = arg8;
			this.maxLifePoints = arg9;
			this.energyPoints = arg10;
			this.maxEnergyPoints = arg11;
			this.actionPointsCurrent = arg12;
			this.movementPointsCurrent = arg13;
			this.initiative = arg14;
			this.prospecting = arg15;
			this.actionPoints = arg16;
			this.movementPoints = arg17;
			this.strength = arg18;
			this.vitality = arg19;
			this.wisdom = arg20;
			this.chance = arg21;
			this.agility = arg22;
			this.intelligence = arg23;
			this.range = arg24;
			this.summonableCreaturesBoost = arg25;
			this.reflect = arg26;
			this.criticalHit = arg27;
			this.criticalHitWeapon = arg28;
			this.criticalMiss = arg29;
			this.healBonus = arg30;
			this.allDamagesBonus = arg31;
			this.weaponDamagesBonusPercent = arg32;
			this.damagesBonusPercent = arg33;
			this.trapBonus = arg34;
			this.trapBonusPercent = arg35;
			this.permanentDamagePercent = arg36;
			this.tackleBlock = arg37;
			this.tackleEvade = arg38;
			this.PAAttack = arg39;
			this.PMAttack = arg40;
			this.AddDamageBonus = arg41;
			this.criticalDamageBonus = arg42;
			this.neutralDamageBonus = arg43;
			this.earthDamageBonus = arg44;
			this.waterDamageBonus = arg45;
			this.airDamageBonus = arg46;
			this.fireDamageBonus = arg47;
			this.dodgePALostProbability = arg48;
			this.dodgePMLostProbability = arg49;
			this.neutralElementResistPercent = arg50;
			this.earthElementResistPercent = arg51;
			this.waterElementResistPercent = arg52;
			this.airElementResistPercent = arg53;
			this.fireElementResistPercent = arg54;
			this.neutralElementReduction = arg55;
			this.earthElementReduction = arg56;
			this.waterElementReduction = arg57;
			this.airElementReduction = arg58;
			this.fireElementReduction = arg59;
			this.AddDamageReduction = arg60;
			this.criticalDamageReduction = arg61;
			this.pvpNeutralElementResistPercent = arg62;
			this.pvpEarthElementResistPercent = arg63;
			this.pvpWaterElementResistPercent = arg64;
			this.pvpAirElementResistPercent = arg65;
			this.pvpFireElementResistPercent = arg66;
			this.pvpNeutralElementReduction = arg67;
			this.pvpEarthElementReduction = arg68;
			this.pvpWaterElementReduction = arg69;
			this.pvpAirElementReduction = arg70;
			this.pvpFireElementReduction = arg71;
			this.spellModifications = arg72;
			return this;
		}
		
		public virtual void reset()
		{
			this.experience = 0;
			this.experienceLevelFloor = 0;
			this.experienceNextLevelFloor = 0;
			this.kamas = 0;
			this.statsPoints = 0;
			this.spellsPoints = 0;
			this.alignmentInfos = new ActorExtendedAlignmentInformations();
			this.maxLifePoints = 0;
			this.energyPoints = 0;
			this.maxEnergyPoints = 0;
			this.actionPointsCurrent = 0;
			this.movementPointsCurrent = 0;
			this.initiative = new CharacterBaseCharacteristic();
			this.criticalMiss = new CharacterBaseCharacteristic();
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_CharacterCharacteristicsInformations(arg1);
		}
		
		public void serializeAs_CharacterCharacteristicsInformations(BigEndianWriter arg1)
		{
			if ( this.experience < 0 )
			{
				throw new Exception("Forbidden value (" + this.experience + ") on element experience.");
			}
			arg1.WriteDouble(this.experience);
			if ( this.experienceLevelFloor < 0 )
			{
				throw new Exception("Forbidden value (" + this.experienceLevelFloor + ") on element experienceLevelFloor.");
			}
			arg1.WriteDouble(this.experienceLevelFloor);
			if ( this.experienceNextLevelFloor < 0 )
			{
				throw new Exception("Forbidden value (" + this.experienceNextLevelFloor + ") on element experienceNextLevelFloor.");
			}
			arg1.WriteDouble(this.experienceNextLevelFloor);
			if ( this.kamas < 0 )
			{
				throw new Exception("Forbidden value (" + this.kamas + ") on element kamas.");
			}
			arg1.WriteInt((int)this.kamas);
			if ( this.statsPoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.statsPoints + ") on element statsPoints.");
			}
			arg1.WriteInt((int)this.statsPoints);
			if ( this.spellsPoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.spellsPoints + ") on element spellsPoints.");
			}
			arg1.WriteInt((int)this.spellsPoints);
			this.alignmentInfos.serializeAs_ActorExtendedAlignmentInformations(arg1);
			if ( this.lifePoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.lifePoints + ") on element lifePoints.");
			}
			arg1.WriteInt((int)this.lifePoints);
			if ( this.maxLifePoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxLifePoints + ") on element maxLifePoints.");
			}
			arg1.WriteInt((int)this.maxLifePoints);
			if ( this.energyPoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.energyPoints + ") on element energyPoints.");
			}
			arg1.WriteShort((short)this.energyPoints);
			if ( this.maxEnergyPoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxEnergyPoints + ") on element maxEnergyPoints.");
			}
			arg1.WriteShort((short)this.maxEnergyPoints);
			arg1.WriteShort((short)this.actionPointsCurrent);
			arg1.WriteShort((short)this.movementPointsCurrent);
			this.initiative.serializeAs_CharacterBaseCharacteristic(arg1);
			this.prospecting.serializeAs_CharacterBaseCharacteristic(arg1);
			this.actionPoints.serializeAs_CharacterBaseCharacteristic(arg1);
			this.movementPoints.serializeAs_CharacterBaseCharacteristic(arg1);
			this.strength.serializeAs_CharacterBaseCharacteristic(arg1);
			this.vitality.serializeAs_CharacterBaseCharacteristic(arg1);
			this.wisdom.serializeAs_CharacterBaseCharacteristic(arg1);
			this.chance.serializeAs_CharacterBaseCharacteristic(arg1);
			this.agility.serializeAs_CharacterBaseCharacteristic(arg1);
			this.intelligence.serializeAs_CharacterBaseCharacteristic(arg1);
			this.range.serializeAs_CharacterBaseCharacteristic(arg1);
			this.summonableCreaturesBoost.serializeAs_CharacterBaseCharacteristic(arg1);
			this.reflect.serializeAs_CharacterBaseCharacteristic(arg1);
			this.criticalHit.serializeAs_CharacterBaseCharacteristic(arg1);
			if ( this.criticalHitWeapon < 0 )
			{
				throw new Exception("Forbidden value (" + this.criticalHitWeapon + ") on element criticalHitWeapon.");
			}
			arg1.WriteShort((short)this.criticalHitWeapon);
			this.criticalMiss.serializeAs_CharacterBaseCharacteristic(arg1);
			this.healBonus.serializeAs_CharacterBaseCharacteristic(arg1);
			this.allDamagesBonus.serializeAs_CharacterBaseCharacteristic(arg1);
			this.weaponDamagesBonusPercent.serializeAs_CharacterBaseCharacteristic(arg1);
			this.damagesBonusPercent.serializeAs_CharacterBaseCharacteristic(arg1);
			this.trapBonus.serializeAs_CharacterBaseCharacteristic(arg1);
			this.trapBonusPercent.serializeAs_CharacterBaseCharacteristic(arg1);
			this.permanentDamagePercent.serializeAs_CharacterBaseCharacteristic(arg1);
			this.tackleBlock.serializeAs_CharacterBaseCharacteristic(arg1);
			this.tackleEvade.serializeAs_CharacterBaseCharacteristic(arg1);
			this.PAAttack.serializeAs_CharacterBaseCharacteristic(arg1);
			this.PMAttack.serializeAs_CharacterBaseCharacteristic(arg1);
			this.AddDamageBonus.serializeAs_CharacterBaseCharacteristic(arg1);
			this.criticalDamageBonus.serializeAs_CharacterBaseCharacteristic(arg1);
			this.neutralDamageBonus.serializeAs_CharacterBaseCharacteristic(arg1);
			this.earthDamageBonus.serializeAs_CharacterBaseCharacteristic(arg1);
			this.waterDamageBonus.serializeAs_CharacterBaseCharacteristic(arg1);
			this.airDamageBonus.serializeAs_CharacterBaseCharacteristic(arg1);
			this.fireDamageBonus.serializeAs_CharacterBaseCharacteristic(arg1);
			this.dodgePALostProbability.serializeAs_CharacterBaseCharacteristic(arg1);
			this.dodgePMLostProbability.serializeAs_CharacterBaseCharacteristic(arg1);
			this.neutralElementResistPercent.serializeAs_CharacterBaseCharacteristic(arg1);
			this.earthElementResistPercent.serializeAs_CharacterBaseCharacteristic(arg1);
			this.waterElementResistPercent.serializeAs_CharacterBaseCharacteristic(arg1);
			this.airElementResistPercent.serializeAs_CharacterBaseCharacteristic(arg1);
			this.fireElementResistPercent.serializeAs_CharacterBaseCharacteristic(arg1);
			this.neutralElementReduction.serializeAs_CharacterBaseCharacteristic(arg1);
			this.earthElementReduction.serializeAs_CharacterBaseCharacteristic(arg1);
			this.waterElementReduction.serializeAs_CharacterBaseCharacteristic(arg1);
			this.airElementReduction.serializeAs_CharacterBaseCharacteristic(arg1);
			this.fireElementReduction.serializeAs_CharacterBaseCharacteristic(arg1);
			this.AddDamageReduction.serializeAs_CharacterBaseCharacteristic(arg1);
			this.criticalDamageReduction.serializeAs_CharacterBaseCharacteristic(arg1);
			this.pvpNeutralElementResistPercent.serializeAs_CharacterBaseCharacteristic(arg1);
			this.pvpEarthElementResistPercent.serializeAs_CharacterBaseCharacteristic(arg1);
			this.pvpWaterElementResistPercent.serializeAs_CharacterBaseCharacteristic(arg1);
			this.pvpAirElementResistPercent.serializeAs_CharacterBaseCharacteristic(arg1);
			this.pvpFireElementResistPercent.serializeAs_CharacterBaseCharacteristic(arg1);
			this.pvpNeutralElementReduction.serializeAs_CharacterBaseCharacteristic(arg1);
			this.pvpEarthElementReduction.serializeAs_CharacterBaseCharacteristic(arg1);
			this.pvpWaterElementReduction.serializeAs_CharacterBaseCharacteristic(arg1);
			this.pvpAirElementReduction.serializeAs_CharacterBaseCharacteristic(arg1);
			this.pvpFireElementReduction.serializeAs_CharacterBaseCharacteristic(arg1);
			arg1.WriteShort((short)this.spellModifications.Count);
			var loc1 = 0;
			while ( loc1 < this.spellModifications.Count )
			{
				this.spellModifications[loc1].serializeAs_CharacterSpellModification(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CharacterCharacteristicsInformations(arg1);
		}
		
		public void deserializeAs_CharacterCharacteristicsInformations(BigEndianReader arg1)
		{
			object loc3 = null;
			this.experience = (double)arg1.ReadDouble();
			if ( this.experience < 0 )
			{
				throw new Exception("Forbidden value (" + this.experience + ") on element of CharacterCharacteristicsInformations.experience.");
			}
			this.experienceLevelFloor = (double)arg1.ReadDouble();
			if ( this.experienceLevelFloor < 0 )
			{
				throw new Exception("Forbidden value (" + this.experienceLevelFloor + ") on element of CharacterCharacteristicsInformations.experienceLevelFloor.");
			}
			this.experienceNextLevelFloor = (double)arg1.ReadDouble();
			if ( this.experienceNextLevelFloor < 0 )
			{
				throw new Exception("Forbidden value (" + this.experienceNextLevelFloor + ") on element of CharacterCharacteristicsInformations.experienceNextLevelFloor.");
			}
			this.kamas = (uint)arg1.ReadInt();
			if ( this.kamas < 0 )
			{
				throw new Exception("Forbidden value (" + this.kamas + ") on element of CharacterCharacteristicsInformations.kamas.");
			}
			this.statsPoints = (uint)arg1.ReadInt();
			if ( this.statsPoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.statsPoints + ") on element of CharacterCharacteristicsInformations.statsPoints.");
			}
			this.spellsPoints = (uint)arg1.ReadInt();
			if ( this.spellsPoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.spellsPoints + ") on element of CharacterCharacteristicsInformations.spellsPoints.");
			}
			this.alignmentInfos = new ActorExtendedAlignmentInformations();
			this.alignmentInfos.deserialize(arg1);
			this.lifePoints = (uint)arg1.ReadInt();
			if ( this.lifePoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.lifePoints + ") on element of CharacterCharacteristicsInformations.lifePoints.");
			}
			this.maxLifePoints = (uint)arg1.ReadInt();
			if ( this.maxLifePoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxLifePoints + ") on element of CharacterCharacteristicsInformations.maxLifePoints.");
			}
			this.energyPoints = (uint)arg1.ReadShort();
			if ( this.energyPoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.energyPoints + ") on element of CharacterCharacteristicsInformations.energyPoints.");
			}
			this.maxEnergyPoints = (uint)arg1.ReadShort();
			if ( this.maxEnergyPoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxEnergyPoints + ") on element of CharacterCharacteristicsInformations.maxEnergyPoints.");
			}
			this.actionPointsCurrent = (int)arg1.ReadShort();
			this.movementPointsCurrent = (int)arg1.ReadShort();
			this.initiative = new CharacterBaseCharacteristic();
			this.initiative.deserialize(arg1);
			this.prospecting = new CharacterBaseCharacteristic();
			this.prospecting.deserialize(arg1);
			this.actionPoints = new CharacterBaseCharacteristic();
			this.actionPoints.deserialize(arg1);
			this.movementPoints = new CharacterBaseCharacteristic();
			this.movementPoints.deserialize(arg1);
			this.strength = new CharacterBaseCharacteristic();
			this.strength.deserialize(arg1);
			this.vitality = new CharacterBaseCharacteristic();
			this.vitality.deserialize(arg1);
			this.wisdom = new CharacterBaseCharacteristic();
			this.wisdom.deserialize(arg1);
			this.chance = new CharacterBaseCharacteristic();
			this.chance.deserialize(arg1);
			this.agility = new CharacterBaseCharacteristic();
			this.agility.deserialize(arg1);
			this.intelligence = new CharacterBaseCharacteristic();
			this.intelligence.deserialize(arg1);
			this.range = new CharacterBaseCharacteristic();
			this.range.deserialize(arg1);
			this.summonableCreaturesBoost = new CharacterBaseCharacteristic();
			this.summonableCreaturesBoost.deserialize(arg1);
			this.reflect = new CharacterBaseCharacteristic();
			this.reflect.deserialize(arg1);
			this.criticalHit = new CharacterBaseCharacteristic();
			this.criticalHit.deserialize(arg1);
			this.criticalHitWeapon = (uint)arg1.ReadShort();
			if ( this.criticalHitWeapon < 0 )
			{
				throw new Exception("Forbidden value (" + this.criticalHitWeapon + ") on element of CharacterCharacteristicsInformations.criticalHitWeapon.");
			}
			this.criticalMiss = new CharacterBaseCharacteristic();
			this.criticalMiss.deserialize(arg1);
			this.healBonus = new CharacterBaseCharacteristic();
			this.healBonus.deserialize(arg1);
			this.allDamagesBonus = new CharacterBaseCharacteristic();
			this.allDamagesBonus.deserialize(arg1);
			this.weaponDamagesBonusPercent = new CharacterBaseCharacteristic();
			this.weaponDamagesBonusPercent.deserialize(arg1);
			this.damagesBonusPercent = new CharacterBaseCharacteristic();
			this.damagesBonusPercent.deserialize(arg1);
			this.trapBonus = new CharacterBaseCharacteristic();
			this.trapBonus.deserialize(arg1);
			this.trapBonusPercent = new CharacterBaseCharacteristic();
			this.trapBonusPercent.deserialize(arg1);
			this.permanentDamagePercent = new CharacterBaseCharacteristic();
			this.permanentDamagePercent.deserialize(arg1);
			this.tackleBlock = new CharacterBaseCharacteristic();
			this.tackleBlock.deserialize(arg1);
			this.tackleEvade = new CharacterBaseCharacteristic();
			this.tackleEvade.deserialize(arg1);
			this.PAAttack = new CharacterBaseCharacteristic();
			this.PAAttack.deserialize(arg1);
			this.PMAttack = new CharacterBaseCharacteristic();
			this.PMAttack.deserialize(arg1);
			this.AddDamageBonus = new CharacterBaseCharacteristic();
			this.AddDamageBonus.deserialize(arg1);
			this.criticalDamageBonus = new CharacterBaseCharacteristic();
			this.criticalDamageBonus.deserialize(arg1);
			this.neutralDamageBonus = new CharacterBaseCharacteristic();
			this.neutralDamageBonus.deserialize(arg1);
			this.earthDamageBonus = new CharacterBaseCharacteristic();
			this.earthDamageBonus.deserialize(arg1);
			this.waterDamageBonus = new CharacterBaseCharacteristic();
			this.waterDamageBonus.deserialize(arg1);
			this.airDamageBonus = new CharacterBaseCharacteristic();
			this.airDamageBonus.deserialize(arg1);
			this.fireDamageBonus = new CharacterBaseCharacteristic();
			this.fireDamageBonus.deserialize(arg1);
			this.dodgePALostProbability = new CharacterBaseCharacteristic();
			this.dodgePALostProbability.deserialize(arg1);
			this.dodgePMLostProbability = new CharacterBaseCharacteristic();
			this.dodgePMLostProbability.deserialize(arg1);
			this.neutralElementResistPercent = new CharacterBaseCharacteristic();
			this.neutralElementResistPercent.deserialize(arg1);
			this.earthElementResistPercent = new CharacterBaseCharacteristic();
			this.earthElementResistPercent.deserialize(arg1);
			this.waterElementResistPercent = new CharacterBaseCharacteristic();
			this.waterElementResistPercent.deserialize(arg1);
			this.airElementResistPercent = new CharacterBaseCharacteristic();
			this.airElementResistPercent.deserialize(arg1);
			this.fireElementResistPercent = new CharacterBaseCharacteristic();
			this.fireElementResistPercent.deserialize(arg1);
			this.neutralElementReduction = new CharacterBaseCharacteristic();
			this.neutralElementReduction.deserialize(arg1);
			this.earthElementReduction = new CharacterBaseCharacteristic();
			this.earthElementReduction.deserialize(arg1);
			this.waterElementReduction = new CharacterBaseCharacteristic();
			this.waterElementReduction.deserialize(arg1);
			this.airElementReduction = new CharacterBaseCharacteristic();
			this.airElementReduction.deserialize(arg1);
			this.fireElementReduction = new CharacterBaseCharacteristic();
			this.fireElementReduction.deserialize(arg1);
			this.AddDamageReduction = new CharacterBaseCharacteristic();
			this.AddDamageReduction.deserialize(arg1);
			this.criticalDamageReduction = new CharacterBaseCharacteristic();
			this.criticalDamageReduction.deserialize(arg1);
			this.pvpNeutralElementResistPercent = new CharacterBaseCharacteristic();
			this.pvpNeutralElementResistPercent.deserialize(arg1);
			this.pvpEarthElementResistPercent = new CharacterBaseCharacteristic();
			this.pvpEarthElementResistPercent.deserialize(arg1);
			this.pvpWaterElementResistPercent = new CharacterBaseCharacteristic();
			this.pvpWaterElementResistPercent.deserialize(arg1);
			this.pvpAirElementResistPercent = new CharacterBaseCharacteristic();
			this.pvpAirElementResistPercent.deserialize(arg1);
			this.pvpFireElementResistPercent = new CharacterBaseCharacteristic();
			this.pvpFireElementResistPercent.deserialize(arg1);
			this.pvpNeutralElementReduction = new CharacterBaseCharacteristic();
			this.pvpNeutralElementReduction.deserialize(arg1);
			this.pvpEarthElementReduction = new CharacterBaseCharacteristic();
			this.pvpEarthElementReduction.deserialize(arg1);
			this.pvpWaterElementReduction = new CharacterBaseCharacteristic();
			this.pvpWaterElementReduction.deserialize(arg1);
			this.pvpAirElementReduction = new CharacterBaseCharacteristic();
			this.pvpAirElementReduction.deserialize(arg1);
			this.pvpFireElementReduction = new CharacterBaseCharacteristic();
			this.pvpFireElementReduction.deserialize(arg1);
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new CharacterSpellModification()) as CharacterSpellModification).deserialize(arg1);
				this.spellModifications.Add((CharacterSpellModification)loc3);
				++loc2;
			}
		}
		
	}
}
