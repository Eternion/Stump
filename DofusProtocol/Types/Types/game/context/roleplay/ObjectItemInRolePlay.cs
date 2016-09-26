

// Generated on 09/26/2016 01:50:22
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class ObjectItemInRolePlay
    {
        public const short Id = 198;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short cellId;
        public short objectGID;
        
        public ObjectItemInRolePlay()
        {
        }
        
        public ObjectItemInRolePlay(short cellId, short objectGID)
        {
            this.cellId = cellId;
            this.objectGID = objectGID;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(cellId);
            writer.WriteVarShort(objectGID);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            cellId = reader.ReadVarShort();
            if (cellId < 0 || cellId > 559)
                throw new Exception("Forbidden value on cellId = " + cellId + ", it doesn't respect the following condition : cellId < 0 || cellId > 559");
            objectGID = reader.ReadVarShort();
            if (objectGID < 0)
                throw new Exception("Forbidden value on objectGID = " + objectGID + ", it doesn't respect the following condition : objectGID < 0");
        }
        
        
    }
    
}