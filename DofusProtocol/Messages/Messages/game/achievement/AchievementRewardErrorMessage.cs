

// Generated on 04/19/2016 10:17:07
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AchievementRewardErrorMessage : Message
    {
        public const uint Id = 6375;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short achievementId;
        
        public AchievementRewardErrorMessage()
        {
        }
        
        public AchievementRewardErrorMessage(short achievementId)
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