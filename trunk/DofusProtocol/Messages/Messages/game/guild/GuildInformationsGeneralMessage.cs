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
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildInformationsGeneralMessage : Message
	{
		public const uint protocolId = 5557;
		internal Boolean _isInitialized = false;
		public Boolean enabled = false;
		public uint level = 0;
		public double expLevelFloor = 0;
		public double experience = 0;
		public double expNextLevelFloor = 0;
		
		public GuildInformationsGeneralMessage()
		{
		}
		
		public GuildInformationsGeneralMessage(Boolean arg1, uint arg2, double arg3, double arg4, double arg5)
			: this()
		{
			initGuildInformationsGeneralMessage(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getMessageId()
		{
			return 5557;
		}
		
		public GuildInformationsGeneralMessage initGuildInformationsGeneralMessage(Boolean arg1 = false, uint arg2 = 0, double arg3 = 0, double arg4 = 0, double arg5 = 0)
		{
			this.enabled = arg1;
			this.level = arg2;
			this.expLevelFloor = arg3;
			this.experience = arg4;
			this.expNextLevelFloor = arg5;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.enabled = false;
			this.level = 0;
			this.expLevelFloor = 0;
			this.experience = 0;
			this.expNextLevelFloor = 0;
			this._isInitialized = false;
		}
		
		public override void pack(BigEndianWriter arg1)
		{
			this.serialize(arg1);
			WritePacket(arg1, this.getMessageId());
		}
		
		public override void unpack(BigEndianReader arg1, uint arg2)
		{
			this.deserialize(arg1);
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GuildInformationsGeneralMessage(arg1);
		}
		
		public void serializeAs_GuildInformationsGeneralMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.enabled);
			if ( this.level < 0 || this.level > 255 )
			{
				throw new Exception("Forbidden value (" + this.level + ") on element level.");
			}
			arg1.WriteByte((byte)this.level);
			if ( this.expLevelFloor < 0 )
			{
				throw new Exception("Forbidden value (" + this.expLevelFloor + ") on element expLevelFloor.");
			}
			arg1.WriteDouble(this.expLevelFloor);
			if ( this.experience < 0 )
			{
				throw new Exception("Forbidden value (" + this.experience + ") on element experience.");
			}
			arg1.WriteDouble(this.experience);
			if ( this.expNextLevelFloor < 0 )
			{
				throw new Exception("Forbidden value (" + this.expNextLevelFloor + ") on element expNextLevelFloor.");
			}
			arg1.WriteDouble(this.expNextLevelFloor);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildInformationsGeneralMessage(arg1);
		}
		
		public void deserializeAs_GuildInformationsGeneralMessage(BigEndianReader arg1)
		{
			this.enabled = (Boolean)arg1.ReadBoolean();
			this.level = (uint)arg1.ReadByte();
			if ( this.level < 0 || this.level > 255 )
			{
				throw new Exception("Forbidden value (" + this.level + ") on element of GuildInformationsGeneralMessage.level.");
			}
			this.expLevelFloor = (double)arg1.ReadDouble();
			if ( this.expLevelFloor < 0 )
			{
				throw new Exception("Forbidden value (" + this.expLevelFloor + ") on element of GuildInformationsGeneralMessage.expLevelFloor.");
			}
			this.experience = (double)arg1.ReadDouble();
			if ( this.experience < 0 )
			{
				throw new Exception("Forbidden value (" + this.experience + ") on element of GuildInformationsGeneralMessage.experience.");
			}
			this.expNextLevelFloor = (double)arg1.ReadDouble();
			if ( this.expNextLevelFloor < 0 )
			{
				throw new Exception("Forbidden value (" + this.expNextLevelFloor + ") on element of GuildInformationsGeneralMessage.expNextLevelFloor.");
			}
		}
		
	}
}
