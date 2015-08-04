

// Generated on 08/04/2015 13:25:09
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildFightPlayersHelpersLeaveMessage : Message
    {
        public const uint Id = 5719;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int fightId;
        public int playerId;
        
        public GuildFightPlayersHelpersLeaveMessage()
        {
        }
        
        public GuildFightPlayersHelpersLeaveMessage(int fightId, int playerId)
        {
            this.fightId = fightId;
            this.playerId = playerId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(fightId);
            writer.WriteVarInt(playerId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightId = reader.ReadInt();
            if (fightId < 0)
                throw new Exception("Forbidden value on fightId = " + fightId + ", it doesn't respect the following condition : fightId < 0");
            playerId = reader.ReadVarInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
        }
        
    }
    
}