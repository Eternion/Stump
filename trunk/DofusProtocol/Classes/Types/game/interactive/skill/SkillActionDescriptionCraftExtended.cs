using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class SkillActionDescriptionCraftExtended : SkillActionDescriptionCraft
	{
		public const uint protocolId = 104;
		public uint thresholdSlots = 0;
		public uint optimumProbability = 0;
		
		public SkillActionDescriptionCraftExtended()
		{
		}
		
		public SkillActionDescriptionCraftExtended(uint arg1, uint arg2, uint arg3, uint arg4, uint arg5)
			: this()
		{
			initSkillActionDescriptionCraftExtended(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getTypeId()
		{
			return 104;
		}
		
		public SkillActionDescriptionCraftExtended initSkillActionDescriptionCraftExtended(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0, uint arg4 = 0, uint arg5 = 0)
		{
			base.initSkillActionDescriptionCraft(arg1, arg2, arg3);
			this.thresholdSlots = arg4;
			this.optimumProbability = arg5;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.thresholdSlots = 0;
			this.optimumProbability = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_SkillActionDescriptionCraftExtended(arg1);
		}
		
		public void serializeAs_SkillActionDescriptionCraftExtended(BigEndianWriter arg1)
		{
			base.serializeAs_SkillActionDescriptionCraft(arg1);
			if ( this.thresholdSlots < 0 )
			{
				throw new Exception("Forbidden value (" + this.thresholdSlots + ") on element thresholdSlots.");
			}
			arg1.WriteByte((byte)this.thresholdSlots);
			if ( this.optimumProbability < 0 )
			{
				throw new Exception("Forbidden value (" + this.optimumProbability + ") on element optimumProbability.");
			}
			arg1.WriteByte((byte)this.optimumProbability);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_SkillActionDescriptionCraftExtended(arg1);
		}
		
		public void deserializeAs_SkillActionDescriptionCraftExtended(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.thresholdSlots = (uint)arg1.ReadByte();
			if ( this.thresholdSlots < 0 )
			{
				throw new Exception("Forbidden value (" + this.thresholdSlots + ") on element of SkillActionDescriptionCraftExtended.thresholdSlots.");
			}
			this.optimumProbability = (uint)arg1.ReadByte();
			if ( this.optimumProbability < 0 )
			{
				throw new Exception("Forbidden value (" + this.optimumProbability + ") on element of SkillActionDescriptionCraftExtended.optimumProbability.");
			}
		}
		
	}
}
