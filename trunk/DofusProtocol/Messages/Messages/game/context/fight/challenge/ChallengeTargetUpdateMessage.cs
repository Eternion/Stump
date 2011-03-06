using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ChallengeTargetUpdateMessage : Message
	{
		public const uint protocolId = 6123;
		internal Boolean _isInitialized = false;
		public uint challengeId = 0;
		public int targetId = 0;
		
		public ChallengeTargetUpdateMessage()
		{
		}
		
		public ChallengeTargetUpdateMessage(uint arg1, int arg2)
			: this()
		{
			initChallengeTargetUpdateMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6123;
		}
		
		public ChallengeTargetUpdateMessage initChallengeTargetUpdateMessage(uint arg1 = 0, int arg2 = 0)
		{
			this.challengeId = arg1;
			this.targetId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.challengeId = 0;
			this.targetId = 0;
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
			this.serializeAs_ChallengeTargetUpdateMessage(arg1);
		}
		
		public void serializeAs_ChallengeTargetUpdateMessage(BigEndianWriter arg1)
		{
			if ( this.challengeId < 0 )
			{
				throw new Exception("Forbidden value (" + this.challengeId + ") on element challengeId.");
			}
			arg1.WriteByte((byte)this.challengeId);
			arg1.WriteInt((int)this.targetId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ChallengeTargetUpdateMessage(arg1);
		}
		
		public void deserializeAs_ChallengeTargetUpdateMessage(BigEndianReader arg1)
		{
			this.challengeId = (uint)arg1.ReadByte();
			if ( this.challengeId < 0 )
			{
				throw new Exception("Forbidden value (" + this.challengeId + ") on element of ChallengeTargetUpdateMessage.challengeId.");
			}
			this.targetId = (int)arg1.ReadInt();
		}
		
	}
}
