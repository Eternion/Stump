

// Generated on 10/30/2016 16:20:38
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ChallengeFightJoinRefusedMessage : Message
    {
        public const uint Id = 5908;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public long playerId;
        public sbyte reason;
        
        public ChallengeFightJoinRefusedMessage()
        {
        }
        
        public ChallengeFightJoinRefusedMessage(long playerId, sbyte reason)
        {
            this.playerId = playerId;
            this.reason = reason;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarLong(playerId);
            writer.WriteSByte(reason);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            playerId = reader.ReadVarLong();
            if (playerId < 0 || playerId > 9007199254740990)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0 || playerId > 9007199254740990");
            reason = reader.ReadSByte();
        }
        
    }
    
}