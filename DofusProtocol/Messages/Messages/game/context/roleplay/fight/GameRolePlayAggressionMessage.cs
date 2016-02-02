

// Generated on 02/02/2016 14:14:16
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameRolePlayAggressionMessage : Message
    {
        public const uint Id = 6073;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public long attackerId;
        public long defenderId;
        
        public GameRolePlayAggressionMessage()
        {
        }
        
        public GameRolePlayAggressionMessage(long attackerId, long defenderId)
        {
            this.attackerId = attackerId;
            this.defenderId = defenderId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarLong(attackerId);
            writer.WriteVarLong(defenderId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            attackerId = reader.ReadVarLong();
            if (attackerId < 0 || attackerId > 9.007199254740992E15)
                throw new Exception("Forbidden value on attackerId = " + attackerId + ", it doesn't respect the following condition : attackerId < 0 || attackerId > 9.007199254740992E15");
            defenderId = reader.ReadVarLong();
            if (defenderId < 0 || defenderId > 9.007199254740992E15)
                throw new Exception("Forbidden value on defenderId = " + defenderId + ", it doesn't respect the following condition : defenderId < 0 || defenderId > 9.007199254740992E15");
        }
        
    }
    
}