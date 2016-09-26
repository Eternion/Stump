

// Generated on 09/26/2016 01:49:57
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameMapChangeOrientationMessage : Message
    {
        public const uint Id = 946;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.ActorOrientation orientation;
        
        public GameMapChangeOrientationMessage()
        {
        }
        
        public GameMapChangeOrientationMessage(Types.ActorOrientation orientation)
        {
            this.orientation = orientation;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            orientation.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            orientation = new Types.ActorOrientation();
            orientation.Deserialize(reader);
        }
        
    }
    
}