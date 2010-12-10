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
	
	public class ObjectEffectDuration : ObjectEffect
	{
		public const uint protocolId = 75;
		public uint days = 0;
		public uint hours = 0;
		public uint minutes = 0;
		
		public ObjectEffectDuration()
		{
		}
		
		public ObjectEffectDuration(uint arg1, uint arg2, uint arg3, uint arg4)
			: this()
		{
			initObjectEffectDuration(arg1, arg2, arg3, arg4);
		}
		
		public override uint getTypeId()
		{
			return 75;
		}
		
		public ObjectEffectDuration initObjectEffectDuration(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0, uint arg4 = 0)
		{
			base.initObjectEffect(arg1);
			this.days = arg2;
			this.hours = arg3;
			this.minutes = arg4;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.days = 0;
			this.hours = 0;
			this.minutes = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ObjectEffectDuration(arg1);
		}
		
		public void serializeAs_ObjectEffectDuration(BigEndianWriter arg1)
		{
			base.serializeAs_ObjectEffect(arg1);
			if ( this.days < 0 )
			{
				throw new Exception("Forbidden value (" + this.days + ") on element days.");
			}
			arg1.WriteShort((short)this.days);
			if ( this.hours < 0 )
			{
				throw new Exception("Forbidden value (" + this.hours + ") on element hours.");
			}
			arg1.WriteShort((short)this.hours);
			if ( this.minutes < 0 )
			{
				throw new Exception("Forbidden value (" + this.minutes + ") on element minutes.");
			}
			arg1.WriteShort((short)this.minutes);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectEffectDuration(arg1);
		}
		
		public void deserializeAs_ObjectEffectDuration(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.days = (uint)arg1.ReadShort();
			if ( this.days < 0 )
			{
				throw new Exception("Forbidden value (" + this.days + ") on element of ObjectEffectDuration.days.");
			}
			this.hours = (uint)arg1.ReadShort();
			if ( this.hours < 0 )
			{
				throw new Exception("Forbidden value (" + this.hours + ") on element of ObjectEffectDuration.hours.");
			}
			this.minutes = (uint)arg1.ReadShort();
			if ( this.minutes < 0 )
			{
				throw new Exception("Forbidden value (" + this.minutes + ") on element of ObjectEffectDuration.minutes.");
			}
		}
		
	}
}
