

// Generated on 09/26/2016 01:50:06
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class DareSubscribeRequestMessage : Message
    {
        public const uint Id = 6666;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double dareId;
        public bool subscribe;
        
        public DareSubscribeRequestMessage()
        {
        }
        
        public DareSubscribeRequestMessage(double dareId, bool subscribe)
        {
            this.dareId = dareId;
            this.subscribe = subscribe;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(dareId);
            writer.WriteBoolean(subscribe);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            dareId = reader.ReadDouble();
            if (dareId < 0 || dareId > 9007199254740990)
                throw new Exception("Forbidden value on dareId = " + dareId + ", it doesn't respect the following condition : dareId < 0 || dareId > 9007199254740990");
            subscribe = reader.ReadBoolean();
        }
        
    }
    
}