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
	
	public class StartupActionFinishedMessage : Message
	{
		public const uint protocolId = 1304;
		internal Boolean _isInitialized = false;
		public Boolean success = false;
		public uint actionId = 0;
		public Boolean automaticAction = false;
		
		public StartupActionFinishedMessage()
		{
		}
		
		public StartupActionFinishedMessage(Boolean arg1, uint arg2, Boolean arg3)
			: this()
		{
			initStartupActionFinishedMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 1304;
		}
		
		public StartupActionFinishedMessage initStartupActionFinishedMessage(Boolean arg1 = false, uint arg2 = 0, Boolean arg3 = false)
		{
			this.success = arg1;
			this.actionId = arg2;
			this.automaticAction = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.success = false;
			this.actionId = 0;
			this.automaticAction = false;
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
			this.serializeAs_StartupActionFinishedMessage(arg1);
		}
		
		public void serializeAs_StartupActionFinishedMessage(BigEndianWriter arg1)
		{
			var loc1 = 0;
			BooleanByteWrapper.SetFlag(loc1, 0, this.success);
			BooleanByteWrapper.SetFlag(loc1, 1, this.automaticAction);
			arg1.WriteByte((byte)loc1);
			if ( this.actionId < 0 )
			{
				throw new Exception("Forbidden value (" + this.actionId + ") on element actionId.");
			}
			arg1.WriteInt((int)this.actionId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_StartupActionFinishedMessage(arg1);
		}
		
		public void deserializeAs_StartupActionFinishedMessage(BigEndianReader arg1)
		{
			var loc1 = arg1.ReadByte();
			this.success = (Boolean)BooleanByteWrapper.GetFlag(loc1, 0);
			this.automaticAction = (Boolean)BooleanByteWrapper.GetFlag(loc1, 1);
			this.actionId = (uint)arg1.ReadInt();
			if ( this.actionId < 0 )
			{
				throw new Exception("Forbidden value (" + this.actionId + ") on element of StartupActionFinishedMessage.actionId.");
			}
		}
		
	}
}
