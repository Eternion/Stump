

// Generated on 09/01/2015 10:48:03
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameContextMoveElementMessage : Message
    {
        public const uint Id = 253;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.EntityMovementInformations movement;
        
        public GameContextMoveElementMessage()
        {
        }
        
        public GameContextMoveElementMessage(Types.EntityMovementInformations movement)
        {
            this.movement = movement;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            movement.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            movement = new Types.EntityMovementInformations();
            movement.Deserialize(reader);
        }
        
    }
    
}