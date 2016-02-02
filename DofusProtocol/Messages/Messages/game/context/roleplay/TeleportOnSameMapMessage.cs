

// Generated on 02/02/2016 14:14:15
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class TeleportOnSameMapMessage : Message
    {
        public const uint Id = 6048;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double targetId;
        public short cellId;
        
        public TeleportOnSameMapMessage()
        {
        }
        
        public TeleportOnSameMapMessage(double targetId, short cellId)
        {
            this.targetId = targetId;
            this.cellId = cellId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(targetId);
            writer.WriteVarShort(cellId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            targetId = reader.ReadDouble();
            if (targetId < -9.007199254740992E15 || targetId > 9.007199254740992E15)
                throw new Exception("Forbidden value on targetId = " + targetId + ", it doesn't respect the following condition : targetId < -9.007199254740992E15 || targetId > 9.007199254740992E15");
            cellId = reader.ReadVarShort();
            if (cellId < 0 || cellId > 559)
                throw new Exception("Forbidden value on cellId = " + cellId + ", it doesn't respect the following condition : cellId < 0 || cellId > 559");
        }
        
    }
    
}