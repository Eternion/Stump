

// Generated on 10/26/2014 23:29:43
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PrismSetSabotagedRequestMessage : Message
    {
        public const uint Id = 6468;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short subAreaId;
        
        public PrismSetSabotagedRequestMessage()
        {
        }
        
        public PrismSetSabotagedRequestMessage(short subAreaId)
        {
            this.subAreaId = subAreaId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(subAreaId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            subAreaId = reader.ReadShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short);
        }
        
    }
    
}