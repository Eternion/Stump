using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PartyInvitationMessage : Message
	{
		public const uint protocolId = 5586;
		internal Boolean _isInitialized = false;
		public uint fromId = 0;
		public String fromName = "";
		public uint toId = 0;
		public String toName = "";
		
		public PartyInvitationMessage()
		{
		}
		
		public PartyInvitationMessage(uint arg1, String arg2, uint arg3, String arg4)
			: this()
		{
			initPartyInvitationMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 5586;
		}
		
		public PartyInvitationMessage initPartyInvitationMessage(uint arg1 = 0, String arg2 = "", uint arg3 = 0, String arg4 = "")
		{
			this.fromId = arg1;
			this.fromName = arg2;
			this.toId = arg3;
			this.toName = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.fromId = 0;
			this.fromName = "";
			this.toId = 0;
			this.toName = "";
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
			this.serializeAs_PartyInvitationMessage(arg1);
		}
		
		public void serializeAs_PartyInvitationMessage(BigEndianWriter arg1)
		{
			if ( this.fromId < 0 )
			{
				throw new Exception("Forbidden value (" + this.fromId + ") on element fromId.");
			}
			arg1.WriteInt((int)this.fromId);
			arg1.WriteUTF((string)this.fromName);
			if ( this.toId < 0 )
			{
				throw new Exception("Forbidden value (" + this.toId + ") on element toId.");
			}
			arg1.WriteInt((int)this.toId);
			arg1.WriteUTF((string)this.toName);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PartyInvitationMessage(arg1);
		}
		
		public void deserializeAs_PartyInvitationMessage(BigEndianReader arg1)
		{
			this.fromId = (uint)arg1.ReadInt();
			if ( this.fromId < 0 )
			{
				throw new Exception("Forbidden value (" + this.fromId + ") on element of PartyInvitationMessage.fromId.");
			}
			this.fromName = (String)arg1.ReadUTF();
			this.toId = (uint)arg1.ReadInt();
			if ( this.toId < 0 )
			{
				throw new Exception("Forbidden value (" + this.toId + ") on element of PartyInvitationMessage.toId.");
			}
			this.toName = (String)arg1.ReadUTF();
		}
		
	}
}
