

// Generated on 10/26/2014 23:29:14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AchievementDetailsRequestMessage : Message
    {
        public const uint Id = 6380;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short achievementId;
        
        public AchievementDetailsRequestMessage()
        {
        }
        
        public AchievementDetailsRequestMessage(short achievementId)
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
            if (achievementId < 0)
                throw new Exception("Forbidden value on achievementId = " + achievementId + ", it doesn't respect the following condition : achievementId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short);
        }
        
    }
    
}