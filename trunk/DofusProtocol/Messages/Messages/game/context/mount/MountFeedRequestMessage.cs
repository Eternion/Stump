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
	
	public class MountFeedRequestMessage : Message
	{
		public const uint protocolId = 6189;
		internal Boolean _isInitialized = false;
		public double mountUid = 0;
		public int mountLocation = 0;
		public uint mountFoodUid = 0;
		public uint quantity = 0;
		
		public MountFeedRequestMessage()
		{
		}
		
		public MountFeedRequestMessage(double arg1, int arg2, uint arg3, uint arg4)
			: this()
		{
			initMountFeedRequestMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 6189;
		}
		
		public MountFeedRequestMessage initMountFeedRequestMessage(double arg1 = 0, int arg2 = 0, uint arg3 = 0, uint arg4 = 0)
		{
			this.mountUid = arg1;
			this.mountLocation = arg2;
			this.mountFoodUid = arg3;
			this.quantity = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.mountUid = 0;
			this.mountLocation = 0;
			this.mountFoodUid = 0;
			this.quantity = 0;
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
			this.serializeAs_MountFeedRequestMessage(arg1);
		}
		
		public void serializeAs_MountFeedRequestMessage(BigEndianWriter arg1)
		{
			if ( this.mountUid < 0 )
			{
				throw new Exception("Forbidden value (" + this.mountUid + ") on element mountUid.");
			}
			arg1.WriteDouble(this.mountUid);
			arg1.WriteByte((byte)this.mountLocation);
			if ( this.mountFoodUid < 0 )
			{
				throw new Exception("Forbidden value (" + this.mountFoodUid + ") on element mountFoodUid.");
			}
			arg1.WriteInt((int)this.mountFoodUid);
			if ( this.quantity < 0 )
			{
				throw new Exception("Forbidden value (" + this.quantity + ") on element quantity.");
			}
			arg1.WriteInt((int)this.quantity);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_MountFeedRequestMessage(arg1);
		}
		
		public void deserializeAs_MountFeedRequestMessage(BigEndianReader arg1)
		{
			this.mountUid = (double)arg1.ReadDouble();
			if ( this.mountUid < 0 )
			{
				throw new Exception("Forbidden value (" + this.mountUid + ") on element of MountFeedRequestMessage.mountUid.");
			}
			this.mountLocation = (int)arg1.ReadByte();
			this.mountFoodUid = (uint)arg1.ReadInt();
			if ( this.mountFoodUid < 0 )
			{
				throw new Exception("Forbidden value (" + this.mountFoodUid + ") on element of MountFeedRequestMessage.mountFoodUid.");
			}
			this.quantity = (uint)arg1.ReadInt();
			if ( this.quantity < 0 )
			{
				throw new Exception("Forbidden value (" + this.quantity + ") on element of MountFeedRequestMessage.quantity.");
			}
		}
		
	}
}
