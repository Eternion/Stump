

// Generated on 12/29/2014 21:12:28
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
        
        public int targetId;
        public short cellId;
        
        public TeleportOnSameMapMessage()
        {
        }
        
        public TeleportOnSameMapMessage(int targetId, short cellId)
        {
            this.targetId = targetId;
            this.cellId = cellId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(targetId);
            writer.WriteShort(cellId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            targetId = reader.ReadInt();
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