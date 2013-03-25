
// Generated on 03/25/2013 19:23:57
using System;
using System.Collections.Generic;
using System.Linq;
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