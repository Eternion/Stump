

// Generated on 10/30/2016 16:20:26
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameFightPlacementSwapPositionsOfferMessage : Message
    {
        public const uint Id = 6542;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int requestId;
        public double requesterId;
        public short requesterCellId;
        public double requestedId;
        public short requestedCellId;
        
        public GameFightPlacementSwapPositionsOfferMessage()
        {
        }
        
        public GameFightPlacementSwapPositionsOfferMessage(int requestId, double requesterId, short requesterCellId, double requestedId, short requestedCellId)
        {
            this.requestId = requestId;
            this.requesterId = requesterId;
            this.requesterCellId = requesterCellId;
            this.requestedId = requestedId;
            this.requestedCellId = requestedCellId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(requestId);
            writer.WriteDouble(requesterId);
            writer.WriteVarShort(requesterCellId);
            writer.WriteDouble(requestedId);
            writer.WriteVarShort(requestedCellId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            requestId = reader.ReadInt();
            if (requestId < 0)
                throw new Exception("Forbidden value on requestId = " + requestId + ", it doesn't respect the following condition : requestId < 0");
            requesterId = reader.ReadDouble();
            if (requesterId < -9007199254740990 || requesterId > 9007199254740990)
                throw new Exception("Forbidden value on requesterId = " + requesterId + ", it doesn't respect the following condition : requesterId < -9007199254740990 || requesterId > 9007199254740990");
            requesterCellId = reader.ReadVarShort();
            if (requesterCellId < 0 || requesterCellId > 559)
                throw new Exception("Forbidden value on requesterCellId = " + requesterCellId + ", it doesn't respect the following condition : requesterCellId < 0 || requesterCellId > 559");
            requestedId = reader.ReadDouble();
            if (requestedId < -9007199254740990 || requestedId > 9007199254740990)
                throw new Exception("Forbidden value on requestedId = " + requestedId + ", it doesn't respect the following condition : requestedId < -9007199254740990 || requestedId > 9007199254740990");
            requestedCellId = reader.ReadVarShort();
            if (requestedCellId < 0 || requestedCellId > 559)
                throw new Exception("Forbidden value on requestedCellId = " + requestedCellId + ", it doesn't respect the following condition : requestedCellId < 0 || requestedCellId > 559");
        }
        
    }
    
}