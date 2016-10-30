

// Generated on 10/30/2016 16:20:53
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class FightResultExperienceData : FightResultAdditionalData
    {
        public const short Id = 192;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public bool showExperience;
        public bool showExperienceLevelFloor;
        public bool showExperienceNextLevelFloor;
        public bool showExperienceFightDelta;
        public bool showExperienceForGuild;
        public bool showExperienceForMount;
        public bool isIncarnationExperience;
        public long experience;
        public long experienceLevelFloor;
        public long experienceNextLevelFloor;
        public long experienceFightDelta;
        public long experienceForGuild;
        public long experienceForMount;
        public sbyte rerollExperienceMul;
        
        public FightResultExperienceData()
        {
        }
        
        public FightResultExperienceData(bool showExperience, bool showExperienceLevelFloor, bool showExperienceNextLevelFloor, bool showExperienceFightDelta, bool showExperienceForGuild, bool showExperienceForMount, bool isIncarnationExperience, long experience, long experienceLevelFloor, long experienceNextLevelFloor, long experienceFightDelta, long experienceForGuild, long experienceForMount, sbyte rerollExperienceMul)
        {
            this.showExperience = showExperience;
            this.showExperienceLevelFloor = showExperienceLevelFloor;
            this.showExperienceNextLevelFloor = showExperienceNextLevelFloor;
            this.showExperienceFightDelta = showExperienceFightDelta;
            this.showExperienceForGuild = showExperienceForGuild;
            this.showExperienceForMount = showExperienceForMount;
            this.isIncarnationExperience = isIncarnationExperience;
            this.experience = experience;
            this.experienceLevelFloor = experienceLevelFloor;
            this.experienceNextLevelFloor = experienceNextLevelFloor;
            this.experienceFightDelta = experienceFightDelta;
            this.experienceForGuild = experienceForGuild;
            this.experienceForMount = experienceForMount;
            this.rerollExperienceMul = rerollExperienceMul;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, showExperience);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, showExperienceLevelFloor);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 2, showExperienceNextLevelFloor);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 3, showExperienceFightDelta);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 4, showExperienceForGuild);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 5, showExperienceForMount);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 6, isIncarnationExperience);
            writer.WriteByte(flag1);
            writer.WriteVarLong(experience);
            writer.WriteVarLong(experienceLevelFloor);
            writer.WriteVarLong(experienceNextLevelFloor);
            writer.WriteVarLong(experienceFightDelta);
            writer.WriteVarLong(experienceForGuild);
            writer.WriteVarLong(experienceForMount);
            writer.WriteSByte(rerollExperienceMul);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            byte flag1 = reader.ReadByte();
            showExperience = BooleanByteWrapper.GetFlag(flag1, 0);
            showExperienceLevelFloor = BooleanByteWrapper.GetFlag(flag1, 1);
            showExperienceNextLevelFloor = BooleanByteWrapper.GetFlag(flag1, 2);
            showExperienceFightDelta = BooleanByteWrapper.GetFlag(flag1, 3);
            showExperienceForGuild = BooleanByteWrapper.GetFlag(flag1, 4);
            showExperienceForMount = BooleanByteWrapper.GetFlag(flag1, 5);
            isIncarnationExperience = BooleanByteWrapper.GetFlag(flag1, 6);
            experience = reader.ReadVarLong();
            if (experience < 0 || experience > 9007199254740990)
                throw new Exception("Forbidden value on experience = " + experience + ", it doesn't respect the following condition : experience < 0 || experience > 9007199254740990");
            experienceLevelFloor = reader.ReadVarLong();
            if (experienceLevelFloor < 0 || experienceLevelFloor > 9007199254740990)
                throw new Exception("Forbidden value on experienceLevelFloor = " + experienceLevelFloor + ", it doesn't respect the following condition : experienceLevelFloor < 0 || experienceLevelFloor > 9007199254740990");
            experienceNextLevelFloor = reader.ReadVarLong();
            if (experienceNextLevelFloor < 0 || experienceNextLevelFloor > 9007199254740990)
                throw new Exception("Forbidden value on experienceNextLevelFloor = " + experienceNextLevelFloor + ", it doesn't respect the following condition : experienceNextLevelFloor < 0 || experienceNextLevelFloor > 9007199254740990");
            experienceFightDelta = reader.ReadVarLong();
            if (experienceFightDelta < 0 || experienceFightDelta > 9007199254740990)
                throw new Exception("Forbidden value on experienceFightDelta = " + experienceFightDelta + ", it doesn't respect the following condition : experienceFightDelta < 0 || experienceFightDelta > 9007199254740990");
            experienceForGuild = reader.ReadVarLong();
            if (experienceForGuild < 0 || experienceForGuild > 9007199254740990)
                throw new Exception("Forbidden value on experienceForGuild = " + experienceForGuild + ", it doesn't respect the following condition : experienceForGuild < 0 || experienceForGuild > 9007199254740990");
            experienceForMount = reader.ReadVarLong();
            if (experienceForMount < 0 || experienceForMount > 9007199254740990)
                throw new Exception("Forbidden value on experienceForMount = " + experienceForMount + ", it doesn't respect the following condition : experienceForMount < 0 || experienceForMount > 9007199254740990");
            rerollExperienceMul = reader.ReadSByte();
            if (rerollExperienceMul < 0)
                throw new Exception("Forbidden value on rerollExperienceMul = " + rerollExperienceMul + ", it doesn't respect the following condition : rerollExperienceMul < 0");
        }
        
        
    }
    
}