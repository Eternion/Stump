

// Generated on 08/04/2015 13:24:55
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightPlacementSwapPositionsCancelMessage : Message
    {
        public const uint Id = 6543;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int requestId;
        
        public GameFightPlacementSwapPositionsCancelMessage()
        {
        }
        
        public GameFightPlacementSwapPositionsCancelMessage(int requestId)
        {
            this.requestId = requestId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(requestId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            requestId = reader.ReadInt();
            if (requestId < 0)
                throw new Exception("Forbidden value on requestId = " + requestId + ", it doesn't respect the following condition : requestId < 0");
        }
        
    }
    
}