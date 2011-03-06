using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GoldAddedMessage : Message
	{
		public const uint protocolId = 6030;
		internal Boolean _isInitialized = false;
		public GoldItem gold;
		
		public GoldAddedMessage()
		{
			this.gold = new GoldItem();
		}
		
		public GoldAddedMessage(GoldItem arg1)
			: this()
		{
			initGoldAddedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6030;
		}
		
		public GoldAddedMessage initGoldAddedMessage(GoldItem arg1 = null)
		{
			this.gold = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.gold = new GoldItem();
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
			this.serializeAs_GoldAddedMessage(arg1);
		}
		
		public void serializeAs_GoldAddedMessage(BigEndianWriter arg1)
		{
			this.gold.serializeAs_GoldItem(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GoldAddedMessage(arg1);
		}
		
		public void deserializeAs_GoldAddedMessage(BigEndianReader arg1)
		{
			this.gold = new GoldItem();
			this.gold.deserialize(arg1);
		}
		
	}
}
