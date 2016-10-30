

// Generated on 10/30/2016 16:20:26
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightPlacementSwapPositionsRequestMessage : GameFightPlacementPositionRequestMessage
    {
        public const uint Id = 6541;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double requestedId;
        
        public GameFightPlacementSwapPositionsRequestMessage()
        {
        }
        
        public GameFightPlacementSwapPositionsRequestMessage(short cellId, double requestedId)
         : base(cellId)
        {
            this.requestedId = requestedId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteDouble(requestedId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            requestedId = reader.ReadDouble();
            if (requestedId < -9007199254740990 || requestedId > 9007199254740990)
                throw new Exception("Forbidden value on requestedId = " + requestedId + ", it doesn't respect the following condition : requestedId < -9007199254740990 || requestedId > 9007199254740990");
        }
        
    }
    
}