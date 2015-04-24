

// Generated on 04/24/2015 03:38:01
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightStartingMessage : Message
    {
        public const uint Id = 700;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte fightType;
        public int attackerId;
        public int defenderId;
        
        public GameFightStartingMessage()
        {
        }
        
        public GameFightStartingMessage(sbyte fightType, int attackerId, int defenderId)
        {
            this.fightType = fightType;
            this.attackerId = attackerId;
            this.defenderId = defenderId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(fightType);
            writer.WriteInt(attackerId);
            writer.WriteInt(defenderId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            fightType = reader.ReadSByte();
            if (fightType < 0)
                throw new Exception("Forbidden value on fightType = " + fightType + ", it doesn't respect the following condition : fightType < 0");
            attackerId = reader.ReadInt();
            defenderId = reader.ReadInt();
        }
        
    }
    
}