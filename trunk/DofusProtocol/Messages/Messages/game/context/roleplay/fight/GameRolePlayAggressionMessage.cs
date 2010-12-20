using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameRolePlayAggressionMessage : Message
	{
		public const uint protocolId = 6073;
		internal Boolean _isInitialized = false;
		public uint attackerId = 0;
		public uint defenderId = 0;
		
		public GameRolePlayAggressionMessage()
		{
		}
		
		public GameRolePlayAggressionMessage(uint arg1, uint arg2)
			: this()
		{
			initGameRolePlayAggressionMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6073;
		}
		
		public GameRolePlayAggressionMessage initGameRolePlayAggressionMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.attackerId = arg1;
			this.defenderId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.attackerId = 0;
			this.defenderId = 0;
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
			this.serializeAs_GameRolePlayAggressionMessage(arg1);
		}
		
		public void serializeAs_GameRolePlayAggressionMessage(BigEndianWriter arg1)
		{
			if ( this.attackerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.attackerId + ") on element attackerId.");
			}
			arg1.WriteInt((int)this.attackerId);
			if ( this.defenderId < 0 )
			{
				throw new Exception("Forbidden value (" + this.defenderId + ") on element defenderId.");
			}
			arg1.WriteInt((int)this.defenderId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameRolePlayAggressionMessage(arg1);
		}
		
		public void deserializeAs_GameRolePlayAggressionMessage(BigEndianReader arg1)
		{
			this.attackerId = (uint)arg1.ReadInt();
			if ( this.attackerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.attackerId + ") on element of GameRolePlayAggressionMessage.attackerId.");
			}
			this.defenderId = (uint)arg1.ReadInt();
			if ( this.defenderId < 0 )
			{
				throw new Exception("Forbidden value (" + this.defenderId + ") on element of GameRolePlayAggressionMessage.defenderId.");
			}
		}
		
	}
}
