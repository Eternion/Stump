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
	
	public class SkillActionDescriptionCollect : SkillActionDescriptionTimed
	{
		public const uint protocolId = 99;
		public uint min = 0;
		public uint max = 0;
		
		public SkillActionDescriptionCollect()
		{
		}
		
		public SkillActionDescriptionCollect(uint arg1, uint arg2, uint arg3, uint arg4)
			: this()
		{
			initSkillActionDescriptionCollect(arg1, arg2, arg3, arg4);
		}
		
		public override uint getTypeId()
		{
			return 99;
		}
		
		public SkillActionDescriptionCollect initSkillActionDescriptionCollect(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0, uint arg4 = 0)
		{
			base.initSkillActionDescriptionTimed(arg1, arg2);
			this.min = arg3;
			this.max = arg4;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.min = 0;
			this.max = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_SkillActionDescriptionCollect(arg1);
		}
		
		public void serializeAs_SkillActionDescriptionCollect(BigEndianWriter arg1)
		{
			base.serializeAs_SkillActionDescriptionTimed(arg1);
			if ( this.min < 0 )
			{
				throw new Exception("Forbidden value (" + this.min + ") on element min.");
			}
			arg1.WriteShort((short)this.min);
			if ( this.max < 0 )
			{
				throw new Exception("Forbidden value (" + this.max + ") on element max.");
			}
			arg1.WriteShort((short)this.max);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_SkillActionDescriptionCollect(arg1);
		}
		
		public void deserializeAs_SkillActionDescriptionCollect(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.min = (uint)arg1.ReadShort();
			if ( this.min < 0 )
			{
				throw new Exception("Forbidden value (" + this.min + ") on element of SkillActionDescriptionCollect.min.");
			}
			this.max = (uint)arg1.ReadShort();
			if ( this.max < 0 )
			{
				throw new Exception("Forbidden value (" + this.max + ") on element of SkillActionDescriptionCollect.max.");
			}
		}
		
	}
}
