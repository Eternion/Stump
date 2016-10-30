

// Generated on 10/30/2016 16:20:30
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameRolePlayArenaFighterStatusMessage : Message
    {
        public const uint Id = 6281;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int fightId;
        public int playerId;
        public bool accepted;
        
        public GameRolePlayArenaFighterStatusMessage()
        {
        }
        
        public GameRolePlayArenaFighterStatusMessage(int fightId, int playerId, bool accepted)
        {
            this.fightId = fightId;
            this.playerId = playerId;
            this.accepted = accepted;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(fightId);
            writer.WriteInt(playerId);
            writer.WriteBoolean(accepted);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightId = reader.ReadInt();
            playerId = reader.ReadInt();
            accepted = reader.ReadBoolean();
        }
        
    }
    
}