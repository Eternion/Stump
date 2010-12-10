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
	
	public class ProtocolRequired : Message
	{
		public const uint protocolId = 1;
		internal Boolean _isInitialized = false;
		public uint requiredVersion = 0;
		public uint currentVersion = 0;
		
		public ProtocolRequired()
		{
		}
		
		public ProtocolRequired(uint arg1, uint arg2)
			: this()
		{
			initProtocolRequired(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 1;
		}
		
		public ProtocolRequired initProtocolRequired(uint arg1 = 0, uint arg2 = 0)
		{
			this.requiredVersion = arg1;
			this.currentVersion = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.requiredVersion = 0;
			this.currentVersion = 0;
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
			this.serializeAs_ProtocolRequired(arg1);
		}
		
		public void serializeAs_ProtocolRequired(BigEndianWriter arg1)
		{
			if ( this.requiredVersion < 0 )
			{
				throw new Exception("Forbidden value (" + this.requiredVersion + ") on element requiredVersion.");
			}
			arg1.WriteInt((int)this.requiredVersion);
			if ( this.currentVersion < 0 )
			{
				throw new Exception("Forbidden value (" + this.currentVersion + ") on element currentVersion.");
			}
			arg1.WriteInt((int)this.currentVersion);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ProtocolRequired(arg1);
		}
		
		public void deserializeAs_ProtocolRequired(BigEndianReader arg1)
		{
			this.requiredVersion = (uint)arg1.ReadInt();
			if ( this.requiredVersion < 0 )
			{
				throw new Exception("Forbidden value (" + this.requiredVersion + ") on element of ProtocolRequired.requiredVersion.");
			}
			this.currentVersion = (uint)arg1.ReadInt();
			if ( this.currentVersion < 0 )
			{
				throw new Exception("Forbidden value (" + this.currentVersion + ") on element of ProtocolRequired.currentVersion.");
			}
		}
		
	}
}
