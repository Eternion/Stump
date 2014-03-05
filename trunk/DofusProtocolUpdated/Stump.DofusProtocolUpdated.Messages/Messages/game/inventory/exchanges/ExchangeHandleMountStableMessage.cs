

// Generated on 03/05/2014 20:34:38
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeHandleMountStableMessage : Message
    {
        public const uint Id = 5965;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte actionType;
        public int rideId;
        
        public ExchangeHandleMountStableMessage()
        {
        }
        
        public ExchangeHandleMountStableMessage(sbyte actionType, int rideId)
        {
            this.actionType = actionType;
            this.rideId = rideId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(actionType);
            writer.WriteInt(rideId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            actionType = reader.ReadSByte();
            rideId = reader.ReadInt();
            if (rideId < 0)
                throw new Exception("Forbidden value on rideId = " + rideId + ", it doesn't respect the following condition : rideId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(sbyte) + sizeof(int);
        }
        
    }
    
}