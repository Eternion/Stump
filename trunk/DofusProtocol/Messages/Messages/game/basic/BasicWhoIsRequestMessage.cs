
// Generated on 03/25/2013 19:24:03
using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public string search;
        
        public BasicWhoIsRequestMessage()
        {
        }
        
        public BasicWhoIsRequestMessage(string search)
        {
            this.search = search;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUTF(search);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            search = reader.ReadUTF();
        }
        
    }
    
}