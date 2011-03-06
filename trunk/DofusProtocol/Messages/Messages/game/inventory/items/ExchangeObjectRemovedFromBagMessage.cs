using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeObjectRemovedFromBagMessage : ExchangeObjectMessage
	{
		public const uint protocolId = 6010;
		internal Boolean _isInitialized = false;
		public uint objectUID = 0;
		
		public ExchangeObjectRemovedFromBagMessage()
		{
		}
		
		public ExchangeObjectRemovedFromBagMessage(Boolean arg1, uint arg2)
			: this()
		{
			initExchangeObjectRemovedFromBagMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6010;
		}
		
		public ExchangeObjectRemovedFromBagMessage initExchangeObjectRemovedFromBagMessage(Boolean arg1 = false, uint arg2 = 0)
		{
			base.initExchangeObjectMessage(arg1);
			this.@objectUID = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.@objectUID = 0;
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
			this.serializeAs_ExchangeObjectRemovedFromBagMessage(arg1);
		}
		
		public void serializeAs_ExchangeObjectRemovedFromBagMessage(BigEndianWriter arg1)
		{
			base.serializeAs_ExchangeObjectMessage(arg1);
			if ( this.@objectUID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectUID + ") on element objectUID.");
			}
			arg1.WriteInt((int)this.@objectUID);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeObjectRemovedFromBagMessage(arg1);
		}
		
		public void deserializeAs_ExchangeObjectRemovedFromBagMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.@objectUID = (uint)arg1.ReadInt();
			if ( this.@objectUID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectUID + ") on element of ExchangeObjectRemovedFromBagMessage.objectUID.");
			}
		}
		
	}
}
