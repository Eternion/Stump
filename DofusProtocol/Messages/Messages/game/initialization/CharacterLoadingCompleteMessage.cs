

// Generated on 02/02/2016 14:14:32
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CharacterLoadingCompleteMessage : Message
    {
        public const uint Id = 6471;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public CharacterLoadingCompleteMessage()
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