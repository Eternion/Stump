

// Generated on 10/30/2016 16:20:43
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeReplayStopMessage : Message
    {
        public const uint Id = 6001;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public ExchangeReplayStopMessage()
        {
        }
        
        
        public override void Serialize(IDataWriter writer)
        {
        }
        
        public override void Deserialize(IDataReader reader)
        {
        }
        
    }
    
}