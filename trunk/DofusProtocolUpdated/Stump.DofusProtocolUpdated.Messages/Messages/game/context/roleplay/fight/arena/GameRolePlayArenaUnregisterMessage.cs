

// Generated on 03/05/2014 20:34:28
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameRolePlayArenaUnregisterMessage : Message
    {
        public const uint Id = 6282;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        
        public GameRolePlayArenaUnregisterMessage()
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