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
	
	public class GameFightMinimalStats : Object
	{
		public const uint protocolId = 31;
		public uint lifePoints = 0;
		public uint maxLifePoints = 0;
		public int actionPoints = 0;
		public int movementPoints = 0;
		public int summoner = 0;
		public int neutralElementResistPercent = 0;
		public int earthElementResistPercent = 0;
		public int waterElementResistPercent = 0;
		public int airElementResistPercent = 0;
		public int fireElementResistPercent = 0;
		public uint dodgePALostProbability = 0;
		public uint dodgePMLostProbability = 0;
		public uint tackleBlock = 0;
		public int invisibilityState = 0;
		
		public GameFightMinimalStats()
		{
		}
		
		public GameFightMinimalStats(uint arg1, uint arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8, int arg9, int arg10, uint arg11, uint arg12, uint arg13, int arg14)
			: this()
		{
			initGameFightMinimalStats(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
		}
		
		public virtual uint getTypeId()
		{
			return 31;
		}
		
		public GameFightMinimalStats initGameFightMinimalStats(uint arg1 = 0, uint arg2 = 0, int arg3 = 0, int arg4 = 0, int arg5 = 0, int arg6 = 0, int arg7 = 0, int arg8 = 0, int arg9 = 0, int arg10 = 0, uint arg11 = 0, uint arg12 = 0, uint arg13 = 0, int arg14 = 0)
		{
			this.lifePoints = arg1;
			this.maxLifePoints = arg2;
			this.actionPoints = arg3;
			this.movementPoints = arg4;
			this.summoner = arg5;
			this.neutralElementResistPercent = arg6;
			this.earthElementResistPercent = arg7;
			this.waterElementResistPercent = arg8;
			this.airElementResistPercent = arg9;
			this.fireElementResistPercent = arg10;
			this.dodgePALostProbability = arg11;
			this.dodgePMLostProbability = arg12;
			this.tackleBlock = arg13;
			this.invisibilityState = arg14;
			return this;
		}
		
		public virtual void reset()
		{
			this.lifePoints = 0;
			this.maxLifePoints = 0;
			this.actionPoints = 0;
			this.movementPoints = 0;
			this.summoner = 0;
			this.neutralElementResistPercent = 0;
			this.earthElementResistPercent = 0;
			this.waterElementResistPercent = 0;
			this.airElementResistPercent = 0;
			this.fireElementResistPercent = 0;
			this.dodgePALostProbability = 0;
			this.dodgePMLostProbability = 0;
			this.tackleBlock = 0;
			this.invisibilityState = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameFightMinimalStats(arg1);
		}
		
		public void serializeAs_GameFightMinimalStats(BigEndianWriter arg1)
		{
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
			arg1.WriteShort((short)this.actionPoints);
			arg1.WriteShort((short)this.movementPoints);
			arg1.WriteInt((int)this.summoner);
			arg1.WriteShort((short)this.neutralElementResistPercent);
			arg1.WriteShort((short)this.earthElementResistPercent);
			arg1.WriteShort((short)this.waterElementResistPercent);
			arg1.WriteShort((short)this.airElementResistPercent);
			arg1.WriteShort((short)this.fireElementResistPercent);
			if ( this.dodgePALostProbability < 0 )
			{
				throw new Exception("Forbidden value (" + this.dodgePALostProbability + ") on element dodgePALostProbability.");
			}
			arg1.WriteShort((short)this.dodgePALostProbability);
			if ( this.dodgePMLostProbability < 0 )
			{
				throw new Exception("Forbidden value (" + this.dodgePMLostProbability + ") on element dodgePMLostProbability.");
			}
			arg1.WriteShort((short)this.dodgePMLostProbability);
			if ( this.tackleBlock < 0 )
			{
				throw new Exception("Forbidden value (" + this.tackleBlock + ") on element tackleBlock.");
			}
			arg1.WriteShort((short)this.tackleBlock);
			arg1.WriteByte((byte)this.invisibilityState);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightMinimalStats(arg1);
		}
		
		public void deserializeAs_GameFightMinimalStats(BigEndianReader arg1)
		{
			this.lifePoints = (uint)arg1.ReadInt();
			if ( this.lifePoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.lifePoints + ") on element of GameFightMinimalStats.lifePoints.");
			}
			this.maxLifePoints = (uint)arg1.ReadInt();
			if ( this.maxLifePoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxLifePoints + ") on element of GameFightMinimalStats.maxLifePoints.");
			}
			this.actionPoints = (int)arg1.ReadShort();
			this.movementPoints = (int)arg1.ReadShort();
			this.summoner = (int)arg1.ReadInt();
			this.neutralElementResistPercent = (int)arg1.ReadShort();
			this.earthElementResistPercent = (int)arg1.ReadShort();
			this.waterElementResistPercent = (int)arg1.ReadShort();
			this.airElementResistPercent = (int)arg1.ReadShort();
			this.fireElementResistPercent = (int)arg1.ReadShort();
			this.dodgePALostProbability = (uint)arg1.ReadShort();
			if ( this.dodgePALostProbability < 0 )
			{
				throw new Exception("Forbidden value (" + this.dodgePALostProbability + ") on element of GameFightMinimalStats.dodgePALostProbability.");
			}
			this.dodgePMLostProbability = (uint)arg1.ReadShort();
			if ( this.dodgePMLostProbability < 0 )
			{
				throw new Exception("Forbidden value (" + this.dodgePMLostProbability + ") on element of GameFightMinimalStats.dodgePMLostProbability.");
			}
			this.tackleBlock = (uint)arg1.ReadShort();
			if ( this.tackleBlock < 0 )
			{
				throw new Exception("Forbidden value (" + this.tackleBlock + ") on element of GameFightMinimalStats.tackleBlock.");
			}
			this.invisibilityState = (int)arg1.ReadByte();
		}
		
	}
}
