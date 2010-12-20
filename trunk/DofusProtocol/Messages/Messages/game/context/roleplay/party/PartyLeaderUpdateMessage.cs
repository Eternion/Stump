using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PartyLeaderUpdateMessage : Message
	{
		public const uint protocolId = 5578;
		internal Boolean _isInitialized = false;
		public uint partyLeaderId = 0;
		
		public PartyLeaderUpdateMessage()
		{
		}
		
		public PartyLeaderUpdateMessage(uint arg1)
			: this()
		{
			initPartyLeaderUpdateMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5578;
		}
		
		public PartyLeaderUpdateMessage initPartyLeaderUpdateMessage(uint arg1 = 0)
		{
			this.partyLeaderId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.partyLeaderId = 0;
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
			this.serializeAs_PartyLeaderUpdateMessage(arg1);
		}
		
		public void serializeAs_PartyLeaderUpdateMessage(BigEndianWriter arg1)
		{
			if ( this.partyLeaderId < 0 )
			{
				throw new Exception("Forbidden value (" + this.partyLeaderId + ") on element partyLeaderId.");
			}
			arg1.WriteInt((int)this.partyLeaderId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PartyLeaderUpdateMessage(arg1);
		}
		
		public void deserializeAs_PartyLeaderUpdateMessage(BigEndianReader arg1)
		{
			this.partyLeaderId = (uint)arg1.ReadInt();
			if ( this.partyLeaderId < 0 )
			{
				throw new Exception("Forbidden value (" + this.partyLeaderId + ") on element of PartyLeaderUpdateMessage.partyLeaderId.");
			}
		}
		
	}
}
