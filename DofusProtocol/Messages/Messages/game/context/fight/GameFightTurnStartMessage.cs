

// Generated on 10/30/2016 16:20:27
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightTurnStartMessage : Message
    {
        public const uint Id = 714;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double id;
        public int waitTime;
        
        public GameFightTurnStartMessage()
        {
        }
        
        public GameFightTurnStartMessage(double id, int waitTime)
        {
            this.id = id;
            this.waitTime = waitTime;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(id);
            writer.WriteVarInt(waitTime);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            id = reader.ReadDouble();
            if (id < -9007199254740990 || id > 9007199254740990)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < -9007199254740990 || id > 9007199254740990");
            waitTime = reader.ReadVarInt();
            if (waitTime < 0)
                throw new Exception("Forbidden value on waitTime = " + waitTime + ", it doesn't respect the following condition : waitTime < 0");
        }
        
    }
    
}