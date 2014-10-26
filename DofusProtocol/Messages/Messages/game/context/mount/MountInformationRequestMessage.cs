

// Generated on 10/26/2014 23:29:23
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
            if (id < -9.007199254740992E15 || id > 9.007199254740992E15)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < -9.007199254740992E15 || id > 9.007199254740992E15");
            time = reader.ReadDouble();
            if (time < -9.007199254740992E15 || time > 9.007199254740992E15)
                throw new Exception("Forbidden value on time = " + time + ", it doesn't respect the following condition : time < -9.007199254740992E15 || time > 9.007199254740992E15");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(double) + sizeof(double);
        }
        
    }
    
}