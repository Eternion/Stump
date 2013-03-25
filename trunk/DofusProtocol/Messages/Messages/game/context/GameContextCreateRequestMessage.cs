
// Generated on 03/25/2013 19:24:06
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameContextCreateRequestMessage : Message
    {
        public const uint Id = 250;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public GameContextCreateRequestMessage()
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