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
	
	public class AbstractFightDispellableEffect : Object
	{
		public const uint protocolId = 206;
		public uint uid = 0;
		public int targetId = 0;
		public int turnDuration = 0;
		public uint dispelable = 1;
		public uint spellId = 0;
		
		public AbstractFightDispellableEffect()
		{
		}
		
		public AbstractFightDispellableEffect(uint arg1, int arg2, int arg3, uint arg4, uint arg5)
			: this()
		{
			initAbstractFightDispellableEffect(arg1, arg2, arg3, arg4, arg5);
		}
		
		public virtual uint getTypeId()
		{
			return 206;
		}
		
		public AbstractFightDispellableEffect initAbstractFightDispellableEffect(uint arg1 = 0, int arg2 = 0, int arg3 = 0, uint arg4 = 1, uint arg5 = 0)
		{
			this.uid = arg1;
			this.targetId = arg2;
			this.turnDuration = arg3;
			this.dispelable = arg4;
			this.spellId = arg5;
			return this;
		}
		
		public virtual void reset()
		{
			this.uid = 0;
			this.targetId = 0;
			this.turnDuration = 0;
			this.dispelable = 1;
			this.spellId = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_AbstractFightDispellableEffect(arg1);
		}
		
		public void serializeAs_AbstractFightDispellableEffect(BigEndianWriter arg1)
		{
			if ( this.uid < 0 )
			{
				throw new Exception("Forbidden value (" + this.uid + ") on element uid.");
			}
			arg1.WriteInt((int)this.uid);
			arg1.WriteInt((int)this.targetId);
			arg1.WriteShort((short)this.turnDuration);
			arg1.WriteByte((byte)this.dispelable);
			if ( this.spellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.spellId + ") on element spellId.");
			}
			arg1.WriteShort((short)this.spellId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AbstractFightDispellableEffect(arg1);
		}
		
		public void deserializeAs_AbstractFightDispellableEffect(BigEndianReader arg1)
		{
			this.uid = (uint)arg1.ReadInt();
			if ( this.uid < 0 )
			{
				throw new Exception("Forbidden value (" + this.uid + ") on element of AbstractFightDispellableEffect.uid.");
			}
			this.targetId = (int)arg1.ReadInt();
			this.turnDuration = (int)arg1.ReadShort();
			this.dispelable = (uint)arg1.ReadByte();
			if ( this.dispelable < 0 )
			{
				throw new Exception("Forbidden value (" + this.dispelable + ") on element of AbstractFightDispellableEffect.dispelable.");
			}
			this.spellId = (uint)arg1.ReadShort();
			if ( this.spellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.spellId + ") on element of AbstractFightDispellableEffect.spellId.");
			}
		}
		
	}
}
