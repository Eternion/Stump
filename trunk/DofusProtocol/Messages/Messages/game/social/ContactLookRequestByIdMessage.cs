using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ContactLookRequestByIdMessage : ContactLookRequestMessage
	{
		public const uint protocolId = 5935;
		internal Boolean _isInitialized = false;
		public uint playerId = 0;
		
		public ContactLookRequestByIdMessage()
		{
		}
		
		public ContactLookRequestByIdMessage(uint arg1, uint arg2, uint arg3)
			: this()
		{
			initContactLookRequestByIdMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5935;
		}
		
		public ContactLookRequestByIdMessage initContactLookRequestByIdMessage(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0)
		{
			base.initContactLookRequestMessage(arg1, arg2);
			this.playerId = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.playerId = 0;
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
			this.serializeAs_ContactLookRequestByIdMessage(arg1);
		}
		
		public void serializeAs_ContactLookRequestByIdMessage(BigEndianWriter arg1)
		{
			base.serializeAs_ContactLookRequestMessage(arg1);
			if ( this.playerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.playerId + ") on element playerId.");
			}
			arg1.WriteInt((int)this.playerId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ContactLookRequestByIdMessage(arg1);
		}
		
		public void deserializeAs_ContactLookRequestByIdMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.playerId = (uint)arg1.ReadInt();
			if ( this.playerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.playerId + ") on element of ContactLookRequestByIdMessage.playerId.");
			}
		}
		
	}
}
