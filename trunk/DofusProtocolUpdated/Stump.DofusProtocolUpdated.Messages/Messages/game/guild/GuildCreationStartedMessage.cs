

// Generated on 12/12/2013 16:57:10
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GuildCreationStartedMessage : Message
    {
        public const uint Id = 5920;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public GuildCreationStartedMessage()
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