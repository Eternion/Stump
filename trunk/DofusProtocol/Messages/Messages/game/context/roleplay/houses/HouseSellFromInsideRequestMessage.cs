using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class HouseSellFromInsideRequestMessage : HouseSellRequestMessage
	{
		public const uint protocolId = 5884;
		internal Boolean _isInitialized = false;
		
		public HouseSellFromInsideRequestMessage()
		{
		}
		
		public HouseSellFromInsideRequestMessage(uint arg1)
			: this()
		{
			initHouseSellFromInsideRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5884;
		}
		
		public HouseSellFromInsideRequestMessage initHouseSellFromInsideRequestMessage(uint arg1 = 0)
		{
			base.initHouseSellRequestMessage(arg1);
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
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
			this.serializeAs_HouseSellFromInsideRequestMessage(arg1);
		}
		
		public void serializeAs_HouseSellFromInsideRequestMessage(BigEndianWriter arg1)
		{
			base.serializeAs_HouseSellRequestMessage(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_HouseSellFromInsideRequestMessage(arg1);
		}
		
		public void deserializeAs_HouseSellFromInsideRequestMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
		}
		
	}
}
