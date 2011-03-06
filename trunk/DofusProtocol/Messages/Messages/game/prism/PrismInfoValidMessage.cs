using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PrismInfoValidMessage : Message
	{
		public const uint protocolId = 5858;
		internal Boolean _isInitialized = false;
		public ProtectedEntityWaitingForHelpInfo waitingForHelpInfo;
		
		public PrismInfoValidMessage()
		{
			this.waitingForHelpInfo = new ProtectedEntityWaitingForHelpInfo();
		}
		
		public PrismInfoValidMessage(ProtectedEntityWaitingForHelpInfo arg1)
			: this()
		{
			initPrismInfoValidMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5858;
		}
		
		public PrismInfoValidMessage initPrismInfoValidMessage(ProtectedEntityWaitingForHelpInfo arg1 = null)
		{
			this.waitingForHelpInfo = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.waitingForHelpInfo = new ProtectedEntityWaitingForHelpInfo();
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
			this.serializeAs_PrismInfoValidMessage(arg1);
		}
		
		public void serializeAs_PrismInfoValidMessage(BigEndianWriter arg1)
		{
			this.waitingForHelpInfo.serializeAs_ProtectedEntityWaitingForHelpInfo(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PrismInfoValidMessage(arg1);
		}
		
		public void deserializeAs_PrismInfoValidMessage(BigEndianReader arg1)
		{
			this.waitingForHelpInfo = new ProtectedEntityWaitingForHelpInfo();
			this.waitingForHelpInfo.deserialize(arg1);
		}
		
	}
}
