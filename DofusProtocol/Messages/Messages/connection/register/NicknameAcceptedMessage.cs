

// Generated on 09/01/2014 15:51:48
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class NicknameAcceptedMessage : Message
    {
        public const uint Id = 5641;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public NicknameAcceptedMessage()
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