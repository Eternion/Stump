using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameFightShowFighterRandomStaticPoseMessage : GameFightShowFighterMessage
	{
		public const uint protocolId = 6218;
		internal Boolean _isInitialized = false;
		
		public GameFightShowFighterRandomStaticPoseMessage()
		{
		}
		
		public GameFightShowFighterRandomStaticPoseMessage(GameFightFighterInformations arg1)
			: this()
		{
			initGameFightShowFighterRandomStaticPoseMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6218;
		}
		
		public GameFightShowFighterRandomStaticPoseMessage initGameFightShowFighterRandomStaticPoseMessage(GameFightFighterInformations arg1 = null)
		{
			base.initGameFightShowFighterMessage(arg1);
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
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
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameFightShowFighterRandomStaticPoseMessage(arg1);
		}
		
		public void serializeAs_GameFightShowFighterRandomStaticPoseMessage(BigEndianWriter arg1)
		{
			base.serializeAs_GameFightShowFighterMessage(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightShowFighterRandomStaticPoseMessage(arg1);
		}
		
		public void deserializeAs_GameFightShowFighterRandomStaticPoseMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
		}
		
	}
}
