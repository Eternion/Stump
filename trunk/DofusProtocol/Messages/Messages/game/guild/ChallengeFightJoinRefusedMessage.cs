using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ChallengeFightJoinRefusedMessage : Message
	{
		public const uint protocolId = 5908;
		internal Boolean _isInitialized = false;
		public uint playerId = 0;
		public int reason = 0;
		
		public ChallengeFightJoinRefusedMessage()
		{
		}
		
		public ChallengeFightJoinRefusedMessage(uint arg1, int arg2)
			: this()
		{
			initChallengeFightJoinRefusedMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5908;
		}
		
		public ChallengeFightJoinRefusedMessage initChallengeFightJoinRefusedMessage(uint arg1 = 0, int arg2 = 0)
		{
			this.playerId = arg1;
			this.reason = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.playerId = 0;
			this.reason = 0;
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
			this.serializeAs_ChallengeFightJoinRefusedMessage(arg1);
		}
		
		public void serializeAs_ChallengeFightJoinRefusedMessage(BigEndianWriter arg1)
		{
			if ( this.playerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.playerId + ") on element playerId.");
			}
			arg1.WriteInt((int)this.playerId);
			arg1.WriteByte((byte)this.reason);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ChallengeFightJoinRefusedMessage(arg1);
		}
		
		public void deserializeAs_ChallengeFightJoinRefusedMessage(BigEndianReader arg1)
		{
			this.playerId = (uint)arg1.ReadInt();
			if ( this.playerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.playerId + ") on element of ChallengeFightJoinRefusedMessage.playerId.");
			}
			this.reason = (int)arg1.ReadByte();
		}
		
	}
}
