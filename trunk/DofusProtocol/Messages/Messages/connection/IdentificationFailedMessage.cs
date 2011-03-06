using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class IdentificationFailedMessage : Message
	{
		public const uint protocolId = 20;
		internal Boolean _isInitialized = false;
		public uint reason = 99;
		
		public IdentificationFailedMessage()
		{
		}
		
		public IdentificationFailedMessage(uint arg1)
			: this()
		{
			initIdentificationFailedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 20;
		}
		
		public IdentificationFailedMessage initIdentificationFailedMessage(uint arg1 = 99)
		{
			this.reason = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.reason = 99;
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
			this.serializeAs_IdentificationFailedMessage(arg1);
		}
		
		public void serializeAs_IdentificationFailedMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.reason);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_IdentificationFailedMessage(arg1);
		}
		
		public void deserializeAs_IdentificationFailedMessage(BigEndianReader arg1)
		{
			this.reason = (uint)arg1.ReadByte();
			if ( this.reason < 0 )
			{
				throw new Exception("Forbidden value (" + this.reason + ") on element of IdentificationFailedMessage.reason.");
			}
		}
		
	}
}
