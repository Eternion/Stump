

// Generated on 11/16/2015 14:25:55
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AchievementRewardRequestMessage : Message
    {
        public const uint Id = 6377;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short achievementId;
        
        public AchievementRewardRequestMessage()
        {
        }
        
        public AchievementRewardRequestMessage(short achievementId)
        {
            this.achievementId = achievementId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(achievementId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            achievementId = reader.ReadShort();
        }
        
    }
    
}