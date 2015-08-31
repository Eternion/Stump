

// Generated on 08/04/2015 13:24:45
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var finishedAchievementsIds_before = writer.Position;
            var finishedAchievementsIds_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in finishedAchievementsIds)
            {
                 writer.WriteVarShort(entry);
                 finishedAchievementsIds_count++;
            }
            var finishedAchievementsIds_after = writer.Position;
            writer.Seek((int)finishedAchievementsIds_before);
            writer.WriteUShort((ushort)finishedAchievementsIds_count);
            writer.Seek((int)finishedAchievementsIds_after);

            var rewardableAchievements_before = writer.Position;
            var rewardableAchievements_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in rewardableAchievements)
            {
                 entry.Serialize(writer);
                 rewardableAchievements_count++;
            }
            var rewardableAchievements_after = writer.Position;
            writer.Seek((int)rewardableAchievements_before);
            writer.WriteUShort((ushort)rewardableAchievements_count);
            writer.Seek((int)rewardableAchievements_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var finishedAchievementsIds_ = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 finishedAchievementsIds_[i] = reader.ReadVarShort();
            }
            finishedAchievementsIds = finishedAchievementsIds_;
            limit = reader.ReadUShort();
            var rewardableAchievements_ = new Types.AchievementRewardable[limit];
            for (int i = 0; i < limit; i++)
            {
                 rewardableAchievements_[i] = new Types.AchievementRewardable();
                 rewardableAchievements_[i].Deserialize(reader);
            }
            rewardableAchievements = rewardableAchievements_;
        }
        
    }
    
}