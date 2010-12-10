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
	
	public class AchievementStartedValue : Achievement
	{
		public const uint protocolId = 361;
		public uint value = 0;
		public uint maxValue = 0;
		
		public AchievementStartedValue()
		{
		}
		
		public AchievementStartedValue(uint arg1, uint arg2, uint arg3)
			: this()
		{
			initAchievementStartedValue(arg1, arg2, arg3);
		}
		
		public override uint getTypeId()
		{
			return 361;
		}
		
		public AchievementStartedValue initAchievementStartedValue(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0)
		{
			base.initAchievement(arg1);
			this.value = arg2;
			this.maxValue = arg3;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.value = 0;
			this.maxValue = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_AchievementStartedValue(arg1);
		}
		
		public void serializeAs_AchievementStartedValue(BigEndianWriter arg1)
		{
			base.serializeAs_Achievement(arg1);
			if ( this.value < 0 )
			{
				throw new Exception("Forbidden value (" + this.value + ") on element value.");
			}
			arg1.WriteShort((short)this.value);
			if ( this.maxValue < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxValue + ") on element maxValue.");
			}
			arg1.WriteShort((short)this.maxValue);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AchievementStartedValue(arg1);
		}
		
		public void deserializeAs_AchievementStartedValue(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.value = (uint)arg1.ReadShort();
			if ( this.value < 0 )
			{
				throw new Exception("Forbidden value (" + this.value + ") on element of AchievementStartedValue.value.");
			}
			this.maxValue = (uint)arg1.ReadShort();
			if ( this.maxValue < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxValue + ") on element of AchievementStartedValue.maxValue.");
			}
		}
		
	}
}
