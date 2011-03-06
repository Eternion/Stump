using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameRolePlayPlayerFightRequestMessage : Message
	{
		public const uint protocolId = 5731;
		internal Boolean _isInitialized = false;
		public uint targetId = 0;
		public Boolean friendly = false;
		
		public GameRolePlayPlayerFightRequestMessage()
		{
		}
		
		public GameRolePlayPlayerFightRequestMessage(uint arg1, Boolean arg2)
			: this()
		{
			initGameRolePlayPlayerFightRequestMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5731;
		}
		
		public GameRolePlayPlayerFightRequestMessage initGameRolePlayPlayerFightRequestMessage(uint arg1 = 0, Boolean arg2 = false)
		{
			this.targetId = arg1;
			this.friendly = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.targetId = 0;
			this.friendly = false;
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
			this.serializeAs_GameRolePlayPlayerFightRequestMessage(arg1);
		}
		
		public void serializeAs_GameRolePlayPlayerFightRequestMessage(BigEndianWriter arg1)
		{
			if ( this.targetId < 0 )
			{
				throw new Exception("Forbidden value (" + this.targetId + ") on element targetId.");
			}
			arg1.WriteInt((int)this.targetId);
			arg1.WriteBoolean(this.friendly);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameRolePlayPlayerFightRequestMessage(arg1);
		}
		
		public void deserializeAs_GameRolePlayPlayerFightRequestMessage(BigEndianReader arg1)
		{
			this.targetId = (uint)arg1.ReadInt();
			if ( this.targetId < 0 )
			{
				throw new Exception("Forbidden value (" + this.targetId + ") on element of GameRolePlayPlayerFightRequestMessage.targetId.");
			}
			this.friendly = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
