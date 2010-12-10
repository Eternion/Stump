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
	
	public class FightDispellableEffectExtendedInformations : Object
	{
		public const uint protocolId = 208;
		public uint actionId = 0;
		public int sourceId = 0;
		public AbstractFightDispellableEffect effect;
		
		public FightDispellableEffectExtendedInformations()
		{
			this.effect = new AbstractFightDispellableEffect();
		}
		
		public FightDispellableEffectExtendedInformations(uint arg1, int arg2, AbstractFightDispellableEffect arg3)
			: this()
		{
			initFightDispellableEffectExtendedInformations(arg1, arg2, arg3);
		}
		
		public virtual uint getTypeId()
		{
			return 208;
		}
		
		public FightDispellableEffectExtendedInformations initFightDispellableEffectExtendedInformations(uint arg1 = 0, int arg2 = 0, AbstractFightDispellableEffect arg3 = null)
		{
			this.actionId = arg1;
			this.sourceId = arg2;
			this.effect = arg3;
			return this;
		}
		
		public virtual void reset()
		{
			this.actionId = 0;
			this.sourceId = 0;
			this.effect = new AbstractFightDispellableEffect();
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FightDispellableEffectExtendedInformations(arg1);
		}
		
		public void serializeAs_FightDispellableEffectExtendedInformations(BigEndianWriter arg1)
		{
			if ( this.actionId < 0 )
			{
				throw new Exception("Forbidden value (" + this.actionId + ") on element actionId.");
			}
			arg1.WriteShort((short)this.actionId);
			arg1.WriteInt((int)this.sourceId);
			arg1.WriteShort((short)this.effect.getTypeId());
			this.effect.serialize(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FightDispellableEffectExtendedInformations(arg1);
		}
		
		public void deserializeAs_FightDispellableEffectExtendedInformations(BigEndianReader arg1)
		{
			this.actionId = (uint)arg1.ReadShort();
			if ( this.actionId < 0 )
			{
				throw new Exception("Forbidden value (" + this.actionId + ") on element of FightDispellableEffectExtendedInformations.actionId.");
			}
			this.sourceId = (int)arg1.ReadInt();
			var loc1 = (ushort)arg1.ReadUShort();
			this.effect = ProtocolTypeManager.GetInstance<AbstractFightDispellableEffect>((uint)loc1);
			this.effect.deserialize(arg1);
		}
		
	}
}
