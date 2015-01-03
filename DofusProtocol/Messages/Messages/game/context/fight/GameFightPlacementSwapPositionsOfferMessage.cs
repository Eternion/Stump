

// Generated on 12/29/2014 21:12:15
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
        public int requesterId;
        public short requesterCellId;
        public int requestedId;
        public short requestedCellId;
        
        public GameFightPlacementSwapPositionsOfferMessage()
        {
        }
        
        public GameFightPlacementSwapPositionsOfferMessage(int requestId, int requesterId, short requesterCellId, int requestedId, short requestedCellId)
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
            writer.WriteInt(requesterId);
            writer.WriteShort(requesterCellId);
            writer.WriteInt(requestedId);
            writer.WriteShort(requestedCellId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            requestId = reader.ReadInt();
            if (requestId < 0)
                throw new Exception("Forbidden value on requestId = " + requestId + ", it doesn't respect the following condition : requestId < 0");
            requesterId = reader.ReadInt();
            if (requesterId < 0)
                throw new Exception("Forbidden value on requesterId = " + requesterId + ", it doesn't respect the following condition : requesterId < 0");
            requesterCellId = reader.ReadShort();
            if (requesterCellId < 0 || requesterCellId > 559)
                throw new Exception("Forbidden value on requesterCellId = " + requesterCellId + ", it doesn't respect the following condition : requesterCellId < 0 || requesterCellId > 559");
            requestedId = reader.ReadInt();
            if (requestedId < 0)
                throw new Exception("Forbidden value on requestedId = " + requestedId + ", it doesn't respect the following condition : requestedId < 0");
            requestedCellId = reader.ReadShort();
            if (requestedCellId < 0 || requestedCellId > 559)
                throw new Exception("Forbidden value on requestedCellId = " + requestedCellId + ", it doesn't respect the following condition : requestedCellId < 0 || requestedCellId > 559");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int) + sizeof(int) + sizeof(short) + sizeof(int) + sizeof(short);
        }
        
    }
    
}