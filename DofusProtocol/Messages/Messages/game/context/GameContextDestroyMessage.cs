

// Generated on 08/04/2015 00:36:59
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameContextDestroyMessage : Message
    {
        public const uint Id = 201;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public GameContextDestroyMessage()
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