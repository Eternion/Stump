

// Generated on 12/29/2014 21:13:58
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PrismSetSabotagedRefusedMessage : Message
    {
        public const uint Id = 6466;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short subAreaId;
        public sbyte reason;
        
        public PrismSetSabotagedRefusedMessage()
        {
        }
        
        public PrismSetSabotagedRefusedMessage(short subAreaId, sbyte reason)
        {
            this.subAreaId = subAreaId;
            this.reason = reason;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(subAreaId);
            writer.WriteSByte(reason);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            subAreaId = reader.ReadShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            reason = reader.ReadSByte();
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + sizeof(sbyte);
        }
        
    }
    
}