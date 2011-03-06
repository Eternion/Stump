using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameRolePlayFightRequestCanceledMessage : Message
	{
		public const uint protocolId = 5822;
		internal Boolean _isInitialized = false;
		public int fightId = 0;
		public uint sourceId = 0;
		public int targetId = 0;
		
		public GameRolePlayFightRequestCanceledMessage()
		{
		}
		
		public GameRolePlayFightRequestCanceledMessage(int arg1, uint arg2, int arg3)
			: this()
		{
			initGameRolePlayFightRequestCanceledMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5822;
		}
		
		public GameRolePlayFightRequestCanceledMessage initGameRolePlayFightRequestCanceledMessage(int arg1 = 0, uint arg2 = 0, int arg3 = 0)
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
			this.serializeAs_GameRolePlayFightRequestCanceledMessage(arg1);
		}
		
		public void serializeAs_GameRolePlayFightRequestCanceledMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.fightId);
			if ( this.sourceId < 0 )
			{
				throw new Exception("Forbidden value (" + this.sourceId + ") on element sourceId.");
			}
			arg1.WriteInt((int)this.sourceId);
			arg1.WriteInt((int)this.targetId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameRolePlayFightRequestCanceledMessage(arg1);
		}
		
		public void deserializeAs_GameRolePlayFightRequestCanceledMessage(BigEndianReader arg1)
		{
			this.fightId = (int)arg1.ReadInt();
			this.sourceId = (uint)arg1.ReadInt();
			if ( this.sourceId < 0 )
			{
				throw new Exception("Forbidden value (" + this.sourceId + ") on element of GameRolePlayFightRequestCanceledMessage.sourceId.");
			}
			this.targetId = (int)arg1.ReadInt();
		}
		
	}
}
