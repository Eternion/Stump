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
	
	public class IdentificationMessageWithServerIdMessage : IdentificationMessage
	{
		public const uint protocolId = 6104;
		internal Boolean _isInitialized = false;
		public int serverId = 0;
		
		public override uint getMessageId ()
		{
			return 6104;
		}
		
		public IdentificationMessageWithServerIdMessage initIdentificationMessageWithServerIdMessage (Stump.DofusProtocol.Classes.Version arg1 = null, String arg2 = "", String arg3 = "", Boolean arg4 = false, int arg5 = 0)
		{
			base.initIdentificationMessage(arg1, arg2, arg3, arg4);
			this.serverId = arg5;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset ()
		{
			base.reset();
			this.serverId = 0;
			this._isInitialized = false;
		}
		
		public override void pack (BigEndianWriter arg1)
		{
			this.serialize(arg1);
			WritePacket(arg1, this.getMessageId());
		}
		
		public override void unpack (BigEndianReader arg1, uint arg2)
		{
			this.deserialize(arg1);
		}
		
		public new void serialize (BigEndianWriter arg1)
		{
			this.serializeAs_IdentificationMessageWithServerIdMessage(arg1);
		}
		
		public void serializeAs_IdentificationMessageWithServerIdMessage (BigEndianWriter arg1)
		{
			base.serializeAs_IdentificationMessage(arg1);
			arg1.WriteShort((short)this.serverId);
		}
		
		public new void deserialize (BigEndianReader arg1)
		{
			this.deserializeAs_IdentificationMessageWithServerIdMessage(arg1);
		}
		
		public void deserializeAs_IdentificationMessageWithServerIdMessage (BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.serverId = (int)arg1.ReadShort();
		}
		
	}
}
