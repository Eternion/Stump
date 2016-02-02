

// Generated on 02/02/2016 14:13:59
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class DebugClearHighlightCellsMessage : Message
    {
        public const uint Id = 2002;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public DebugClearHighlightCellsMessage()
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