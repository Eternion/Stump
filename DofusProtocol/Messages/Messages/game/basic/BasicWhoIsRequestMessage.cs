

// Generated on 10/27/2014 19:57:35
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class BasicWhoIsRequestMessage : Message
    {
        public const uint Id = 181;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool verbose;
        public string search;
        
        public BasicWhoIsRequestMessage()
        {
        }
        
        public BasicWhoIsRequestMessage(bool verbose, string search)
        {
            this.verbose = verbose;
            this.search = search;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(verbose);
            writer.WriteUTF(search);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            verbose = reader.ReadBoolean();
            search = reader.ReadUTF();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(bool) + sizeof(short) + Encoding.UTF8.GetByteCount(search);
        }
        
    }
    
}