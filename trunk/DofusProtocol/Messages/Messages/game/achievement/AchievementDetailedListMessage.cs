

// Generated on 07/29/2013 23:07:29
using System;
using System.Collections.Generic;
using System.Linq;
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
            writer.WriteUShort((ushort)startedAchievements.Count());
            foreach (var entry in startedAchievements)
            {
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)finishedAchievements.Count());
            foreach (var entry in finishedAchievements)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            startedAchievements = new Types.Achievement[limit];
            for (int i = 0; i < limit; i++)
            {
                 (startedAchievements as Types.Achievement[])[i] = new Types.Achievement();
                 (startedAchievements as Types.Achievement[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            finishedAchievements = new Types.Achievement[limit];
            for (int i = 0; i < limit; i++)
            {
                 (finishedAchievements as Types.Achievement[])[i] = new Types.Achievement();
                 (finishedAchievements as Types.Achievement[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + startedAchievements.Sum(x => x.GetSerializationSize()) + sizeof(short) + finishedAchievements.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}