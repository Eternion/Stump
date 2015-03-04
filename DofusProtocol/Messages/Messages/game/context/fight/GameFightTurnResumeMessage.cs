

// Generated on 02/19/2015 12:09:29
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightTurnResumeMessage : GameFightTurnStartMessage
    {
        public const uint Id = 6307;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int remainingTime;
        
        public GameFightTurnResumeMessage()
        {
        }
        
        public GameFightTurnResumeMessage(int id, int waitTime, int remainingTime)
         : base(id, waitTime)
        {
            this.remainingTime = remainingTime;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteVarInt(remainingTime);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            remainingTime = reader.ReadVarInt();
            if (remainingTime < 0)
                throw new Exception("Forbidden value on remainingTime = " + remainingTime + ", it doesn't respect the following condition : remainingTime < 0");
        }
        
    }
    
}