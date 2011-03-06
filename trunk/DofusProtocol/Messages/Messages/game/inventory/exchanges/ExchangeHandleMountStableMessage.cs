using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeHandleMountStableMessage : Message
	{
		public const uint protocolId = 5965;
		internal Boolean _isInitialized = false;
		public int actionType = 0;
		public uint rideId = 0;
		
		public ExchangeHandleMountStableMessage()
		{
		}
		
		public ExchangeHandleMountStableMessage(int arg1, uint arg2)
			: this()
		{
			initExchangeHandleMountStableMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5965;
		}
		
		public ExchangeHandleMountStableMessage initExchangeHandleMountStableMessage(int arg1 = 0, uint arg2 = 0)
		{
			this.actionType = arg1;
			this.rideId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.actionType = 0;
			this.rideId = 0;
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
			this.serializeAs_ExchangeHandleMountStableMessage(arg1);
		}
		
		public void serializeAs_ExchangeHandleMountStableMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.actionType);
			if ( this.rideId < 0 )
			{
				throw new Exception("Forbidden value (" + this.rideId + ") on element rideId.");
			}
			arg1.WriteInt((int)this.rideId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeHandleMountStableMessage(arg1);
		}
		
		public void deserializeAs_ExchangeHandleMountStableMessage(BigEndianReader arg1)
		{
			this.actionType = (int)arg1.ReadByte();
			this.rideId = (uint)arg1.ReadInt();
			if ( this.rideId < 0 )
			{
				throw new Exception("Forbidden value (" + this.rideId + ") on element of ExchangeHandleMountStableMessage.rideId.");
			}
		}
		
	}
}
