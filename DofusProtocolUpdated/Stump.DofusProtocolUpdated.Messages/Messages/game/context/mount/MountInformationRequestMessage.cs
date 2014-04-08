

// Generated on 03/06/2014 18:50:10
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class MountInformationRequestMessage : Message
    {
        public const uint Id = 5972;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double id;
        public double time;
        
        public MountInformationRequestMessage()
        {
        }
        
        public MountInformationRequestMessage(double id, double time)
        {
            this.id = id;
            this.time = time;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(id);
            writer.WriteDouble(time);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            id = reader.ReadDouble();
            time = reader.ReadDouble();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(double) + sizeof(double);
        }
        
    }
    
}