
// Generated on 03/25/2013 19:23:59
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AchievementRewardSuccessMessage : Message
    {
        public const uint Id = 6376;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short achievementId;
        
        public AchievementRewardSuccessMessage()
        {
        }
        
        public AchievementRewardSuccessMessage(short achievementId)
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