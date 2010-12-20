using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameRolePlayPlayerFightFriendlyRequestedMessage : Message
	{
		public const uint protocolId = 5937;
		internal Boolean _isInitialized = false;
		public uint fightId = 0;
		public uint sourceId = 0;
		public uint targetId = 0;
		
		public GameRolePlayPlayerFightFriendlyRequestedMessage()
		{
		}
		
		public GameRolePlayPlayerFightFriendlyRequestedMessage(uint arg1, uint arg2, uint arg3)
			: this()
		{
			initGameRolePlayPlayerFightFriendlyRequestedMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5937;
		}
		
		public GameRolePlayPlayerFightFriendlyRequestedMessage initGameRolePlayPlayerFightFriendlyRequestedMessage(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0)
		{
			this.fightId = arg1;
			this.sourceId = arg2;
			this.targetId = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.fightId = 0;
			this.sourceId = 0;
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
			this.serializeAs_GameRolePlayPlayerFightFriendlyRequestedMessage(arg1);
		}
		
		public void serializeAs_GameRolePlayPlayerFightFriendlyRequestedMessage(BigEndianWriter arg1)
		{
			if ( this.fightId < 0 )
			{
				throw new Exception("Forbidden value (" + this.fightId + ") on element fightId.");
			}
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
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameRolePlayPlayerFightFriendlyRequestedMessage(arg1);
		}
		
		public void deserializeAs_GameRolePlayPlayerFightFriendlyRequestedMessage(BigEndianReader arg1)
		{
			this.fightId = (uint)arg1.ReadInt();
			if ( this.fightId < 0 )
			{
				throw new Exception("Forbidden value (" + this.fightId + ") on element of GameRolePlayPlayerFightFriendlyRequestedMessage.fightId.");
			}
			this.sourceId = (uint)arg1.ReadInt();
			if ( this.sourceId < 0 )
			{
				throw new Exception("Forbidden value (" + this.sourceId + ") on element of GameRolePlayPlayerFightFriendlyRequestedMessage.sourceId.");
			}
			this.targetId = (uint)arg1.ReadInt();
			if ( this.targetId < 0 )
			{
				throw new Exception("Forbidden value (" + this.targetId + ") on element of GameRolePlayPlayerFightFriendlyRequestedMessage.targetId.");
			}
		}
		
	}
}
