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
	
	public class ExchangeCraftResultMagicWithObjectDescMessage : ExchangeCraftResultWithObjectDescMessage
	{
		public const uint protocolId = 6188;
		internal Boolean _isInitialized = false;
		public int magicPoolStatus = 0;
		
		public ExchangeCraftResultMagicWithObjectDescMessage()
		{
		}
		
		public ExchangeCraftResultMagicWithObjectDescMessage(uint arg1, ObjectItemNotInContainer arg2, int arg3)
			: this()
		{
			initExchangeCraftResultMagicWithObjectDescMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 6188;
		}
		
		public ExchangeCraftResultMagicWithObjectDescMessage initExchangeCraftResultMagicWithObjectDescMessage(uint arg1 = 0, ObjectItemNotInContainer arg2 = null, int arg3 = 0)
		{
			base.initExchangeCraftResultWithObjectDescMessage(arg1, arg2);
			this.magicPoolStatus = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.magicPoolStatus = 0;
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
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ExchangeCraftResultMagicWithObjectDescMessage(arg1);
		}
		
		public void serializeAs_ExchangeCraftResultMagicWithObjectDescMessage(BigEndianWriter arg1)
		{
			base.serializeAs_ExchangeCraftResultWithObjectDescMessage(arg1);
			arg1.WriteByte((byte)this.magicPoolStatus);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeCraftResultMagicWithObjectDescMessage(arg1);
		}
		
		public void deserializeAs_ExchangeCraftResultMagicWithObjectDescMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.magicPoolStatus = (int)arg1.ReadByte();
		}
		
	}
}
