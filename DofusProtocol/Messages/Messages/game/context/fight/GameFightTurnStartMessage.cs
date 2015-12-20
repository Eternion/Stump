

// Generated on 12/20/2015 16:36:50
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
            if (id < -9.007199254740992E15 || id > 9.007199254740992E15)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < -9.007199254740992E15 || id > 9.007199254740992E15");
            waitTime = reader.ReadVarInt();
            if (waitTime < 0)
                throw new Exception("Forbidden value on waitTime = " + waitTime + ", it doesn't respect the following condition : waitTime < 0");
        }
        
    }
    
}