using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class AchievementStartedPercent : Achievement
	{
		public const uint protocolId = 362;
		public uint completionPercent = 0;
		
		public AchievementStartedPercent()
		{
		}
		
		public AchievementStartedPercent(uint arg1, uint arg2)
			: this()
		{
			initAchievementStartedPercent(arg1, arg2);
		}
		
		public override uint getTypeId()
		{
			return 362;
		}
		
		public AchievementStartedPercent initAchievementStartedPercent(uint arg1 = 0, uint arg2 = 0)
		{
			base.initAchievement(arg1);
			this.completionPercent = arg2;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.completionPercent = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_AchievementStartedPercent(arg1);
		}
		
		public void serializeAs_AchievementStartedPercent(BigEndianWriter arg1)
		{
			base.serializeAs_Achievement(arg1);
			if ( this.completionPercent < 0 )
			{
				throw new Exception("Forbidden value (" + this.completionPercent + ") on element completionPercent.");
			}
			arg1.WriteByte((byte)this.completionPercent);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AchievementStartedPercent(arg1);
		}
		
		public void deserializeAs_AchievementStartedPercent(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.completionPercent = (uint)arg1.ReadByte();
			if ( this.completionPercent < 0 )
			{
				throw new Exception("Forbidden value (" + this.completionPercent + ") on element of AchievementStartedPercent.completionPercent.");
			}
		}
		
	}
}
