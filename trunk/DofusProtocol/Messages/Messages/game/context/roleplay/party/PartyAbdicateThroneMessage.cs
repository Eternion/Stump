using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PartyAbdicateThroneMessage : Message
	{
		public const uint protocolId = 6080;
		internal Boolean _isInitialized = false;
		public uint playerId = 0;
		
		public PartyAbdicateThroneMessage()
		{
		}
		
		public PartyAbdicateThroneMessage(uint arg1)
			: this()
		{
			initPartyAbdicateThroneMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6080;
		}
		
		public PartyAbdicateThroneMessage initPartyAbdicateThroneMessage(uint arg1 = 0)
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
			this.serializeAs_PartyAbdicateThroneMessage(arg1);
		}
		
		public void serializeAs_PartyAbdicateThroneMessage(BigEndianWriter arg1)
		{
			if ( this.playerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.playerId + ") on element playerId.");
			}
			arg1.WriteInt((int)this.playerId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PartyAbdicateThroneMessage(arg1);
		}
		
		public void deserializeAs_PartyAbdicateThroneMessage(BigEndianReader arg1)
		{
			this.playerId = (uint)arg1.ReadInt();
			if ( this.playerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.playerId + ") on element of PartyAbdicateThroneMessage.playerId.");
			}
		}
		
	}
}
