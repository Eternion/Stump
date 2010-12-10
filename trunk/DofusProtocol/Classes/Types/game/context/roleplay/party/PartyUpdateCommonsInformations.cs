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
	
	public class PartyUpdateCommonsInformations : Object
	{
		public const uint protocolId = 213;
		public uint lifePoints = 0;
		public uint maxLifePoints = 0;
		public uint prospecting = 0;
		public uint regenRate = 0;
		
		public PartyUpdateCommonsInformations()
		{
		}
		
		public PartyUpdateCommonsInformations(uint arg1, uint arg2, uint arg3, uint arg4)
			: this()
		{
			initPartyUpdateCommonsInformations(arg1, arg2, arg3, arg4);
		}
		
		public virtual uint getTypeId()
		{
			return 213;
		}
		
		public PartyUpdateCommonsInformations initPartyUpdateCommonsInformations(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0, uint arg4 = 0)
		{
			this.lifePoints = arg1;
			this.maxLifePoints = arg2;
			this.prospecting = arg3;
			this.regenRate = arg4;
			return this;
		}
		
		public virtual void reset()
		{
			this.lifePoints = 0;
			this.maxLifePoints = 0;
			this.prospecting = 0;
			this.regenRate = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_PartyUpdateCommonsInformations(arg1);
		}
		
		public void serializeAs_PartyUpdateCommonsInformations(BigEndianWriter arg1)
		{
			if ( this.lifePoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.lifePoints + ") on element lifePoints.");
			}
			arg1.WriteInt((int)this.lifePoints);
			if ( this.maxLifePoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxLifePoints + ") on element maxLifePoints.");
			}
			arg1.WriteInt((int)this.maxLifePoints);
			if ( this.prospecting < 0 )
			{
				throw new Exception("Forbidden value (" + this.prospecting + ") on element prospecting.");
			}
			arg1.WriteShort((short)this.prospecting);
			if ( this.regenRate < 0 || this.regenRate > 255 )
			{
				throw new Exception("Forbidden value (" + this.regenRate + ") on element regenRate.");
			}
			arg1.WriteByte((byte)this.regenRate);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PartyUpdateCommonsInformations(arg1);
		}
		
		public void deserializeAs_PartyUpdateCommonsInformations(BigEndianReader arg1)
		{
			this.lifePoints = (uint)arg1.ReadInt();
			if ( this.lifePoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.lifePoints + ") on element of PartyUpdateCommonsInformations.lifePoints.");
			}
			this.maxLifePoints = (uint)arg1.ReadInt();
			if ( this.maxLifePoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxLifePoints + ") on element of PartyUpdateCommonsInformations.maxLifePoints.");
			}
			this.prospecting = (uint)arg1.ReadShort();
			if ( this.prospecting < 0 )
			{
				throw new Exception("Forbidden value (" + this.prospecting + ") on element of PartyUpdateCommonsInformations.prospecting.");
			}
			this.regenRate = (uint)arg1.ReadByte();
			if ( this.regenRate < 0 || this.regenRate > 255 )
			{
				throw new Exception("Forbidden value (" + this.regenRate + ") on element of PartyUpdateCommonsInformations.regenRate.");
			}
		}
		
	}
}
