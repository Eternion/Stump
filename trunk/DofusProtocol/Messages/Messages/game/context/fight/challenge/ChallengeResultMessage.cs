using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ChallengeResultMessage : Message
	{
		public const uint protocolId = 6019;
		internal Boolean _isInitialized = false;
		public uint challengeId = 0;
		public Boolean success = false;
		
		public ChallengeResultMessage()
		{
		}
		
		public ChallengeResultMessage(uint arg1, Boolean arg2)
			: this()
		{
			initChallengeResultMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6019;
		}
		
		public ChallengeResultMessage initChallengeResultMessage(uint arg1 = 0, Boolean arg2 = false)
		{
			this.challengeId = arg1;
			this.success = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.challengeId = 0;
			this.success = false;
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
			this.serializeAs_ChallengeResultMessage(arg1);
		}
		
		public void serializeAs_ChallengeResultMessage(BigEndianWriter arg1)
		{
			if ( this.challengeId < 0 )
			{
				throw new Exception("Forbidden value (" + this.challengeId + ") on element challengeId.");
			}
			arg1.WriteByte((byte)this.challengeId);
			arg1.WriteBoolean(this.success);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ChallengeResultMessage(arg1);
		}
		
		public void deserializeAs_ChallengeResultMessage(BigEndianReader arg1)
		{
			this.challengeId = (uint)arg1.ReadByte();
			if ( this.challengeId < 0 )
			{
				throw new Exception("Forbidden value (" + this.challengeId + ") on element of ChallengeResultMessage.challengeId.");
			}
			this.success = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
