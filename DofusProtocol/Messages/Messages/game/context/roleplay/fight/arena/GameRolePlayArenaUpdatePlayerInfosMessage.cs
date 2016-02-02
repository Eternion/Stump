

// Generated on 02/02/2016 14:14:17
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameRolePlayArenaUpdatePlayerInfosMessage : Message
    {
        public const uint Id = 6301;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.ArenaRankInfos solo;
        
        public GameRolePlayArenaUpdatePlayerInfosMessage()
        {
        }
        
        public GameRolePlayArenaUpdatePlayerInfosMessage(Types.ArenaRankInfos solo)
        {
            this.solo = solo;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            solo.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            solo = new Types.ArenaRankInfos();
            solo.Deserialize(reader);
        }
        
    }
    
}