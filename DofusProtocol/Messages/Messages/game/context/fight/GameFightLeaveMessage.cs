

// Generated on 09/26/2016 01:49:57
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightLeaveMessage : Message
    {
        public const uint Id = 721;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double charId;
        
        public GameFightLeaveMessage()
        {
        }
        
        public GameFightLeaveMessage(double charId)
        {
            this.charId = charId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(charId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            charId = reader.ReadDouble();
            if (charId < -9007199254740990 || charId > 9007199254740990)
                throw new Exception("Forbidden value on charId = " + charId + ", it doesn't respect the following condition : charId < -9007199254740990 || charId > 9007199254740990");
        }
        
    }
    
}