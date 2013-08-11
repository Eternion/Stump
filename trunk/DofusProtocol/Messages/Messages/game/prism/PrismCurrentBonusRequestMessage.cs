

// Generated on 08/11/2013 11:29:03
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PrismCurrentBonusRequestMessage : Message
    {
        public const uint Id = 5840;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public PrismCurrentBonusRequestMessage()
        {
        }
        
        
        public override void Serialize(IDataWriter writer)
        {
        }
        
        public override void Deserialize(IDataReader reader)
        {
        }
        
        public override int GetSerializationSize()
        {
            return 0;
            ;
        }
        
    }
    
}