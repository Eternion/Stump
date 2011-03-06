using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeObjectMoveMessage : Message
	{
		public const uint protocolId = 5518;
		internal Boolean _isInitialized = false;
		public uint objectUID = 0;
		public int quantity = 0;
		
		public ExchangeObjectMoveMessage()
		{
		}
		
		public ExchangeObjectMoveMessage(uint arg1, int arg2)
			: this()
		{
			initExchangeObjectMoveMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5518;
		}
		
		public ExchangeObjectMoveMessage initExchangeObjectMoveMessage(uint arg1 = 0, int arg2 = 0)
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
			this.serializeAs_ExchangeObjectMoveMessage(arg1);
		}
		
		public void serializeAs_ExchangeObjectMoveMessage(BigEndianWriter arg1)
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
			this.deserializeAs_ExchangeObjectMoveMessage(arg1);
		}
		
		public void deserializeAs_ExchangeObjectMoveMessage(BigEndianReader arg1)
		{
			this.@objectUID = (uint)arg1.ReadInt();
			if ( this.@objectUID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectUID + ") on element of ExchangeObjectMoveMessage.objectUID.");
			}
			this.quantity = (int)arg1.ReadInt();
		}
		
	}
}
