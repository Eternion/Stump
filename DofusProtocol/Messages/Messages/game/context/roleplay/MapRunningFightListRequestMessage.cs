

// Generated on 02/02/2016 14:14:15
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class MapRunningFightListRequestMessage : Message
    {
        public const uint Id = 5742;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public MapRunningFightListRequestMessage()
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