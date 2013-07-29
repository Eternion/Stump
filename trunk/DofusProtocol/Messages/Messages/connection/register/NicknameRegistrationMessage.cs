

// Generated on 07/29/2013 23:07:29
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class NicknameRegistrationMessage : Message
    {
        public const uint Id = 5640;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public NicknameRegistrationMessage()
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