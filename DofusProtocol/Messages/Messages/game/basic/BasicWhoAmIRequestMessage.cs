

// Generated on 04/19/2016 10:17:12
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class BasicWhoAmIRequestMessage : Message
    {
        public const uint Id = 5664;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool verbose;
        
        public BasicWhoAmIRequestMessage()
        {
        }
        
        public BasicWhoAmIRequestMessage(bool verbose)
        {
            this.verbose = verbose;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(verbose);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            verbose = reader.ReadBoolean();
        }
        
    }
    
}