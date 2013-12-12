

// Generated on 12/12/2013 16:57:33
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
        public int maxItemLevel;
        public int maxItemPerAccount;
        public int npcContextualId;
        public short unsoldDelay;
        
        public SellerBuyerDescriptor()
        {
        }
        
        public SellerBuyerDescriptor(IEnumerable<int> quantities, IEnumerable<int> types, float taxPercentage, int maxItemLevel, int maxItemPerAccount, int npcContextualId, short unsoldDelay)
        {
            this.quantities = quantities;
            this.types = types;
            this.taxPercentage = taxPercentage;
            this.maxItemLevel = maxItemLevel;
            this.maxItemPerAccount = maxItemPerAccount;
            this.npcContextualId = npcContextualId;
            this.unsoldDelay = unsoldDelay;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)quantities.Count());
            foreach (var entry in quantities)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteUShort((ushort)types.Count());
            foreach (var entry in types)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteFloat(taxPercentage);
            writer.WriteInt(maxItemLevel);
            writer.WriteInt(maxItemPerAccount);
            writer.WriteInt(npcContextualId);
            writer.WriteShort(unsoldDelay);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            quantities = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (quantities as int[])[i] = reader.ReadInt();
            }
            limit = reader.ReadUShort();
            types = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (types as int[])[i] = reader.ReadInt();
            }
            taxPercentage = reader.ReadFloat();
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
            return sizeof(short) + quantities.Sum(x => sizeof(int)) + sizeof(short) + types.Sum(x => sizeof(int)) + sizeof(float) + sizeof(int) + sizeof(int) + sizeof(int) + sizeof(short);
        }
        
    }
    
}