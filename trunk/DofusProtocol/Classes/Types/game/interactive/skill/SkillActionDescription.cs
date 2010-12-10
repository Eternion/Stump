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
	
	public class SkillActionDescription : Object
	{
		public const uint protocolId = 102;
		public uint skillId = 0;
		
		public SkillActionDescription()
		{
		}
		
		public SkillActionDescription(uint arg1)
			: this()
		{
			initSkillActionDescription(arg1);
		}
		
		public virtual uint getTypeId()
		{
			return 102;
		}
		
		public SkillActionDescription initSkillActionDescription(uint arg1 = 0)
		{
			this.skillId = arg1;
			return this;
		}
		
		public virtual void reset()
		{
			this.skillId = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_SkillActionDescription(arg1);
		}
		
		public void serializeAs_SkillActionDescription(BigEndianWriter arg1)
		{
			if ( this.skillId < 0 )
			{
				throw new Exception("Forbidden value (" + this.skillId + ") on element skillId.");
			}
			arg1.WriteShort((short)this.skillId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_SkillActionDescription(arg1);
		}
		
		public void deserializeAs_SkillActionDescription(BigEndianReader arg1)
		{
			this.skillId = (uint)arg1.ReadShort();
			if ( this.skillId < 0 )
			{
				throw new Exception("Forbidden value (" + this.skillId + ") on element of SkillActionDescription.skillId.");
			}
		}
		
	}
}
