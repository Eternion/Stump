

// Generated on 08/04/2015 00:35:38
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class BidExchangerObjectInfo
    {
        public const short Id = 122;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int objectUID;
        public IEnumerable<Types.ObjectEffect> effects;
        public IEnumerable<int> prices;
        
        public BidExchangerObjectInfo()
        {
        }
        
        public BidExchangerObjectInfo(int objectUID, IEnumerable<Types.ObjectEffect> effects, IEnumerable<int> prices)
        {
            this.objectUID = objectUID;
            this.effects = effects;
            this.prices = prices;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(objectUID);
            var effects_before = writer.Position;
            var effects_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in effects)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 effects_count++;
            }
            var effects_after = writer.Position;
            writer.Seek((int)effects_before);
            writer.WriteUShort((ushort)effects_count);
            writer.Seek((int)effects_after);

            var prices_before = writer.Position;
            var prices_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in prices)
            {
                 writer.WriteInt(entry);
                 prices_count++;
            }
            var prices_after = writer.Position;
            writer.Seek((int)prices_before);
            writer.WriteUShort((ushort)prices_count);
            writer.Seek((int)prices_after);

        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            objectUID = reader.ReadVarInt();
            if (objectUID < 0)
                throw new Exception("Forbidden value on objectUID = " + objectUID + ", it doesn't respect the following condition : objectUID < 0");
            var limit = reader.ReadUShort();
            var effects_ = new Types.ObjectEffect[limit];
            for (int i = 0; i < limit; i++)
            {
                 effects_[i] = Types.ProtocolTypeManager.GetInstance<Types.ObjectEffect>(reader.ReadShort());
                 effects_[i].Deserialize(reader);
            }
            effects = effects_;
            limit = reader.ReadUShort();
            var prices_ = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 prices_[i] = reader.ReadInt();
            }
            prices = prices_;
        }
        
        
    }
    
}