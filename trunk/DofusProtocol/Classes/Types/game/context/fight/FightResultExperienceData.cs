using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class FightResultExperienceData : FightResultAdditionalData
	{
		public const uint protocolId = 192;
		public double experience = 0;
		public Boolean showExperience = false;
		public double experienceLevelFloor = 0;
		public Boolean showExperienceLevelFloor = false;
		public double experienceNextLevelFloor = 0;
		public Boolean showExperienceNextLevelFloor = false;
		public int experienceFightDelta = 0;
		public Boolean showExperienceFightDelta = false;
		public uint experienceForGuild = 0;
		public Boolean showExperienceForGuild = false;
		public uint experienceForMount = 0;
		public Boolean showExperienceForMount = false;
		
		public FightResultExperienceData()
		{
		}
		
		public FightResultExperienceData(double arg1, Boolean arg2, double arg3, Boolean arg4, double arg5, Boolean arg6, int arg7, Boolean arg8, uint arg9, Boolean arg10, uint arg11, Boolean arg12)
			: this()
		{
			initFightResultExperienceData(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
		}
		
		public override uint getTypeId()
		{
			return 192;
		}
		
		public FightResultExperienceData initFightResultExperienceData(double arg1 = 0, Boolean arg2 = false, double arg3 = 0, Boolean arg4 = false, double arg5 = 0, Boolean arg6 = false, int arg7 = 0, Boolean arg8 = false, uint arg9 = 0, Boolean arg10 = false, uint arg11 = 0, Boolean arg12 = false)
		{
			this.experience = arg1;
			this.showExperience = arg2;
			this.experienceLevelFloor = arg3;
			this.showExperienceLevelFloor = arg4;
			this.experienceNextLevelFloor = arg5;
			this.showExperienceNextLevelFloor = arg6;
			this.experienceFightDelta = arg7;
			this.showExperienceFightDelta = arg8;
			this.experienceForGuild = arg9;
			this.showExperienceForGuild = arg10;
			this.experienceForMount = arg11;
			this.showExperienceForMount = arg12;
			return this;
		}
		
		public override void reset()
		{
			this.experience = 0;
			this.showExperience = false;
			this.experienceLevelFloor = 0;
			this.showExperienceLevelFloor = false;
			this.experienceNextLevelFloor = 0;
			this.showExperienceNextLevelFloor = false;
			this.experienceFightDelta = 0;
			this.showExperienceFightDelta = false;
			this.experienceForGuild = 0;
			this.showExperienceForGuild = false;
			this.experienceForMount = 0;
			this.showExperienceForMount = false;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FightResultExperienceData(arg1);
		}
		
		public void serializeAs_FightResultExperienceData(BigEndianWriter arg1)
		{
			base.serializeAs_FightResultAdditionalData(arg1);
			var loc1 = 0;
			BooleanByteWrapper.SetFlag(loc1, 0, this.showExperience);
			BooleanByteWrapper.SetFlag(loc1, 1, this.showExperienceLevelFloor);
			BooleanByteWrapper.SetFlag(loc1, 2, this.showExperienceNextLevelFloor);
			BooleanByteWrapper.SetFlag(loc1, 3, this.showExperienceFightDelta);
			BooleanByteWrapper.SetFlag(loc1, 4, this.showExperienceForGuild);
			BooleanByteWrapper.SetFlag(loc1, 5, this.showExperienceForMount);
			arg1.WriteByte((byte)loc1);
			if ( this.experience < 0 )
			{
				throw new Exception("Forbidden value (" + this.experience + ") on element experience.");
			}
			arg1.WriteDouble(this.experience);
			if ( this.experienceLevelFloor < 0 )
			{
				throw new Exception("Forbidden value (" + this.experienceLevelFloor + ") on element experienceLevelFloor.");
			}
			arg1.WriteDouble(this.experienceLevelFloor);
			if ( this.experienceNextLevelFloor < 0 )
			{
				throw new Exception("Forbidden value (" + this.experienceNextLevelFloor + ") on element experienceNextLevelFloor.");
			}
			arg1.WriteDouble(this.experienceNextLevelFloor);
			arg1.WriteInt((int)this.experienceFightDelta);
			if ( this.experienceForGuild < 0 )
			{
				throw new Exception("Forbidden value (" + this.experienceForGuild + ") on element experienceForGuild.");
			}
			arg1.WriteInt((int)this.experienceForGuild);
			if ( this.experienceForMount < 0 )
			{
				throw new Exception("Forbidden value (" + this.experienceForMount + ") on element experienceForMount.");
			}
			arg1.WriteInt((int)this.experienceForMount);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FightResultExperienceData(arg1);
		}
		
		public void deserializeAs_FightResultExperienceData(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			var loc1 = arg1.ReadByte();
			this.showExperience = (Boolean)BooleanByteWrapper.GetFlag(loc1, 0);
			this.showExperienceLevelFloor = (Boolean)BooleanByteWrapper.GetFlag(loc1, 1);
			this.showExperienceNextLevelFloor = (Boolean)BooleanByteWrapper.GetFlag(loc1, 2);
			this.showExperienceFightDelta = (Boolean)BooleanByteWrapper.GetFlag(loc1, 3);
			this.showExperienceForGuild = (Boolean)BooleanByteWrapper.GetFlag(loc1, 4);
			this.showExperienceForMount = (Boolean)BooleanByteWrapper.GetFlag(loc1, 5);
			this.experience = (double)arg1.ReadDouble();
			if ( this.experience < 0 )
			{
				throw new Exception("Forbidden value (" + this.experience + ") on element of FightResultExperienceData.experience.");
			}
			this.experienceLevelFloor = (double)arg1.ReadDouble();
			if ( this.experienceLevelFloor < 0 )
			{
				throw new Exception("Forbidden value (" + this.experienceLevelFloor + ") on element of FightResultExperienceData.experienceLevelFloor.");
			}
			this.experienceNextLevelFloor = (double)arg1.ReadDouble();
			if ( this.experienceNextLevelFloor < 0 )
			{
				throw new Exception("Forbidden value (" + this.experienceNextLevelFloor + ") on element of FightResultExperienceData.experienceNextLevelFloor.");
			}
			this.experienceFightDelta = (int)arg1.ReadInt();
			this.experienceForGuild = (uint)arg1.ReadInt();
			if ( this.experienceForGuild < 0 )
			{
				throw new Exception("Forbidden value (" + this.experienceForGuild + ") on element of FightResultExperienceData.experienceForGuild.");
			}
			this.experienceForMount = (uint)arg1.ReadInt();
			if ( this.experienceForMount < 0 )
			{
				throw new Exception("Forbidden value (" + this.experienceForMount + ") on element of FightResultExperienceData.experienceForMount.");
			}
		}
		
	}
}
