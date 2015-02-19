

// Generated on 02/18/2015 10:46:12
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PaddockMoveItemRequestMessage : Message
    {
        public const uint Id = 6052;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short oldCellId;
        public short newCellId;
        
        public PaddockMoveItemRequestMessage()
        {
        }
        
        public PaddockMoveItemRequestMessage(short oldCellId, short newCellId)
        {
            this.oldCellId = oldCellId;
            this.newCellId = newCellId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(oldCellId);
            writer.WriteVarShort(newCellId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            oldCellId = reader.ReadVarShort();
            if (oldCellId < 0 || oldCellId > 559)
                throw new Exception("Forbidden value on oldCellId = " + oldCellId + ", it doesn't respect the following condition : oldCellId < 0 || oldCellId > 559");
            newCellId = reader.ReadVarShort();
            if (newCellId < 0 || newCellId > 559)
                throw new Exception("Forbidden value on newCellId = " + newCellId + ", it doesn't respect the following condition : newCellId < 0 || newCellId > 559");
        }
        
    }
    
}