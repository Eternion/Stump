using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class AchievementUpdateMessage : Message
	{
		public const uint protocolId = 6207;
		internal Boolean _isInitialized = false;
		public Achievement achievement;
		
		public AchievementUpdateMessage()
		{
			this.achievement = new Achievement();
		}
		
		public AchievementUpdateMessage(Achievement arg1)
			: this()
		{
			initAchievementUpdateMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6207;
		}
		
		public AchievementUpdateMessage initAchievementUpdateMessage(Achievement arg1 = null)
		{
			this.achievement = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.achievement = new Achievement();
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
			this.serializeAs_AchievementUpdateMessage(arg1);
		}
		
		public void serializeAs_AchievementUpdateMessage(BigEndianWriter arg1)
		{
			this.achievement.serializeAs_Achievement(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AchievementUpdateMessage(arg1);
		}
		
		public void deserializeAs_AchievementUpdateMessage(BigEndianReader arg1)
		{
			this.achievement = new Achievement();
			this.achievement.deserialize(arg1);
		}
		
	}
}
