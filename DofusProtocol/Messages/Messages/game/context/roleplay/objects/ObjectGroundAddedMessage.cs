

// Generated on 10/27/2014 19:57:46
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ObjectGroundAddedMessage : Message
    {
        public const uint Id = 3017;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short cellId;
        public short objectGID;
        
        public ObjectGroundAddedMessage()
        {
        }
        
        public ObjectGroundAddedMessage(short cellId, short objectGID)
        {
            this.cellId = cellId;
            this.objectGID = objectGID;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(cellId);
            writer.WriteShort(objectGID);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            cellId = reader.ReadShort();
            if (cellId < 0 || cellId > 559)
                throw new Exception("Forbidden value on cellId = " + cellId + ", it doesn't respect the following condition : cellId < 0 || cellId > 559");
            objectGID = reader.ReadShort();
            if (objectGID < 0)
                throw new Exception("Forbidden value on objectGID = " + objectGID + ", it doesn't respect the following condition : objectGID < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + sizeof(short);
        }
        
    }
    
}