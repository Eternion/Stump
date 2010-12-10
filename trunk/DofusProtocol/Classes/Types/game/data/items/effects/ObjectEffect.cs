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
	
	public class ObjectEffect : Object
	{
		public const uint protocolId = 76;
		public uint actionId = 0;
		
		public ObjectEffect()
		{
		}
		
		public ObjectEffect(uint arg1)
			: this()
		{
			initObjectEffect(arg1);
		}
		
		public virtual uint getTypeId()
		{
			return 76;
		}
		
		public ObjectEffect initObjectEffect(uint arg1 = 0)
		{
			this.actionId = arg1;
			return this;
		}
		
		public virtual void reset()
		{
			this.actionId = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ObjectEffect(arg1);
		}
		
		public void serializeAs_ObjectEffect(BigEndianWriter arg1)
		{
			if ( this.actionId < 0 )
			{
				throw new Exception("Forbidden value (" + this.actionId + ") on element actionId.");
			}
			arg1.WriteShort((short)this.actionId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectEffect(arg1);
		}
		
		public void deserializeAs_ObjectEffect(BigEndianReader arg1)
		{
			this.actionId = (uint)arg1.ReadShort();
			if ( this.actionId < 0 )
			{
				throw new Exception("Forbidden value (" + this.actionId + ") on element of ObjectEffect.actionId.");
			}
		}
		
	}
}
