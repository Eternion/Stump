

// Generated on 09/01/2014 15:51:49
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AchievementDetailedListMessage : Message
    {
        public const uint Id = 6358;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.Achievement> startedAchievements;
        public IEnumerable<Types.Achievement> finishedAchievements;
        
        public AchievementDetailedListMessage()
        {
        }
        
        public AchievementDetailedListMessage(IEnumerable<Types.Achievement> startedAchievements, IEnumerable<Types.Achievement> finishedAchievements)
        {
            this.startedAchievements = startedAchievements;
            this.finishedAchievements = finishedAchievements;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var startedAchievements_before = writer.Position;
            var startedAchievements_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in startedAchievements)
            {
                 entry.Serialize(writer);
                 startedAchievements_count++;
            }
            var startedAchievements_after = writer.Position;
            writer.Seek((int)startedAchievements_before);
            writer.WriteUShort((ushort)startedAchievements_count);
            writer.Seek((int)startedAchievements_after);

            var finishedAchievements_before = writer.Position;
            var finishedAchievements_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in finishedAchievements)
            {
                 entry.Serialize(writer);
                 finishedAchievements_count++;
            }
            var finishedAchievements_after = writer.Position;
            writer.Seek((int)finishedAchievements_before);
            writer.WriteUShort((ushort)finishedAchievements_count);
            writer.Seek((int)finishedAchievements_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var startedAchievements_ = new Types.Achievement[limit];
            for (int i = 0; i < limit; i++)
            {
                 startedAchievements_[i] = new Types.Achievement();
                 startedAchievements_[i].Deserialize(reader);
            }
            startedAchievements = startedAchievements_;
            limit = reader.ReadUShort();
            var finishedAchievements_ = new Types.Achievement[limit];
            for (int i = 0; i < limit; i++)
            {
                 finishedAchievements_[i] = new Types.Achievement();
                 finishedAchievements_[i].Deserialize(reader);
            }
            finishedAchievements = finishedAchievements_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + startedAchievements.Sum(x => x.GetSerializationSize()) + sizeof(short) + finishedAchievements.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}