using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PartyFollowStatusUpdateMessage : Message
	{
		public const uint protocolId = 5581;
		internal Boolean _isInitialized = false;
		public Boolean success = false;
		public uint followedId = 0;
		
		public PartyFollowStatusUpdateMessage()
		{
		}
		
		public PartyFollowStatusUpdateMessage(Boolean arg1, uint arg2)
			: this()
		{
			initPartyFollowStatusUpdateMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5581;
		}
		
		public PartyFollowStatusUpdateMessage initPartyFollowStatusUpdateMessage(Boolean arg1 = false, uint arg2 = 0)
		{
			this.success = arg1;
			this.followedId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.success = false;
			this.followedId = 0;
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
			this.serializeAs_PartyFollowStatusUpdateMessage(arg1);
		}
		
		public void serializeAs_PartyFollowStatusUpdateMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.success);
			if ( this.followedId < 0 )
			{
				throw new Exception("Forbidden value (" + this.followedId + ") on element followedId.");
			}
			arg1.WriteInt((int)this.followedId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PartyFollowStatusUpdateMessage(arg1);
		}
		
		public void deserializeAs_PartyFollowStatusUpdateMessage(BigEndianReader arg1)
		{
			this.success = (Boolean)arg1.ReadBoolean();
			this.followedId = (uint)arg1.ReadInt();
			if ( this.followedId < 0 )
			{
				throw new Exception("Forbidden value (" + this.followedId + ") on element of PartyFollowStatusUpdateMessage.followedId.");
			}
		}
		
	}
}
