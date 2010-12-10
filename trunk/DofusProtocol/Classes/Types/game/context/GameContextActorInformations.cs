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
	
	public class GameContextActorInformations : Object
	{
		public const uint protocolId = 150;
		public int contextualId = 0;
		public EntityLook look;
		public EntityDispositionInformations disposition;
		
		public GameContextActorInformations()
		{
			this.look = new EntityLook();
			this.disposition = new EntityDispositionInformations();
		}
		
		public GameContextActorInformations(int arg1, EntityLook arg2, EntityDispositionInformations arg3)
			: this()
		{
			initGameContextActorInformations(arg1, arg2, arg3);
		}
		
		public virtual uint getTypeId()
		{
			return 150;
		}
		
		public GameContextActorInformations initGameContextActorInformations(int arg1 = 0, EntityLook arg2 = null, EntityDispositionInformations arg3 = null)
		{
			this.contextualId = arg1;
			this.look = arg2;
			this.disposition = arg3;
			return this;
		}
		
		public virtual void reset()
		{
			this.contextualId = 0;
			this.look = new EntityLook();
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameContextActorInformations(arg1);
		}
		
		public void serializeAs_GameContextActorInformations(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.contextualId);
			this.look.serializeAs_EntityLook(arg1);
			arg1.WriteShort((short)this.disposition.getTypeId());
			this.disposition.serialize(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameContextActorInformations(arg1);
		}
		
		public void deserializeAs_GameContextActorInformations(BigEndianReader arg1)
		{
			this.contextualId = (int)arg1.ReadInt();
			this.look = new EntityLook();
			this.look.deserialize(arg1);
			var loc1 = (ushort)arg1.ReadUShort();
			this.disposition = ProtocolTypeManager.GetInstance<EntityDispositionInformations>((uint)loc1);
			this.disposition.deserialize(arg1);
		}
		
	}
}
