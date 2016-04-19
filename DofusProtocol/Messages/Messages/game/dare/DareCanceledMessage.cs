

// Generated on 04/19/2016 10:17:27
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class DareCanceledMessage : Message
    {
        public const uint Id = 6679;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double dareId;
        
        public DareCanceledMessage()
        {
        }
        
        public DareCanceledMessage(double dareId)
        {
            this.dareId = dareId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(dareId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            dareId = reader.ReadDouble();
            if (dareId < 0 || dareId > 9007199254740990)
                throw new Exception("Forbidden value on dareId = " + dareId + ", it doesn't respect the following condition : dareId < 0 || dareId > 9007199254740990");
        }
        
    }
    
}