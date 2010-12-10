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
	
	public class AchievementStartedPercent : Achievement
	{
		public const uint protocolId = 362;
		public uint completionPercent = 0;
		
		public AchievementStartedPercent()
		{
		}
		
		public AchievementStartedPercent(uint arg1, uint arg2)
			: this()
		{
			initAchievementStartedPercent(arg1, arg2);
		}
		
		public override uint getTypeId()
		{
			return 362;
		}
		
		public AchievementStartedPercent initAchievementStartedPercent(uint arg1 = 0, uint arg2 = 0)
		{
			base.initAchievement(arg1);
			this.completionPercent = arg2;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.completionPercent = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_AchievementStartedPercent(arg1);
		}
		
		public void serializeAs_AchievementStartedPercent(BigEndianWriter arg1)
		{
			base.serializeAs_Achievement(arg1);
			if ( this.completionPercent < 0 )
			{
				throw new Exception("Forbidden value (" + this.completionPercent + ") on element completionPercent.");
			}
			arg1.WriteByte((byte)this.completionPercent);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AchievementStartedPercent(arg1);
		}
		
		public void deserializeAs_AchievementStartedPercent(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.completionPercent = (uint)arg1.ReadByte();
			if ( this.completionPercent < 0 )
			{
				throw new Exception("Forbidden value (" + this.completionPercent + ") on element of AchievementStartedPercent.completionPercent.");
			}
		}
		
	}
}
