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
	
	public class MountEquipedErrorMessage : Message
	{
		public const uint protocolId = 5963;
		internal Boolean _isInitialized = false;
		public uint errorType = 0;
		
		public MountEquipedErrorMessage()
		{
		}
		
		public MountEquipedErrorMessage(uint arg1)
			: this()
		{
			initMountEquipedErrorMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5963;
		}
		
		public MountEquipedErrorMessage initMountEquipedErrorMessage(uint arg1 = 0)
		{
			this.errorType = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.errorType = 0;
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
			this.serializeAs_MountEquipedErrorMessage(arg1);
		}
		
		public void serializeAs_MountEquipedErrorMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.errorType);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_MountEquipedErrorMessage(arg1);
		}
		
		public void deserializeAs_MountEquipedErrorMessage(BigEndianReader arg1)
		{
			this.errorType = (uint)arg1.ReadByte();
			if ( this.errorType < 0 )
			{
				throw new Exception("Forbidden value (" + this.errorType + ") on element of MountEquipedErrorMessage.errorType.");
			}
		}
		
	}
}
