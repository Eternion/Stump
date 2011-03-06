using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameRolePlayPlayerFightFriendlyAnswerMessage : Message
	{
		public const uint protocolId = 5732;
		internal Boolean _isInitialized = false;
		public int fightId = 0;
		public Boolean accept = false;
		
		public GameRolePlayPlayerFightFriendlyAnswerMessage()
		{
		}
		
		public GameRolePlayPlayerFightFriendlyAnswerMessage(int arg1, Boolean arg2)
			: this()
		{
			initGameRolePlayPlayerFightFriendlyAnswerMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5732;
		}
		
		public GameRolePlayPlayerFightFriendlyAnswerMessage initGameRolePlayPlayerFightFriendlyAnswerMessage(int arg1 = 0, Boolean arg2 = false)
		{
			this.fightId = arg1;
			this.accept = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.fightId = 0;
			this.accept = false;
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
			this.serializeAs_GameRolePlayPlayerFightFriendlyAnswerMessage(arg1);
		}
		
		public void serializeAs_GameRolePlayPlayerFightFriendlyAnswerMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.fightId);
			arg1.WriteBoolean(this.accept);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameRolePlayPlayerFightFriendlyAnswerMessage(arg1);
		}
		
		public void deserializeAs_GameRolePlayPlayerFightFriendlyAnswerMessage(BigEndianReader arg1)
		{
			this.fightId = (int)arg1.ReadInt();
			this.accept = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
