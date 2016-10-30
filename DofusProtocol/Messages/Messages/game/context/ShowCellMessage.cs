

// Generated on 10/30/2016 16:20:26
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ShowCellMessage : Message
    {
        public const uint Id = 5612;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public double sourceId;
        public short cellId;
        
        public ShowCellMessage()
        {
        }
        
        public ShowCellMessage(double sourceId, short cellId)
        {
            this.sourceId = sourceId;
            this.cellId = cellId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteDouble(sourceId);
            writer.WriteVarShort(cellId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            sourceId = reader.ReadDouble();
            if (sourceId < -9007199254740990 || sourceId > 9007199254740990)
                throw new Exception("Forbidden value on sourceId = " + sourceId + ", it doesn't respect the following condition : sourceId < -9007199254740990 || sourceId > 9007199254740990");
            cellId = reader.ReadVarShort();
            if (cellId < 0 || cellId > 559)
                throw new Exception("Forbidden value on cellId = " + cellId + ", it doesn't respect the following condition : cellId < 0 || cellId > 559");
        }
        
    }
    
}