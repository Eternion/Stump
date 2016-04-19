

// Generated on 04/19/2016 10:17:34
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeObjectTransfertAllFromInvMessage : Message
    {
        public const uint Id = 6184;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public ExchangeObjectTransfertAllFromInvMessage()
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