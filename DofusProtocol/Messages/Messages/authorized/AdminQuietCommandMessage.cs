

// Generated on 10/30/2016 16:20:18
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AdminQuietCommandMessage : AdminCommandMessage
    {
        public const uint Id = 5662;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public AdminQuietCommandMessage()
        {
        }
        
        public AdminQuietCommandMessage(string content)
         : base(content)
        {
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
        }
        
    }
    
}