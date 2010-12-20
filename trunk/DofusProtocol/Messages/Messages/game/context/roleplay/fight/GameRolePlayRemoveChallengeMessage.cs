using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameRolePlayRemoveChallengeMessage : Message
	{
		public const uint protocolId = 300;
		internal Boolean _isInitialized = false;
		public int fightId = 0;
		
		public GameRolePlayRemoveChallengeMessage()
		{
		}
		
		public GameRolePlayRemoveChallengeMessage(int arg1)
			: this()
		{
			initGameRolePlayRemoveChallengeMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 300;
		}
		
		public GameRolePlayRemoveChallengeMessage initGameRolePlayRemoveChallengeMessage(int arg1 = 0)
		{
			this.fightId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.fightId = 0;
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
			this.serializeAs_GameRolePlayRemoveChallengeMessage(arg1);
		}
		
		public void serializeAs_GameRolePlayRemoveChallengeMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.fightId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameRolePlayRemoveChallengeMessage(arg1);
		}
		
		public void deserializeAs_GameRolePlayRemoveChallengeMessage(BigEndianReader arg1)
		{
			this.fightId = (int)arg1.ReadInt();
		}
		
	}
}
