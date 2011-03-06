using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeObjectUseInWorkshopMessage : Message
	{
		public const uint protocolId = 6004;
		internal Boolean _isInitialized = false;
		public uint objectUID = 0;
		public int quantity = 0;
		
		public ExchangeObjectUseInWorkshopMessage()
		{
		}
		
		public ExchangeObjectUseInWorkshopMessage(uint arg1, int arg2)
			: this()
		{
			initExchangeObjectUseInWorkshopMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6004;
		}
		
		public ExchangeObjectUseInWorkshopMessage initExchangeObjectUseInWorkshopMessage(uint arg1 = 0, int arg2 = 0)
		{
			this.@objectUID = arg1;
			this.quantity = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.@objectUID = 0;
			this.quantity = 0;
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
			this.serializeAs_ExchangeObjectUseInWorkshopMessage(arg1);
		}
		
		public void serializeAs_ExchangeObjectUseInWorkshopMessage(BigEndianWriter arg1)
		{
			if ( this.@objectUID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectUID + ") on element objectUID.");
			}
			arg1.WriteInt((int)this.@objectUID);
			arg1.WriteInt((int)this.quantity);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeObjectUseInWorkshopMessage(arg1);
		}
		
		public void deserializeAs_ExchangeObjectUseInWorkshopMessage(BigEndianReader arg1)
		{
			this.@objectUID = (uint)arg1.ReadInt();
			if ( this.@objectUID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectUID + ") on element of ExchangeObjectUseInWorkshopMessage.objectUID.");
			}
			this.quantity = (int)arg1.ReadInt();
		}
		
	}
}
