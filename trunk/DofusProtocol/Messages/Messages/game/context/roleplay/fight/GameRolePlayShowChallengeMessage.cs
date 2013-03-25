
// Generated on 03/25/2013 19:24:10
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameRolePlayShowChallengeMessage : Message
    {
        public const uint Id = 301;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.FightCommonInformations commonsInfos;
        
        public GameRolePlayShowChallengeMessage()
        {
        }
        
        public GameRolePlayShowChallengeMessage(Types.FightCommonInformations commonsInfos)
        {
            this.commonsInfos = commonsInfos;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            commonsInfos.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            commonsInfos = new Types.FightCommonInformations();
            commonsInfos.Deserialize(reader);
        }
        
    }
    
}