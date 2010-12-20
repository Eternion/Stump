using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ContactLookRequestMessage : Message
	{
		public const uint protocolId = 5932;
		internal Boolean _isInitialized = false;
		public uint requestId = 0;
		public uint contactType = 0;
		
		public ContactLookRequestMessage()
		{
		}
		
		public ContactLookRequestMessage(uint arg1, uint arg2)
			: this()
		{
			initContactLookRequestMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5932;
		}
		
		public ContactLookRequestMessage initContactLookRequestMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.requestId = arg1;
			this.contactType = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.requestId = 0;
			this.contactType = 0;
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
			this.serializeAs_ContactLookRequestMessage(arg1);
		}
		
		public void serializeAs_ContactLookRequestMessage(BigEndianWriter arg1)
		{
			if ( this.requestId < 0 || this.requestId > 255 )
			{
				throw new Exception("Forbidden value (" + this.requestId + ") on element requestId.");
			}
			arg1.WriteByte((byte)this.requestId);
			arg1.WriteByte((byte)this.contactType);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ContactLookRequestMessage(arg1);
		}
		
		public void deserializeAs_ContactLookRequestMessage(BigEndianReader arg1)
		{
			this.requestId = (uint)arg1.ReadByte();
			if ( this.requestId < 0 || this.requestId > 255 )
			{
				throw new Exception("Forbidden value (" + this.requestId + ") on element of ContactLookRequestMessage.requestId.");
			}
			this.contactType = (uint)arg1.ReadByte();
			if ( this.contactType < 0 )
			{
				throw new Exception("Forbidden value (" + this.contactType + ") on element of ContactLookRequestMessage.contactType.");
			}
		}
		
	}
}
