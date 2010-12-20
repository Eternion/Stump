using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PrismFightStateUpdateMessage : Message
	{
		public const uint protocolId = 6040;
		internal Boolean _isInitialized = false;
		public uint state = 0;
		
		public PrismFightStateUpdateMessage()
		{
		}
		
		public PrismFightStateUpdateMessage(uint arg1)
			: this()
		{
			initPrismFightStateUpdateMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6040;
		}
		
		public PrismFightStateUpdateMessage initPrismFightStateUpdateMessage(uint arg1 = 0)
		{
			this.state = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.state = 0;
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
			this.serializeAs_PrismFightStateUpdateMessage(arg1);
		}
		
		public void serializeAs_PrismFightStateUpdateMessage(BigEndianWriter arg1)
		{
			if ( this.state < 0 )
			{
				throw new Exception("Forbidden value (" + this.state + ") on element state.");
			}
			arg1.WriteByte((byte)this.state);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PrismFightStateUpdateMessage(arg1);
		}
		
		public void deserializeAs_PrismFightStateUpdateMessage(BigEndianReader arg1)
		{
			this.state = (uint)arg1.ReadByte();
			if ( this.state < 0 )
			{
				throw new Exception("Forbidden value (" + this.state + ") on element of PrismFightStateUpdateMessage.state.");
			}
		}
		
	}
}
