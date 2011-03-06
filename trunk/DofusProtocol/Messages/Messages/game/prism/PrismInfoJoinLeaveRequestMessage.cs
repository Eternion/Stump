using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PrismInfoJoinLeaveRequestMessage : Message
	{
		public const uint protocolId = 5844;
		internal Boolean _isInitialized = false;
		public Boolean join = false;
		
		public PrismInfoJoinLeaveRequestMessage()
		{
		}
		
		public PrismInfoJoinLeaveRequestMessage(Boolean arg1)
			: this()
		{
			initPrismInfoJoinLeaveRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5844;
		}
		
		public PrismInfoJoinLeaveRequestMessage initPrismInfoJoinLeaveRequestMessage(Boolean arg1 = false)
		{
			this.join = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.join = false;
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
			this.serializeAs_PrismInfoJoinLeaveRequestMessage(arg1);
		}
		
		public void serializeAs_PrismInfoJoinLeaveRequestMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.join);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PrismInfoJoinLeaveRequestMessage(arg1);
		}
		
		public void deserializeAs_PrismInfoJoinLeaveRequestMessage(BigEndianReader arg1)
		{
			this.join = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
