

// Generated on 01/04/2015 11:54:42
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GetPartsListMessage : Message
    {
        public const uint Id = 1501;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public GetPartsListMessage()
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