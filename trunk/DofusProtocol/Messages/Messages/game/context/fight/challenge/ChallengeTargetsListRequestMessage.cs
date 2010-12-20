using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ChallengeTargetsListRequestMessage : Message
	{
		public const uint protocolId = 5614;
		internal Boolean _isInitialized = false;
		public uint challengeId = 0;
		
		public ChallengeTargetsListRequestMessage()
		{
		}
		
		public ChallengeTargetsListRequestMessage(uint arg1)
			: this()
		{
			initChallengeTargetsListRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5614;
		}
		
		public ChallengeTargetsListRequestMessage initChallengeTargetsListRequestMessage(uint arg1 = 0)
		{
			this.challengeId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.challengeId = 0;
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
			this.serializeAs_ChallengeTargetsListRequestMessage(arg1);
		}
		
		public void serializeAs_ChallengeTargetsListRequestMessage(BigEndianWriter arg1)
		{
			if ( this.challengeId < 0 )
			{
				throw new Exception("Forbidden value (" + this.challengeId + ") on element challengeId.");
			}
			arg1.WriteByte((byte)this.challengeId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ChallengeTargetsListRequestMessage(arg1);
		}
		
		public void deserializeAs_ChallengeTargetsListRequestMessage(BigEndianReader arg1)
		{
			this.challengeId = (uint)arg1.ReadByte();
			if ( this.challengeId < 0 )
			{
				throw new Exception("Forbidden value (" + this.challengeId + ") on element of ChallengeTargetsListRequestMessage.challengeId.");
			}
		}
		
	}
}
