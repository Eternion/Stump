
// Generated on 03/25/2013 19:24:04
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CharacterNameSuggestionRequestMessage : Message
    {
        public const uint Id = 162;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public CharacterNameSuggestionRequestMessage()
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