

// Generated on 04/24/2015 03:38:12
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeObjectMessage : Message
    {
        public const uint Id = 5515;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool remote;
        
        public ExchangeObjectMessage()
        {
        }
        
        public ExchangeObjectMessage(bool remote)
        {
            this.remote = remote;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(remote);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            remote = reader.ReadBoolean();
        }
        
    }
    
}