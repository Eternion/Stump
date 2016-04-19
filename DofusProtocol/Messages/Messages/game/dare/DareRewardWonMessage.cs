

// Generated on 04/19/2016 10:17:27
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class DareRewardWonMessage : Message
    {
        public const uint Id = 6678;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.DareReward reward;
        
        public DareRewardWonMessage()
        {
        }
        
        public DareRewardWonMessage(Types.DareReward reward)
        {
            this.reward = reward;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            reward.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            reward = new Types.DareReward();
            reward.Deserialize(reader);
        }
        
    }
    
}