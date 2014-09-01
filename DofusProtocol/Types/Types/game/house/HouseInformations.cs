

// Generated on 09/01/2014 15:52:53
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
        
        public bool isOnSale;
        public bool isSaleLocked;
        public int houseId;
        public IEnumerable<int> doorsOnMap;
        public string ownerName;
        public short modelId;
        
        public HouseInformations()
        {
        }
        
        public HouseInformations(bool isOnSale, bool isSaleLocked, int houseId, IEnumerable<int> doorsOnMap, string ownerName, short modelId)
        {
            this.isOnSale = isOnSale;
            this.isSaleLocked = isSaleLocked;
            this.houseId = houseId;
            this.doorsOnMap = doorsOnMap;
            this.ownerName = ownerName;
            this.modelId = modelId;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            byte flag1 = 0;
            flag1 = BooleanByteWrapper.SetFlag(flag1, 0, isOnSale);
            flag1 = BooleanByteWrapper.SetFlag(flag1, 1, isSaleLocked);
            writer.WriteByte(flag1);
            writer.WriteInt(houseId);
            var doorsOnMap_before = writer.Position;
            var doorsOnMap_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in doorsOnMap)
            {
                 writer.WriteInt(entry);
                 doorsOnMap_count++;
            }
            var doorsOnMap_after = writer.Position;
            writer.Seek((int)doorsOnMap_before);
            writer.WriteUShort((ushort)doorsOnMap_count);
            writer.Seek((int)doorsOnMap_after);

            writer.WriteUTF(ownerName);
            writer.WriteShort(modelId);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            byte flag1 = reader.ReadByte();
            isOnSale = BooleanByteWrapper.GetFlag(flag1, 0);
            isSaleLocked = BooleanByteWrapper.GetFlag(flag1, 1);
            houseId = reader.ReadInt();
            if (houseId < 0)
                throw new Exception("Forbidden value on houseId = " + houseId + ", it doesn't respect the following condition : houseId < 0");
            var limit = reader.ReadUShort();
            var doorsOnMap_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 doorsOnMap_[i] = reader.ReadInt();
            }
            doorsOnMap = doorsOnMap_;
            ownerName = reader.ReadUTF();
            modelId = reader.ReadShort();
            if (modelId < 0)
                throw new Exception("Forbidden value on modelId = " + modelId + ", it doesn't respect the following condition : modelId < 0");
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(bool) + 0 + sizeof(int) + sizeof(short) + doorsOnMap.Sum(x => sizeof(int)) + sizeof(short) + Encoding.UTF8.GetByteCount(ownerName) + sizeof(short);
        }
        
    }
    
}