using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class IdentificationFailedBannedMessage : IdentificationFailedMessage
	{
		public const uint protocolId = 6174;
		internal Boolean _isInitialized = false;
		public uint duration = 0;
		
		public IdentificationFailedBannedMessage()
		{
		}
		
		public IdentificationFailedBannedMessage(uint arg1, uint arg2)
			: this()
		{
			initIdentificationFailedBannedMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6174;
		}
		
		public IdentificationFailedBannedMessage initIdentificationFailedBannedMessage(uint arg1 = 99, uint arg2 = 0)
		{
			base.initIdentificationFailedMessage(arg1);
			this.duration = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.duration = 0;
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
			this.serializeAs_IdentificationFailedBannedMessage(arg1);
		}
		
		public void serializeAs_IdentificationFailedBannedMessage(BigEndianWriter arg1)
		{
			base.serializeAs_IdentificationFailedMessage(arg1);
			if ( this.duration < 0 )
			{
				throw new Exception("Forbidden value (" + this.duration + ") on element duration.");
			}
			arg1.WriteInt((int)this.duration);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_IdentificationFailedBannedMessage(arg1);
		}
		
		public void deserializeAs_IdentificationFailedBannedMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.duration = (uint)arg1.ReadInt();
			if ( this.duration < 0 )
			{
				throw new Exception("Forbidden value (" + this.duration + ") on element of IdentificationFailedBannedMessage.duration.");
			}
		}
		
	}
}
