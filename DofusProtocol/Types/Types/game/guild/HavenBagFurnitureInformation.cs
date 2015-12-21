

// Generated on 12/20/2015 17:30:59
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class HavenBagFurnitureInformation
    {
        public const short Id = 498;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short cellId;
        public int funitureId;
        public sbyte orientation;
        
        public HavenBagFurnitureInformation()
        {
        }
        
        public HavenBagFurnitureInformation(short cellId, int funitureId, sbyte orientation)
        {
            this.cellId = cellId;
            this.funitureId = funitureId;
            this.orientation = orientation;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(cellId);
            writer.WriteInt(funitureId);
            writer.WriteSByte(orientation);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            cellId = reader.ReadVarShort();
            if (cellId < 0)
                throw new Exception("Forbidden value on cellId = " + cellId + ", it doesn't respect the following condition : cellId < 0");
            funitureId = reader.ReadInt();
            orientation = reader.ReadSByte();
            if (orientation < 0)
                throw new Exception("Forbidden value on orientation = " + orientation + ", it doesn't respect the following condition : orientation < 0");
        }
        
        
    }
    
}