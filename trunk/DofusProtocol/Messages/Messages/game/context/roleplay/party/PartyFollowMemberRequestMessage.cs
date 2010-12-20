using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PartyFollowMemberRequestMessage : Message
	{
		public const uint protocolId = 5577;
		internal Boolean _isInitialized = false;
		public uint playerId = 0;
		
		public PartyFollowMemberRequestMessage()
		{
		}
		
		public PartyFollowMemberRequestMessage(uint arg1)
			: this()
		{
			initPartyFollowMemberRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5577;
		}
		
		public PartyFollowMemberRequestMessage initPartyFollowMemberRequestMessage(uint arg1 = 0)
		{
			this.playerId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
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
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_PartyFollowMemberRequestMessage(arg1);
		}
		
		public void serializeAs_PartyFollowMemberRequestMessage(BigEndianWriter arg1)
		{
			if ( this.playerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.playerId + ") on element playerId.");
			}
			arg1.WriteInt((int)this.playerId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PartyFollowMemberRequestMessage(arg1);
		}
		
		public void deserializeAs_PartyFollowMemberRequestMessage(BigEndianReader arg1)
		{
			this.playerId = (uint)arg1.ReadInt();
			if ( this.playerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.playerId + ") on element of PartyFollowMemberRequestMessage.playerId.");
			}
		}
		
	}
}
