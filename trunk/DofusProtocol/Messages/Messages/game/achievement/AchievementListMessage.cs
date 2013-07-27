

// Generated on 07/26/2013 22:50:49
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AchievementListMessage : Message
    {
        public const uint Id = 6205;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<short> finishedAchievementsIds;
        public IEnumerable<Types.AchievementRewardable> rewardableAchievements;
        
        public AchievementListMessage()
        {
        }
        
        public AchievementListMessage(IEnumerable<short> finishedAchievementsIds, IEnumerable<Types.AchievementRewardable> rewardableAchievements)
        {
            this.finishedAchievementsIds = finishedAchievementsIds;
            this.rewardableAchievements = rewardableAchievements;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)finishedAchievementsIds.Count());
            foreach (var entry in finishedAchievementsIds)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)rewardableAchievements.Count());
            foreach (var entry in rewardableAchievements)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            finishedAchievementsIds = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (finishedAchievementsIds as short[])[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            rewardableAchievements = new Types.AchievementRewardable[limit];
            for (int i = 0; i < limit; i++)
            {
                 (rewardableAchievements as Types.AchievementRewardable[])[i] = new Types.AchievementRewardable();
                 (rewardableAchievements as Types.AchievementRewardable[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + finishedAchievementsIds.Sum(x => sizeof(short)) + sizeof(short) + rewardableAchievements.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}