using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ContactLookMessage : Message
	{
		public const uint protocolId = 5934;
		internal Boolean _isInitialized = false;
		public uint requestId = 0;
		public String playerName = "";
		public uint playerId = 0;
		public EntityLook look;
		
		public ContactLookMessage()
		{
			this.look = new EntityLook();
		}
		
		public ContactLookMessage(uint arg1, String arg2, uint arg3, EntityLook arg4)
			: this()
		{
			initContactLookMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 5934;
		}
		
		public ContactLookMessage initContactLookMessage(uint arg1 = 0, String arg2 = "", uint arg3 = 0, EntityLook arg4 = null)
		{
			this.requestId = arg1;
			this.playerName = arg2;
			this.playerId = arg3;
			this.look = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.requestId = 0;
			this.playerName = "";
			this.playerId = 0;
			this.look = new EntityLook();
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
			this.serializeAs_ContactLookMessage(arg1);
		}
		
		public void serializeAs_ContactLookMessage(BigEndianWriter arg1)
		{
			if ( this.requestId < 0 )
			{
				throw new Exception("Forbidden value (" + this.requestId + ") on element requestId.");
			}
			arg1.WriteInt((int)this.requestId);
			arg1.WriteUTF((string)this.playerName);
			if ( this.playerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.playerId + ") on element playerId.");
			}
			arg1.WriteInt((int)this.playerId);
			this.look.serializeAs_EntityLook(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ContactLookMessage(arg1);
		}
		
		public void deserializeAs_ContactLookMessage(BigEndianReader arg1)
		{
			this.requestId = (uint)arg1.ReadInt();
			if ( this.requestId < 0 )
			{
				throw new Exception("Forbidden value (" + this.requestId + ") on element of ContactLookMessage.requestId.");
			}
			this.playerName = (String)arg1.ReadUTF();
			this.playerId = (uint)arg1.ReadInt();
			if ( this.playerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.playerId + ") on element of ContactLookMessage.playerId.");
			}
			this.look = new EntityLook();
			this.look.deserialize(arg1);
		}
		
	}
}
