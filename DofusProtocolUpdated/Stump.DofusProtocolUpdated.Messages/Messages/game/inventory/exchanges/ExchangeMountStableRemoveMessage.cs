

// Generated on 03/06/2014 18:50:22
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeMountStableRemoveMessage : Message
    {
        public const uint Id = 5964;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double mountId;
        
        public ExchangeMountStableRemoveMessage()
        {
        }
        
        public ExchangeMountStableRemoveMessage(double mountId)
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
        
        public override int GetSerializationSize()
        {
            return sizeof(double);
        }
        
    }
    
}