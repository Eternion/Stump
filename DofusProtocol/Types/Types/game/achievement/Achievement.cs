

// Generated on 04/19/2016 10:17:42
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class Achievement
    {
        public const short Id = 363;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short id;
        public IEnumerable<Types.AchievementObjective> finishedObjective;
        public IEnumerable<Types.AchievementStartedObjective> startedObjectives;
        
        public Achievement()
        {
        }
        
        public Achievement(short id, IEnumerable<Types.AchievementObjective> finishedObjective, IEnumerable<Types.AchievementStartedObjective> startedObjectives)
        {
            this.id = id;
            this.finishedObjective = finishedObjective;
            this.startedObjectives = startedObjectives;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(id);
            var finishedObjective_before = writer.Position;
            var finishedObjective_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in finishedObjective)
            {
                 entry.Serialize(writer);
                 finishedObjective_count++;
            }
            var finishedObjective_after = writer.Position;
            writer.Seek((int)finishedObjective_before);
            writer.WriteUShort((ushort)finishedObjective_count);
            writer.Seek((int)finishedObjective_after);

            var startedObjectives_before = writer.Position;
            var startedObjectives_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in startedObjectives)
            {
                 entry.Serialize(writer);
                 startedObjectives_count++;
            }
            var startedObjectives_after = writer.Position;
            writer.Seek((int)startedObjectives_before);
            writer.WriteUShort((ushort)startedObjectives_count);
            writer.Seek((int)startedObjectives_after);

        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            id = reader.ReadVarShort();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
            var limit = reader.ReadUShort();
            var finishedObjective_ = new Types.AchievementObjective[limit];
            for (int i = 0; i < limit; i++)
            {
                 finishedObjective_[i] = new Types.AchievementObjective();
                 finishedObjective_[i].Deserialize(reader);
            }
            finishedObjective = finishedObjective_;
            limit = reader.ReadUShort();
            var startedObjectives_ = new Types.AchievementStartedObjective[limit];
            for (int i = 0; i < limit; i++)
            {
                 startedObjectives_[i] = new Types.AchievementStartedObjective();
                 startedObjectives_[i].Deserialize(reader);
            }
            startedObjectives = startedObjectives_;
        }
        
        
    }
    
}