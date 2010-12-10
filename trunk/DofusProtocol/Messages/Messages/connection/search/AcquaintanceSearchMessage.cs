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
	
	public class AcquaintanceSearchMessage : Message
	{
		public const uint protocolId = 6144;
		internal Boolean _isInitialized = false;
		public String nickname = "";
		
		public AcquaintanceSearchMessage()
		{
		}
		
		public AcquaintanceSearchMessage(String arg1)
			: this()
		{
			initAcquaintanceSearchMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6144;
		}
		
		public AcquaintanceSearchMessage initAcquaintanceSearchMessage(String arg1 = "")
		{
			this.nickname = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.nickname = "";
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
			this.serializeAs_AcquaintanceSearchMessage(arg1);
		}
		
		public void serializeAs_AcquaintanceSearchMessage(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.nickname);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AcquaintanceSearchMessage(arg1);
		}
		
		public void deserializeAs_AcquaintanceSearchMessage(BigEndianReader arg1)
		{
			this.nickname = (String)arg1.ReadUTF();
		}
		
	}
}
