

// Generated on 08/04/2015 13:25:20
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class StartupActionAddMessage : Message
    {
        public const uint Id = 6538;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.StartupActionAddObject newAction;
        
        public StartupActionAddMessage()
        {
        }
        
        public StartupActionAddMessage(Types.StartupActionAddObject newAction)
        {
            this.newAction = newAction;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            newAction.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            newAction = new Types.StartupActionAddObject();
            newAction.Deserialize(reader);
        }
        
    }
    
}