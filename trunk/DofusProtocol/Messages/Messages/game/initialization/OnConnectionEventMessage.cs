using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class OnConnectionEventMessage : Message
	{
		public const uint protocolId = 5726;
		internal Boolean _isInitialized = false;
		public uint eventType = 0;
		
		public OnConnectionEventMessage()
		{
		}
		
		public OnConnectionEventMessage(uint arg1)
			: this()
		{
			initOnConnectionEventMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5726;
		}
		
		public OnConnectionEventMessage initOnConnectionEventMessage(uint arg1 = 0)
		{
			this.eventType = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.eventType = 0;
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
			this.serializeAs_OnConnectionEventMessage(arg1);
		}
		
		public void serializeAs_OnConnectionEventMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.eventType);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_OnConnectionEventMessage(arg1);
		}
		
		public void deserializeAs_OnConnectionEventMessage(BigEndianReader arg1)
		{
			this.eventType = (uint)arg1.ReadByte();
			if ( this.eventType < 0 )
			{
				throw new Exception("Forbidden value (" + this.eventType + ") on element of OnConnectionEventMessage.eventType.");
			}
		}
		
	}
}
