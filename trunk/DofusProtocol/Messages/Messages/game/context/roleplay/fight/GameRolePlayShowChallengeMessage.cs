using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameRolePlayShowChallengeMessage : Message
	{
		public const uint protocolId = 301;
		internal Boolean _isInitialized = false;
		public FightCommonInformations commonsInfos;
		
		public GameRolePlayShowChallengeMessage()
		{
			this.commonsInfos = new FightCommonInformations();
		}
		
		public GameRolePlayShowChallengeMessage(FightCommonInformations arg1)
			: this()
		{
			initGameRolePlayShowChallengeMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 301;
		}
		
		public GameRolePlayShowChallengeMessage initGameRolePlayShowChallengeMessage(FightCommonInformations arg1 = null)
		{
			this.commonsInfos = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.commonsInfos = new FightCommonInformations();
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
			this.serializeAs_GameRolePlayShowChallengeMessage(arg1);
		}
		
		public void serializeAs_GameRolePlayShowChallengeMessage(BigEndianWriter arg1)
		{
			this.commonsInfos.serializeAs_FightCommonInformations(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameRolePlayShowChallengeMessage(arg1);
		}
		
		public void deserializeAs_GameRolePlayShowChallengeMessage(BigEndianReader arg1)
		{
			this.commonsInfos = new FightCommonInformations();
			this.commonsInfos.deserialize(arg1);
		}
		
	}
}
