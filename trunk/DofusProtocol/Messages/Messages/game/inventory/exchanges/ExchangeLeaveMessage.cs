using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeLeaveMessage : LeaveDialogMessage
	{
		public const uint protocolId = 5628;
		internal Boolean _isInitialized = false;
		public Boolean success = false;
		
		public ExchangeLeaveMessage()
		{
		}
		
		public ExchangeLeaveMessage(Boolean arg1)
			: this()
		{
			initExchangeLeaveMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5628;
		}
		
		public ExchangeLeaveMessage initExchangeLeaveMessage(Boolean arg1 = false)
		{
			this.success = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.success = false;
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
			this.serializeAs_ExchangeLeaveMessage(arg1);
		}
		
		public void serializeAs_ExchangeLeaveMessage(BigEndianWriter arg1)
		{
			base.serializeAs_LeaveDialogMessage(arg1);
			arg1.WriteBoolean(this.success);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeLeaveMessage(arg1);
		}
		
		public void deserializeAs_ExchangeLeaveMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.success = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
