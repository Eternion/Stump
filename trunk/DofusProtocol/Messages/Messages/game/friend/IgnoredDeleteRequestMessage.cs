

// Generated on 08/11/2013 11:28:44
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class IgnoredDeleteRequestMessage : Message
    {
        public const uint Id = 5680;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string name;
        public bool session;
        
        public IgnoredDeleteRequestMessage()
        {
        }
        
        public IgnoredDeleteRequestMessage(string name, bool session)
        {
            this.name = name;
            this.session = session;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(name);
            writer.WriteBoolean(session);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            name = reader.ReadUTF();
            session = reader.ReadBoolean();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + Encoding.UTF8.GetByteCount(name) + sizeof(bool);
        }
        
    }
    
}