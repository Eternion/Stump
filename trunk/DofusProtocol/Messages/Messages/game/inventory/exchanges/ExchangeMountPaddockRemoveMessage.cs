
// Generated on 03/25/2013 19:24:19
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeMountPaddockRemoveMessage : Message
    {
        public const uint Id = 6050;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double mountId;
        
        public ExchangeMountPaddockRemoveMessage()
        {
        }
        
        public ExchangeMountPaddockRemoveMessage(double mountId)
        {
            this.mountId = mountId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(mountId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            mountId = reader.ReadDouble();
        }
        
    }
    
}