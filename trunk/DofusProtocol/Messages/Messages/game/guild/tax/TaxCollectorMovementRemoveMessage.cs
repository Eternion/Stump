

// Generated on 07/29/2013 23:08:16
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class TaxCollectorMovementRemoveMessage : Message
    {
        public const uint Id = 5915;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int collectorId;
        
        public TaxCollectorMovementRemoveMessage()
        {
        }
        
        public TaxCollectorMovementRemoveMessage(int collectorId)
        {
            this.collectorId = collectorId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(collectorId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            collectorId = reader.ReadInt();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int);
        }
        
    }
    
}