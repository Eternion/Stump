using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PartyMemberRemoveMessage : Message
	{
		public const uint protocolId = 5579;
		internal Boolean _isInitialized = false;
		public uint leavingPlayerId = 0;
		
		public PartyMemberRemoveMessage()
		{
		}
		
		public PartyMemberRemoveMessage(uint arg1)
			: this()
		{
			initPartyMemberRemoveMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5579;
		}
		
		public PartyMemberRemoveMessage initPartyMemberRemoveMessage(uint arg1 = 0)
		{
			this.leavingPlayerId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.leavingPlayerId = 0;
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
			this.serializeAs_PartyMemberRemoveMessage(arg1);
		}
		
		public void serializeAs_PartyMemberRemoveMessage(BigEndianWriter arg1)
		{
			if ( this.leavingPlayerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.leavingPlayerId + ") on element leavingPlayerId.");
			}
			arg1.WriteInt((int)this.leavingPlayerId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PartyMemberRemoveMessage(arg1);
		}
		
		public void deserializeAs_PartyMemberRemoveMessage(BigEndianReader arg1)
		{
			this.leavingPlayerId = (uint)arg1.ReadInt();
			if ( this.leavingPlayerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.leavingPlayerId + ") on element of PartyMemberRemoveMessage.leavingPlayerId.");
			}
		}
		
	}
}
