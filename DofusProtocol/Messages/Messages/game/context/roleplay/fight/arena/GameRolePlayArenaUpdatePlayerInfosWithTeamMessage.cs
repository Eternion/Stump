

// Generated on 12/20/2015 16:36:52
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameRolePlayArenaUpdatePlayerInfosWithTeamMessage : GameRolePlayArenaUpdatePlayerInfosMessage
    {
        public const uint Id = 6640;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.ArenaRankInfos team;
        
        public GameRolePlayArenaUpdatePlayerInfosWithTeamMessage()
        {
        }
        
        public GameRolePlayArenaUpdatePlayerInfosWithTeamMessage(Types.ArenaRankInfos solo, Types.ArenaRankInfos team)
         : base(solo)
        {
            this.team = team;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            team.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            team = new Types.ArenaRankInfos();
            team.Deserialize(reader);
        }
        
    }
    
}