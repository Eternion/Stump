using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class AchievementStartedValue : Achievement
	{
		public const uint protocolId = 361;
		public uint value = 0;
		public uint maxValue = 0;
		
		public AchievementStartedValue()
		{
		}
		
		public AchievementStartedValue(uint arg1, uint arg2, uint arg3)
			: this()
		{
			initAchievementStartedValue(arg1, arg2, arg3);
		}
		
		public override uint getTypeId()
		{
			return 361;
		}
		
		public AchievementStartedValue initAchievementStartedValue(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0)
		{
			base.initAchievement(arg1);
			this.value = arg2;
			this.maxValue = arg3;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.value = 0;
			this.maxValue = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_AchievementStartedValue(arg1);
		}
		
		public void serializeAs_AchievementStartedValue(BigEndianWriter arg1)
		{
			base.serializeAs_Achievement(arg1);
			if ( this.value < 0 )
			{
				throw new Exception("Forbidden value (" + this.value + ") on element value.");
			}
			arg1.WriteShort((short)this.value);
			if ( this.maxValue < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxValue + ") on element maxValue.");
			}
			arg1.WriteShort((short)this.maxValue);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AchievementStartedValue(arg1);
		}
		
		public void deserializeAs_AchievementStartedValue(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.value = (uint)arg1.ReadShort();
			if ( this.value < 0 )
			{
				throw new Exception("Forbidden value (" + this.value + ") on element of AchievementStartedValue.value.");
			}
			this.maxValue = (uint)arg1.ReadShort();
			if ( this.maxValue < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxValue + ") on element of AchievementStartedValue.maxValue.");
			}
		}
		
	}
}
