

// Generated on 03/05/2014 20:34:24
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
        
        public int sourceId;
        public short cellId;
        
        public ShowCellMessage()
        {
        }
        
        public ShowCellMessage(int sourceId, short cellId)
        {
            this.sourceId = sourceId;
            this.cellId = cellId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(sourceId);
            writer.WriteShort(cellId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            sourceId = reader.ReadInt();
            cellId = reader.ReadShort();
            if (cellId < 0 || cellId > 559)
                throw new Exception("Forbidden value on cellId = " + cellId + ", it doesn't respect the following condition : cellId < 0 || cellId > 559");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int) + sizeof(short);
        }
        
    }
    
}