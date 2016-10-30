

// Generated on 10/30/2016 16:20:23
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class BasicWhoIsNoMatchMessage : Message
    {
        public const uint Id = 179;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public string search;
        
        public BasicWhoIsNoMatchMessage()
        {
        }
        
        public BasicWhoIsNoMatchMessage(string search)
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