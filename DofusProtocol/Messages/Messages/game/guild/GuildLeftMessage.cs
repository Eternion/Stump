

// Generated on 09/01/2015 10:48:19
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildLeftMessage : Message
    {
        public const uint Id = 5562;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public GuildLeftMessage()
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