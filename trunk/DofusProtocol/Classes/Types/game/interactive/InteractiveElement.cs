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
	
	public class InteractiveElement : Object
	{
		public const uint protocolId = 80;
		public uint elementId = 0;
		public int elementTypeId = 0;
		public List<InteractiveElementSkill> enabledSkills;
		public List<InteractiveElementSkill> disabledSkills;
		
		public InteractiveElement()
		{
			this.enabledSkills = new List<InteractiveElementSkill>();
			this.disabledSkills = new List<InteractiveElementSkill>();
		}
		
		public InteractiveElement(uint arg1, int arg2, List<InteractiveElementSkill> arg3, List<InteractiveElementSkill> arg4)
			: this()
		{
			initInteractiveElement(arg1, arg2, arg3, arg4);
		}
		
		public virtual uint getTypeId()
		{
			return 80;
		}
		
		public InteractiveElement initInteractiveElement(uint arg1 = 0, int arg2 = 0, List<InteractiveElementSkill> arg3 = null, List<InteractiveElementSkill> arg4 = null)
		{
			this.elementId = arg1;
			this.elementTypeId = arg2;
			this.enabledSkills = arg3;
			this.disabledSkills = arg4;
			return this;
		}
		
		public virtual void reset()
		{
			this.elementId = 0;
			this.elementTypeId = 0;
			this.enabledSkills = new List<InteractiveElementSkill>();
			this.disabledSkills = new List<InteractiveElementSkill>();
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_InteractiveElement(arg1);
		}
		
		public void serializeAs_InteractiveElement(BigEndianWriter arg1)
		{
			if ( this.elementId < 0 )
			{
				throw new Exception("Forbidden value (" + this.elementId + ") on element elementId.");
			}
			arg1.WriteInt((int)this.elementId);
			arg1.WriteInt((int)this.elementTypeId);
			arg1.WriteShort((short)this.enabledSkills.Count);
			var loc1 = 0;
			while ( loc1 < this.enabledSkills.Count )
			{
				arg1.WriteShort((short)this.enabledSkills[loc1].getTypeId());
				this.enabledSkills[loc1].serialize(arg1);
				++loc1;
			}
			arg1.WriteShort((short)this.disabledSkills.Count);
			var loc2 = 0;
			while ( loc2 < this.disabledSkills.Count )
			{
				arg1.WriteShort((short)this.disabledSkills[loc2].getTypeId());
				this.disabledSkills[loc2].serialize(arg1);
				++loc2;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_InteractiveElement(arg1);
		}
		
		public void deserializeAs_InteractiveElement(BigEndianReader arg1)
		{
			var loc5 = 0;
			object loc6 = null;
			var loc7 = 0;
			object loc8 = null;
			this.elementId = (uint)arg1.ReadInt();
			if ( this.elementId < 0 )
			{
				throw new Exception("Forbidden value (" + this.elementId + ") on element of InteractiveElement.elementId.");
			}
			this.elementTypeId = (int)arg1.ReadInt();
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc5 = (ushort)arg1.ReadUShort();
				(( loc6 = ProtocolTypeManager.GetInstance<InteractiveElementSkill>((uint)loc5)) as InteractiveElementSkill).deserialize(arg1);
				this.enabledSkills.Add((InteractiveElementSkill)loc6);
				++loc2;
			}
			var loc3 = (ushort)arg1.ReadUShort();
			var loc4 = 0;
			while ( loc4 < loc3 )
			{
				loc7 = (ushort)arg1.ReadUShort();
				(( loc8 = ProtocolTypeManager.GetInstance<InteractiveElementSkill>((uint)loc7)) as InteractiveElementSkill).deserialize(arg1);
				this.disabledSkills.Add((InteractiveElementSkill)loc8);
				++loc4;
			}
		}
		
	}
}
