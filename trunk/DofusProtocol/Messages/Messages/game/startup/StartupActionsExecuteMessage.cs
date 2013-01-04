
// Generated on 01/04/2013 14:36:01
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class StartupActionsExecuteMessage : Message
    {
        public const uint Id = 1302;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public StartupActionsExecuteMessage()
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