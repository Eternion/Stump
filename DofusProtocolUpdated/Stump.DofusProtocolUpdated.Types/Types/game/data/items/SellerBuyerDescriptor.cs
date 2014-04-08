

// Generated on 03/06/2014 18:50:35
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class SellerBuyerDescriptor
    {
        public const short Id = 121;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public IEnumerable<int> quantities;
        public IEnumerable<int> types;
        public float taxPercentage;
        public float taxModificationPercentage;
        public int maxItemLevel;
        public int maxItemPerAccount;
        public int npcContextualId;
        public short unsoldDelay;
        
        public SellerBuyerDescriptor()
        {
        }
        
        public SellerBuyerDescriptor(IEnumerable<int> quantities, IEnumerable<int> types, float taxPercentage, float taxModificationPercentage, int maxItemLevel, int maxItemPerAccount, int npcContextualId, short unsoldDelay)
        {
            this.quantities = quantities;
            this.types = types;
            this.taxPercentage = taxPercentage;
            this.taxModificationPercentage = taxModificationPercentage;
            this.maxItemLevel = maxItemLevel;
            this.maxItemPerAccount = maxItemPerAccount;
            this.npcContextualId = npcContextualId;
            this.unsoldDelay = unsoldDelay;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            var quantities_before = writer.Position;
            var quantities_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in quantities)
            {
                 writer.WriteInt(entry);
                 quantities_count++;
            }
            var quantities_after = writer.Position;
            writer.Seek((int)quantities_before);
            writer.WriteUShort((ushort)quantities_count);
            writer.Seek((int)quantities_after);

            var types_before = writer.Position;
            var types_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in types)
            {
                 writer.WriteInt(entry);
                 types_count++;
            }
            var types_after = writer.Position;
            writer.Seek((int)types_before);
            writer.WriteUShort((ushort)types_count);
            writer.Seek((int)types_after);

            writer.WriteFloat(taxPercentage);
            writer.WriteFloat(taxModificationPercentage);
            writer.WriteInt(maxItemLevel);
            writer.WriteInt(maxItemPerAccount);
            writer.WriteInt(npcContextualId);
            writer.WriteShort(unsoldDelay);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var quantities_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 quantities_[i] = reader.ReadInt();
            }
            quantities = quantities_;
            limit = reader.ReadUShort();
            var types_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 types_[i] = reader.ReadInt();
            }
            types = types_;
            taxPercentage = reader.ReadFloat();
            taxModificationPercentage = reader.ReadFloat();
            maxItemLevel = reader.ReadInt();
            if (maxItemLevel < 0)
                throw new Exception("Forbidden value on maxItemLevel = " + maxItemLevel + ", it doesn't respect the following condition : maxItemLevel < 0");
            maxItemPerAccount = reader.ReadInt();
            if (maxItemPerAccount < 0)
                throw new Exception("Forbidden value on maxItemPerAccount = " + maxItemPerAccount + ", it doesn't respect the following condition : maxItemPerAccount < 0");
            npcContextualId = reader.ReadInt();
            unsoldDelay = reader.ReadShort();
            if (unsoldDelay < 0)
                throw new Exception("Forbidden value on unsoldDelay = " + unsoldDelay + ", it doesn't respect the following condition : unsoldDelay < 0");
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(short) + quantities.Sum(x => sizeof(int)) + sizeof(short) + types.Sum(x => sizeof(int)) + sizeof(float) + sizeof(float) + sizeof(int) + sizeof(int) + sizeof(int) + sizeof(short);
        }
        
    }
    
}