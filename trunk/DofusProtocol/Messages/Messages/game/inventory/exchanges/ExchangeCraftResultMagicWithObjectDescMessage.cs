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
