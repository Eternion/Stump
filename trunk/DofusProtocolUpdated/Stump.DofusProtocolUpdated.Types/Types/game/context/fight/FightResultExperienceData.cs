

// Generated on 12/12/2013 16:57:29
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
        
        public double experience;
        public double experienceLevelFloor;
        public double experienceNextLevelFloor;
        public int experienceFightDelta;
        public int experienceForGuild;
        public int experienceForMount;
        public int rerollExperienceMul;
        
        public FightResultExperienceData()
        {
        }
        
        public FightResultExperienceData(double experience, double experienceLevelFloor, double experienceNextLevelFloor, int experienceFightDelta, int experienceForGuild, int experienceForMount, int rerollExperienceMul)
        {
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
            writer.WriteDouble(experience);
            writer.WriteDouble(experienceLevelFloor);
            writer.WriteDouble(experienceNextLevelFloor);
            writer.WriteInt(experienceFightDelta);
            writer.WriteInt(experienceForGuild);
            writer.WriteInt(experienceForMount);
            writer.WriteInt(rerollExperienceMul);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            experience = reader.ReadDouble();
            if (experience < 0)
                throw new Exception("Forbidden value on experience = " + experience + ", it doesn't respect the following condition : experience < 0");
            experienceLevelFloor = reader.ReadDouble();
            if (experienceLevelFloor < 0)
                throw new Exception("Forbidden value on experienceLevelFloor = " + experienceLevelFloor + ", it doesn't respect the following condition : experienceLevelFloor < 0");
            experienceNextLevelFloor = reader.ReadDouble();
            if (experienceNextLevelFloor < 0)
                throw new Exception("Forbidden value on experienceNextLevelFloor = " + experienceNextLevelFloor + ", it doesn't respect the following condition : experienceNextLevelFloor < 0");
            experienceFightDelta = reader.ReadInt();
            experienceForGuild = reader.ReadInt();
            if (experienceForGuild < 0)
                throw new Exception("Forbidden value on experienceForGuild = " + experienceForGuild + ", it doesn't respect the following condition : experienceForGuild < 0");
            experienceForMount = reader.ReadInt();
            if (experienceForMount < 0)
                throw new Exception("Forbidden value on experienceForMount = " + experienceForMount + ", it doesn't respect the following condition : experienceForMount < 0");
            rerollExperienceMul = reader.ReadInt();
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(double) + sizeof(double) + sizeof(double) + sizeof(int) + sizeof(int) + sizeof(int) + sizeof(int);
        }
        
    }
    
}