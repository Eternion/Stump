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
	
	public class LockableShowCodeDialogMessage : Message
	{
		public const uint protocolId = 5740;
		internal Boolean _isInitialized = false;
		public Boolean changeOrUse = false;
		public uint codeSize = 0;
		
		public LockableShowCodeDialogMessage()
		{
		}
		
		public LockableShowCodeDialogMessage(Boolean arg1, uint arg2)
			: this()
		{
			initLockableShowCodeDialogMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5740;
		}
		
		public LockableShowCodeDialogMessage initLockableShowCodeDialogMessage(Boolean arg1 = false, uint arg2 = 0)
		{
			this.changeOrUse = arg1;
			this.codeSize = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.changeOrUse = false;
			this.codeSize = 0;
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
			this.serializeAs_LockableShowCodeDialogMessage(arg1);
		}
		
		public void serializeAs_LockableShowCodeDialogMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.changeOrUse);
			if ( this.codeSize < 0 )
			{
				throw new Exception("Forbidden value (" + this.codeSize + ") on element codeSize.");
			}
			arg1.WriteByte((byte)this.codeSize);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_LockableShowCodeDialogMessage(arg1);
		}
		
		public void deserializeAs_LockableShowCodeDialogMessage(BigEndianReader arg1)
		{
			this.changeOrUse = (Boolean)arg1.ReadBoolean();
			this.codeSize = (uint)arg1.ReadByte();
			if ( this.codeSize < 0 )
			{
				throw new Exception("Forbidden value (" + this.codeSize + ") on element of LockableShowCodeDialogMessage.codeSize.");
			}
		}
		
	}
}
