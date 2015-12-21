

// Generated on 12/20/2015 16:36:53
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class TeleportHavenBagAnswerMessage : Message
    {
        public const uint Id = 6646;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool accept;
        
        public TeleportHavenBagAnswerMessage()
        {
        }
        
        public TeleportHavenBagAnswerMessage(bool accept)
        {
            this.accept = accept;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(accept);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            accept = reader.ReadBoolean();
        }
        
    }
    
}