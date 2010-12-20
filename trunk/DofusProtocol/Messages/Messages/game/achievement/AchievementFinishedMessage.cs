using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class AchievementFinishedMessage : Message
	{
		public const uint protocolId = 6208;
		internal Boolean _isInitialized = false;
		public uint achievementId = 0;
		
		public AchievementFinishedMessage()
		{
		}
		
		public AchievementFinishedMessage(uint arg1)
			: this()
		{
			initAchievementFinishedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6208;
		}
		
		public AchievementFinishedMessage initAchievementFinishedMessage(uint arg1 = 0)
		{
			this.achievementId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.achievementId = 0;
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
			this.serializeAs_AchievementFinishedMessage(arg1);
		}
		
		public void serializeAs_AchievementFinishedMessage(BigEndianWriter arg1)
		{
			if ( this.achievementId < 0 )
			{
				throw new Exception("Forbidden value (" + this.achievementId + ") on element achievementId.");
			}
			arg1.WriteShort((short)this.achievementId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AchievementFinishedMessage(arg1);
		}
		
		public void deserializeAs_AchievementFinishedMessage(BigEndianReader arg1)
		{
			this.achievementId = (uint)arg1.ReadShort();
			if ( this.achievementId < 0 )
			{
				throw new Exception("Forbidden value (" + this.achievementId + ") on element of AchievementFinishedMessage.achievementId.");
			}
		}
		
	}
}
