using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ContactLookErrorMessage : Message
	{
		public const uint protocolId = 6045;
		internal Boolean _isInitialized = false;
		public uint requestId = 0;
		
		public ContactLookErrorMessage()
		{
		}
		
		public ContactLookErrorMessage(uint arg1)
			: this()
		{
			initContactLookErrorMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6045;
		}
		
		public ContactLookErrorMessage initContactLookErrorMessage(uint arg1 = 0)
		{
			this.requestId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.requestId = 0;
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
			this.serializeAs_ContactLookErrorMessage(arg1);
		}
		
		public void serializeAs_ContactLookErrorMessage(BigEndianWriter arg1)
		{
			if ( this.requestId < 0 )
			{
				throw new Exception("Forbidden value (" + this.requestId + ") on element requestId.");
			}
			arg1.WriteInt((int)this.requestId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ContactLookErrorMessage(arg1);
		}
		
		public void deserializeAs_ContactLookErrorMessage(BigEndianReader arg1)
		{
			this.requestId = (uint)arg1.ReadInt();
			if ( this.requestId < 0 )
			{
				throw new Exception("Forbidden value (" + this.requestId + ") on element of ContactLookErrorMessage.requestId.");
			}
		}
		
	}
}
