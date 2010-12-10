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
	
	public class SkillActionDescriptionCraft : SkillActionDescription
	{
		public const uint protocolId = 100;
		public uint maxSlots = 0;
		public uint probability = 0;
		
		public SkillActionDescriptionCraft()
		{
		}
		
		public SkillActionDescriptionCraft(uint arg1, uint arg2, uint arg3)
			: this()
		{
			initSkillActionDescriptionCraft(arg1, arg2, arg3);
		}
		
		public override uint getTypeId()
		{
			return 100;
		}
		
		public SkillActionDescriptionCraft initSkillActionDescriptionCraft(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0)
		{
			base.initSkillActionDescription(arg1);
			this.maxSlots = arg2;
			this.probability = arg3;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.maxSlots = 0;
			this.probability = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_SkillActionDescriptionCraft(arg1);
		}
		
		public void serializeAs_SkillActionDescriptionCraft(BigEndianWriter arg1)
		{
			base.serializeAs_SkillActionDescription(arg1);
			if ( this.maxSlots < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxSlots + ") on element maxSlots.");
			}
			arg1.WriteByte((byte)this.maxSlots);
			if ( this.probability < 0 )
			{
				throw new Exception("Forbidden value (" + this.probability + ") on element probability.");
			}
			arg1.WriteByte((byte)this.probability);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_SkillActionDescriptionCraft(arg1);
		}
		
		public void deserializeAs_SkillActionDescriptionCraft(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.maxSlots = (uint)arg1.ReadByte();
			if ( this.maxSlots < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxSlots + ") on element of SkillActionDescriptionCraft.maxSlots.");
			}
			this.probability = (uint)arg1.ReadByte();
			if ( this.probability < 0 )
			{
				throw new Exception("Forbidden value (" + this.probability + ") on element of SkillActionDescriptionCraft.probability.");
			}
		}
		
	}
}
