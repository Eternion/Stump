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
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class MountClientData : Object
	{
		public const uint protocolId = 178;
		public double id = 0;
		public uint model = 0;
		public List<uint> ancestor;
		public List<uint> behaviors;
		public String name = "";
		public Boolean sex = false;
		public uint ownerId = 0;
		public double experience = 0;
		public double experienceForLevel = 0;
		public double experienceForNextLevel = 0;
		public uint level = 0;
		public Boolean isRideable = false;
		public int serenity = 0;
		public int aggressivityMax = 0;
		public uint serenityMax = 0;
		public uint love = 0;
		public uint loveMax = 0;
		public int fecondationTime = 0;
		public Boolean isFecondationReady = false;
		public uint boostLimiter = 0;
		public double boostMax = 0;
		public int reproductionCount = 0;
		public uint reproductionCountMax = 0;
		public List<ObjectEffectInteger> effectList;
		public uint energy = 0;
		public uint maturityForAdult = 0;
		public uint maturity = 0;
		public uint staminaMax = 0;
		public uint stamina = 0;
		public Boolean isWild = false;
		public uint maxPods = 0;
		public uint energyMax = 0;
		
		public MountClientData()
		{
			this.ancestor = new List<uint>();
			this.behaviors = new List<uint>();
			this.effectList = new List<ObjectEffectInteger>();
		}
		
		public MountClientData(double arg1, uint arg2, List<uint> arg3, List<uint> arg4, String arg5, Boolean arg6, uint arg7, double arg8, double arg9, double arg10, uint arg11, Boolean arg12, uint arg13, Boolean arg14, uint arg15, uint arg16, uint arg17, uint arg18, uint arg19, uint arg20, int arg21, int arg22, uint arg23, uint arg24, uint arg25, int arg26, Boolean arg27, uint arg28, double arg29, int arg30, uint arg31, List<ObjectEffectInteger> arg32)
			: this()
		{
			initMountClientData(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, arg17, arg18, arg19, arg20, arg21, arg22, arg23, arg24, arg25, arg26, arg27, arg28, arg29, arg30, arg31, arg32);
		}
		
		public void serializeAs_MountClientData(BigEndianWriter arg1)
		{
			var loc1 = 0;
			loc1 = BooleanByteWrapper.SetFlag(loc1, 0, this.sex);
			loc1 = BooleanByteWrapper.SetFlag(loc1, 1, this.isRideable);
			loc1 = BooleanByteWrapper.SetFlag(loc1, 2, this.isWild);
			loc1 = BooleanByteWrapper.SetFlag(loc1, 3, this.isFecondationReady);
			arg1.WriteByte((byte)loc1);
			arg1.WriteDouble(this.id);
			if ( this.model < 0 )
			{
				throw new Exception("Forbidden value (" + this.model + ") on element model.");
			}
			arg1.WriteInt((int)this.model);
			arg1.WriteShort((short)this.ancestor.Count);
			var loc2 = 0;
			while ( loc2 < this.ancestor.Count )
			{
				if ( this.ancestor[loc2] < 0 )
				{
					throw new Exception("Forbidden value (" + this.ancestor[loc2] + ") on element 3 (starting at 1) of ancestor.");
				}
				arg1.WriteInt((int)this.ancestor[loc2]);
				++loc2;
			}
			arg1.WriteShort((short)this.behaviors.Count);
			var loc3 = 0;
			while ( loc3 < this.behaviors.Count )
			{
				if ( this.behaviors[loc3] < 0 )
				{
					throw new Exception("Forbidden value (" + this.behaviors[loc3] + ") on element 4 (starting at 1) of behaviors.");
				}
				arg1.WriteInt((int)this.behaviors[loc3]);
				++loc3;
			}
			arg1.WriteUTF((string)this.name);
			if ( this.ownerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.ownerId + ") on element ownerId.");
			}
			arg1.WriteInt((int)this.ownerId);
			arg1.WriteDouble(this.experience);
			arg1.WriteDouble(this.experienceForLevel);
			arg1.WriteDouble(this.experienceForNextLevel);
			if ( this.level < 0 )
			{
				throw new Exception("Forbidden value (" + this.level + ") on element level.");
			}
			arg1.WriteByte((byte)this.level);
			if ( this.maxPods < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxPods + ") on element maxPods.");
			}
			arg1.WriteInt((int)this.maxPods);
			if ( this.stamina < 0 )
			{
				throw new Exception("Forbidden value (" + this.stamina + ") on element stamina.");
			}
			arg1.WriteInt((int)this.stamina);
			if ( this.staminaMax < 0 )
			{
				throw new Exception("Forbidden value (" + this.staminaMax + ") on element staminaMax.");
			}
			arg1.WriteInt((int)this.staminaMax);
			if ( this.maturity < 0 )
			{
				throw new Exception("Forbidden value (" + this.maturity + ") on element maturity.");
			}
			arg1.WriteInt((int)this.maturity);
			if ( this.maturityForAdult < 0 )
			{
				throw new Exception("Forbidden value (" + this.maturityForAdult + ") on element maturityForAdult.");
			}
			arg1.WriteInt((int)this.maturityForAdult);
			if ( this.energy < 0 )
			{
				throw new Exception("Forbidden value (" + this.energy + ") on element energy.");
			}
			arg1.WriteInt((int)this.energy);
			if ( this.energyMax < 0 )
			{
				throw new Exception("Forbidden value (" + this.energyMax + ") on element energyMax.");
			}
			arg1.WriteInt((int)this.energyMax);
			arg1.WriteInt((int)this.serenity);
			arg1.WriteInt((int)this.aggressivityMax);
			if ( this.serenityMax < 0 )
			{
				throw new Exception("Forbidden value (" + this.serenityMax + ") on element serenityMax.");
			}
			arg1.WriteInt((int)this.serenityMax);
			if ( this.love < 0 )
			{
				throw new Exception("Forbidden value (" + this.love + ") on element love.");
			}
			arg1.WriteInt((int)this.love);
			if ( this.loveMax < 0 )
			{
				throw new Exception("Forbidden value (" + this.loveMax + ") on element loveMax.");
			}
			arg1.WriteInt((int)this.loveMax);
			arg1.WriteInt((int)this.fecondationTime);
			if ( this.boostLimiter < 0 )
			{
				throw new Exception("Forbidden value (" + this.boostLimiter + ") on element boostLimiter.");
			}
			arg1.WriteInt((int)this.boostLimiter);
			arg1.WriteDouble(this.boostMax);
			arg1.WriteInt((int)this.reproductionCount);
			if ( this.reproductionCountMax < 0 )
			{
				throw new Exception("Forbidden value (" + this.reproductionCountMax + ") on element reproductionCountMax.");
			}
			arg1.WriteInt((int)this.reproductionCountMax);
			arg1.WriteShort((short)this.effectList.Count);
			var loc4 = 0;
			while ( loc4 < this.effectList.Count )
			{
				this.effectList[loc4].serializeAs_ObjectEffectInteger(arg1);
				++loc4;
			}
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_MountClientData(arg1);
		}
		
		public virtual void reset()
		{
			this.id = 0;
			this.model = 0;
			this.ancestor = new List<uint>();
			this.behaviors = new List<uint>();
			this.name = "";
			this.sex = false;
			this.ownerId = 0;
			this.experience = 0;
			this.experienceForLevel = 0;
			this.experienceForNextLevel = 0;
			this.level = 0;
			this.isRideable = false;
			this.maxPods = 0;
			this.isWild = false;
			this.stamina = 0;
			this.staminaMax = 0;
			this.maturity = 0;
			this.maturityForAdult = 0;
			this.energy = 0;
			this.energyMax = 0;
			this.serenity = 0;
			this.aggressivityMax = 0;
			this.serenityMax = 0;
			this.love = 0;
			this.loveMax = 0;
			this.fecondationTime = 0;
			this.isFecondationReady = false;
			this.boostLimiter = 0;
			this.boostMax = 0;
			this.reproductionCount = 0;
			this.reproductionCountMax = 0;
			this.effectList = new List<ObjectEffectInteger>();
		}
		
		public MountClientData initMountClientData(double arg1 = 0, uint arg2 = 0, List<uint> arg3 = null, List<uint> arg4 = null, String arg5 = "", Boolean arg6 = false, uint arg7 = 0, double arg8 = 0, double arg9 = 0, double arg10 = 0, uint arg11 = 0, Boolean arg12 = false, uint arg13 = 0, Boolean arg14 = false, uint arg15 = 0, uint arg16 = 0, uint arg17 = 0, uint arg18 = 0, uint arg19 = 0, uint arg20 = 0, int arg21 = 0, int arg22 = 0, uint arg23 = 0, uint arg24 = 0, uint arg25 = 0, int arg26 = 0, Boolean arg27 = false, uint arg28 = 0, double arg29 = 0, int arg30 = 0, uint arg31 = 0, List<ObjectEffectInteger> arg32 = null)
		{
			this.id = arg1;
			this.model = arg2;
			this.ancestor = arg3;
			this.behaviors = arg4;
			this.name = arg5;
			this.sex = arg6;
			this.ownerId = arg7;
			this.experience = arg8;
			this.experienceForLevel = arg9;
			this.experienceForNextLevel = arg10;
			this.level = arg11;
			this.isRideable = arg12;
			this.maxPods = arg13;
			this.isWild = arg14;
			this.stamina = arg15;
			this.staminaMax = arg16;
			this.maturity = arg17;
			this.maturityForAdult = arg18;
			this.energy = arg19;
			this.energyMax = arg20;
			this.serenity = arg21;
			this.aggressivityMax = arg22;
			this.serenityMax = arg23;
			this.love = arg24;
			this.loveMax = arg25;
			this.fecondationTime = arg26;
			this.isFecondationReady = arg27;
			this.boostLimiter = arg28;
			this.boostMax = arg29;
			this.reproductionCount = arg30;
			this.reproductionCountMax = arg31;
			this.effectList = arg32;
			return this;
		}
		
		public virtual uint getTypeId()
		{
			return 178;
		}
		
		public void deserializeAs_MountClientData(BigEndianReader arg1)
		{
			var loc8 = 0;
			var loc9 = 0;
			object loc10 = null;
			var loc1 = arg1.ReadByte();
			this.sex = (Boolean)BooleanByteWrapper.GetFlag(loc1, 0);
			this.isRideable = (Boolean)BooleanByteWrapper.GetFlag(loc1, 1);
			this.isWild = (Boolean)BooleanByteWrapper.GetFlag(loc1, 2);
			this.isFecondationReady = (Boolean)BooleanByteWrapper.GetFlag(loc1, 3);
			this.id = (double)arg1.ReadDouble();
			this.model = (uint)arg1.ReadInt();
			if ( this.model < 0 )
			{
				throw new Exception("Forbidden value (" + this.model + ") on element of MountClientData.model.");
			}
			var loc2 = (ushort)arg1.ReadUShort();
			var loc3 = 0;
			while ( loc3 < loc2 )
			{
				if ( (loc8 = arg1.ReadInt()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc8 + ") on elements of ancestor.");
				}
				this.ancestor.Add((uint)loc8);
				++loc3;
			}
			var loc4 = (ushort)arg1.ReadUShort();
			var loc5 = 0;
			while ( loc5 < loc4 )
			{
				if ( (loc9 = arg1.ReadInt()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc9 + ") on elements of behaviors.");
				}
				this.behaviors.Add((uint)loc9);
				++loc5;
			}
			this.name = (String)arg1.ReadUTF();
			this.ownerId = (uint)arg1.ReadInt();
			if ( this.ownerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.ownerId + ") on element of MountClientData.ownerId.");
			}
			this.experience = (double)arg1.ReadDouble();
			this.experienceForLevel = (double)arg1.ReadDouble();
			this.experienceForNextLevel = (double)arg1.ReadDouble();
			this.level = (uint)arg1.ReadByte();
			if ( this.level < 0 )
			{
				throw new Exception("Forbidden value (" + this.level + ") on element of MountClientData.level.");
			}
			this.maxPods = (uint)arg1.ReadInt();
			if ( this.maxPods < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxPods + ") on element of MountClientData.maxPods.");
			}
			this.stamina = (uint)arg1.ReadInt();
			if ( this.stamina < 0 )
			{
				throw new Exception("Forbidden value (" + this.stamina + ") on element of MountClientData.stamina.");
			}
			this.staminaMax = (uint)arg1.ReadInt();
			if ( this.staminaMax < 0 )
			{
				throw new Exception("Forbidden value (" + this.staminaMax + ") on element of MountClientData.staminaMax.");
			}
			this.maturity = (uint)arg1.ReadInt();
			if ( this.maturity < 0 )
			{
				throw new Exception("Forbidden value (" + this.maturity + ") on element of MountClientData.maturity.");
			}
			this.maturityForAdult = (uint)arg1.ReadInt();
			if ( this.maturityForAdult < 0 )
			{
				throw new Exception("Forbidden value (" + this.maturityForAdult + ") on element of MountClientData.maturityForAdult.");
			}
			this.energy = (uint)arg1.ReadInt();
			if ( this.energy < 0 )
			{
				throw new Exception("Forbidden value (" + this.energy + ") on element of MountClientData.energy.");
			}
			this.energyMax = (uint)arg1.ReadInt();
			if ( this.energyMax < 0 )
			{
				throw new Exception("Forbidden value (" + this.energyMax + ") on element of MountClientData.energyMax.");
			}
			this.serenity = (int)arg1.ReadInt();
			this.aggressivityMax = (int)arg1.ReadInt();
			this.serenityMax = (uint)arg1.ReadInt();
			if ( this.serenityMax < 0 )
			{
				throw new Exception("Forbidden value (" + this.serenityMax + ") on element of MountClientData.serenityMax.");
			}
			this.love = (uint)arg1.ReadInt();
			if ( this.love < 0 )
			{
				throw new Exception("Forbidden value (" + this.love + ") on element of MountClientData.love.");
			}
			this.loveMax = (uint)arg1.ReadInt();
			if ( this.loveMax < 0 )
			{
				throw new Exception("Forbidden value (" + this.loveMax + ") on element of MountClientData.loveMax.");
			}
			this.fecondationTime = (int)arg1.ReadInt();
			this.boostLimiter = (uint)arg1.ReadInt();
			if ( this.boostLimiter < 0 )
			{
				throw new Exception("Forbidden value (" + this.boostLimiter + ") on element of MountClientData.boostLimiter.");
			}
			this.boostMax = (double)arg1.ReadDouble();
			this.reproductionCount = (int)arg1.ReadInt();
			this.reproductionCountMax = (uint)arg1.ReadInt();
			if ( this.reproductionCountMax < 0 )
			{
				throw new Exception("Forbidden value (" + this.reproductionCountMax + ") on element of MountClientData.reproductionCountMax.");
			}
			var loc6 = (ushort)arg1.ReadUShort();
			var loc7 = 0;
			while ( loc7 < loc6 )
			{
				((loc10 = new ObjectEffectInteger()) as ObjectEffectInteger).deserialize(arg1);
				this.effectList.Add((ObjectEffectInteger)loc10);
				++loc7;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_MountClientData(arg1);
		}
		
	}
}
