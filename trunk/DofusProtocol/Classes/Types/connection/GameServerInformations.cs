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
	
	public class GameServerInformations : Object
	{
		public const uint protocolId = 25;
		public uint id = 0;
		public uint status = 1;
		public uint completion = 0;
		public Boolean isSelectable = false;
		public uint charactersCount = 0;
		
		public GameServerInformations()
		{
		}
		
		public GameServerInformations(uint arg1, uint arg2, uint arg3, Boolean arg4, uint arg5)
			: this()
		{
			initGameServerInformations(arg1, arg2, arg3, arg4, arg5);
		}
		
		public virtual uint getTypeId()
		{
			return 25;
		}
		
		public GameServerInformations initGameServerInformations(uint arg1 = 0, uint arg2 = 1, uint arg3 = 0, Boolean arg4 = false, uint arg5 = 0)
		{
			this.id = arg1;
			this.status = arg2;
			this.completion = arg3;
			this.isSelectable = arg4;
			this.charactersCount = arg5;
			return this;
		}
		
		public virtual void reset()
		{
			this.id = 0;
			this.status = 1;
			this.completion = 0;
			this.isSelectable = false;
			this.charactersCount = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameServerInformations(arg1);
		}
		
		public void serializeAs_GameServerInformations(BigEndianWriter arg1)
		{
			if ( this.id < 0 || this.id > 65535 )
			{
				throw new Exception("Forbidden value (" + this.id + ") on element id.");
			}
			arg1.WriteShort((short)this.id);
			arg1.WriteByte((byte)this.status);
			arg1.WriteByte((byte)this.completion);
			arg1.WriteBoolean(this.isSelectable);
			if ( this.charactersCount < 0 )
			{
				throw new Exception("Forbidden value (" + this.charactersCount + ") on element charactersCount.");
			}
			arg1.WriteByte((byte)this.charactersCount);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameServerInformations(arg1);
		}
		
		public void deserializeAs_GameServerInformations(BigEndianReader arg1)
		{
			this.id = (uint)arg1.ReadUShort();
			if ( this.id < 0 || this.id > 65535 )
			{
				throw new Exception("Forbidden value (" + this.id + ") on element of GameServerInformations.id.");
			}
			this.status = (uint)arg1.ReadByte();
			if ( this.status < 0 )
			{
				throw new Exception("Forbidden value (" + this.status + ") on element of GameServerInformations.status.");
			}
			this.completion = (uint)arg1.ReadByte();
			if ( this.completion < 0 )
			{
				throw new Exception("Forbidden value (" + this.completion + ") on element of GameServerInformations.completion.");
			}
			this.isSelectable = (Boolean)arg1.ReadBoolean();
			this.charactersCount = (uint)arg1.ReadByte();
			if ( this.charactersCount < 0 )
			{
				throw new Exception("Forbidden value (" + this.charactersCount + ") on element of GameServerInformations.charactersCount.");
			}
		}
		
	}
}
