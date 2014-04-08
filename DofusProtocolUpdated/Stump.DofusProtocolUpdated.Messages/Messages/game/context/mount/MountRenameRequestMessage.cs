

// Generated on 03/06/2014 18:50:10
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class MountRenameRequestMessage : Message
    {
        public const uint Id = 5987;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string name;
        public double mountId;
        
        public MountRenameRequestMessage()
        {
        }
        
        public MountRenameRequestMessage(string name, double mountId)
        {
            this.name = name;
            this.mountId = mountId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(name);
            writer.WriteDouble(mountId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            name = reader.ReadUTF();
            mountId = reader.ReadDouble();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + Encoding.UTF8.GetByteCount(name) + sizeof(double);
        }
        
    }
    
}