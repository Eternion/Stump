using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameRolePlayPlayerFightFriendlyAnsweredMessage : Message
	{
		public const uint protocolId = 5733;
		internal Boolean _isInitialized = false;
		public int fightId = 0;
		public uint sourceId = 0;
		public uint targetId = 0;
		public Boolean accept = false;
		
		public GameRolePlayPlayerFightFriendlyAnsweredMessage()
		{
		}
		
		public GameRolePlayPlayerFightFriendlyAnsweredMessage(int arg1, uint arg2, uint arg3, Boolean arg4)
			: this()
		{
			initGameRolePlayPlayerFightFriendlyAnsweredMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 5733;
		}
		
		public GameRolePlayPlayerFightFriendlyAnsweredMessage initGameRolePlayPlayerFightFriendlyAnsweredMessage(int arg1 = 0, uint arg2 = 0, uint arg3 = 0, Boolean arg4 = false)
		{
			this.fightId = arg1;
			this.sourceId = arg2;
			this.targetId = arg3;
			this.accept = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.fightId = 0;
			this.sourceId = 0;
			this.targetId = 0;
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
			this.serializeAs_GameRolePlayPlayerFightFriendlyAnsweredMessage(arg1);
		}
		
		public void serializeAs_GameRolePlayPlayerFightFriendlyAnsweredMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.fightId);
			if ( this.sourceId < 0 )
			{
				throw new Exception("Forbidden value (" + this.sourceId + ") on element sourceId.");
			}
			arg1.WriteInt((int)this.sourceId);
			if ( this.targetId < 0 )
			{
				throw new Exception("Forbidden value (" + this.targetId + ") on element targetId.");
			}
			arg1.WriteInt((int)this.targetId);
			arg1.WriteBoolean(this.accept);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameRolePlayPlayerFightFriendlyAnsweredMessage(arg1);
		}
		
		public void deserializeAs_GameRolePlayPlayerFightFriendlyAnsweredMessage(BigEndianReader arg1)
		{
			this.fightId = (int)arg1.ReadInt();
			this.sourceId = (uint)arg1.ReadInt();
			if ( this.sourceId < 0 )
			{
				throw new Exception("Forbidden value (" + this.sourceId + ") on element of GameRolePlayPlayerFightFriendlyAnsweredMessage.sourceId.");
			}
			this.targetId = (uint)arg1.ReadInt();
			if ( this.targetId < 0 )
			{
				throw new Exception("Forbidden value (" + this.targetId + ") on element of GameRolePlayPlayerFightFriendlyAnsweredMessage.targetId.");
			}
			this.accept = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
