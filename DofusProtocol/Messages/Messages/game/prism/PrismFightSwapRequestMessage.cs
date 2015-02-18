

// Generated on 02/18/2015 10:46:29
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PrismFightSwapRequestMessage : Message
    {
        public const uint Id = 5901;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short subAreaId;
        public int targetId;
        
        public PrismFightSwapRequestMessage()
        {
        }
        
        public PrismFightSwapRequestMessage(short subAreaId, int targetId)
        {
            this.subAreaId = subAreaId;
            this.targetId = targetId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(subAreaId);
            writer.WriteVarInt(targetId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            subAreaId = reader.ReadVarShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            targetId = reader.ReadVarInt();
            if (targetId < 0)
                throw new Exception("Forbidden value on targetId = " + targetId + ", it doesn't respect the following condition : targetId < 0");
        }
        
    }
    
}