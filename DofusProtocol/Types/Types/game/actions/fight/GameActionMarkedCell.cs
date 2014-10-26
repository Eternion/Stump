

// Generated on 10/26/2014 23:30:14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class GameActionMarkedCell
    {
        public const short Id = 85;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public short cellId;
        public sbyte zoneSize;
        public int cellColor;
        public sbyte cellsType;
        
        public GameActionMarkedCell()
        {
        }
        
        public GameActionMarkedCell(short cellId, sbyte zoneSize, int cellColor, sbyte cellsType)
        {
            this.cellId = cellId;
            this.zoneSize = zoneSize;
            this.cellColor = cellColor;
            this.cellsType = cellsType;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteShort(cellId);
            writer.WriteSByte(zoneSize);
            writer.WriteInt(cellColor);
            writer.WriteSByte(cellsType);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            cellId = reader.ReadShort();
            if (cellId < 0 || cellId > 559)
                throw new Exception("Forbidden value on cellId = " + cellId + ", it doesn't respect the following condition : cellId < 0 || cellId > 559");
            zoneSize = reader.ReadSByte();
            cellColor = reader.ReadInt();
            cellsType = reader.ReadSByte();
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(short) + sizeof(sbyte) + sizeof(int) + sizeof(sbyte);
        }
        
    }
    
}