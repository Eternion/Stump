

// Generated on 12/12/2013 16:57:34
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class HouseInformations
    {
        public const short Id = 111;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int houseId;
        public IEnumerable<int> doorsOnMap;
        public string ownerName;
        public short modelId;
        
        public HouseInformations()
        {
        }
        
        public HouseInformations(int houseId, IEnumerable<int> doorsOnMap, string ownerName, short modelId)
        {
            this.houseId = houseId;
            this.doorsOnMap = doorsOnMap;
            this.ownerName = ownerName;
            this.modelId = modelId;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(houseId);
            writer.WriteUShort((ushort)doorsOnMap.Count());
            foreach (var entry in doorsOnMap)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteUTF(ownerName);
            writer.WriteShort(modelId);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            houseId = reader.ReadInt();
            if (houseId < 0)
                throw new Exception("Forbidden value on houseId = " + houseId + ", it doesn't respect the following condition : houseId < 0");
            var limit = reader.ReadUShort();
            doorsOnMap = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (doorsOnMap as int[])[i] = reader.ReadInt();
            }
            ownerName = reader.ReadUTF();
            modelId = reader.ReadShort();
            if (modelId < 0)
                throw new Exception("Forbidden value on modelId = " + modelId + ", it doesn't respect the following condition : modelId < 0");
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(int) + sizeof(short) + doorsOnMap.Sum(x => sizeof(int)) + sizeof(short) + Encoding.UTF8.GetByteCount(ownerName) + sizeof(short);
        }
        
    }
    
}