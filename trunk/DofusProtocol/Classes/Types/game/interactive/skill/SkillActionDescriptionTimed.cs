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
	
	public class SkillActionDescriptionTimed : SkillActionDescription
	{
		public const uint protocolId = 103;
		public uint time = 0;
		
		public SkillActionDescriptionTimed()
		{
		}
		
		public SkillActionDescriptionTimed(uint arg1, uint arg2)
			: this()
		{
			initSkillActionDescriptionTimed(arg1, arg2);
		}
		
		public override uint getTypeId()
		{
			return 103;
		}
		
		public SkillActionDescriptionTimed initSkillActionDescriptionTimed(uint arg1 = 0, uint arg2 = 0)
		{
			base.initSkillActionDescription(arg1);
			this.time = arg2;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.time = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_SkillActionDescriptionTimed(arg1);
		}
		
		public void serializeAs_SkillActionDescriptionTimed(BigEndianWriter arg1)
		{
			base.serializeAs_SkillActionDescription(arg1);
			if ( this.time < 0 || this.time > 255 )
			{
				throw new Exception("Forbidden value (" + this.time + ") on element time.");
			}
			arg1.WriteByte((byte)this.time);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_SkillActionDescriptionTimed(arg1);
		}
		
		public void deserializeAs_SkillActionDescriptionTimed(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.time = (uint)arg1.ReadByte();
			if ( this.time < 0 || this.time > 255 )
			{
				throw new Exception("Forbidden value (" + this.time + ") on element of SkillActionDescriptionTimed.time.");
			}
		}
		
	}
}
