

// Generated on 11/16/2015 14:25:59
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class BasicLatencyStatsRequestMessage : Message
    {
        public const uint Id = 5816;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public BasicLatencyStatsRequestMessage()
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