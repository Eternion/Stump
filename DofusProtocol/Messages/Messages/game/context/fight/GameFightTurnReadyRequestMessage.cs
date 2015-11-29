

// Generated on 11/16/2015 14:26:04
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightTurnReadyRequestMessage : Message
    {
        public const uint Id = 715;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int id;
        
        public GameFightTurnReadyRequestMessage()
        {
        }
        
        public GameFightTurnReadyRequestMessage(int id)
        {
            this.id = id;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(id);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            id = reader.ReadInt();
        }
        
    }
    
}