

// Generated on 10/26/2014 23:30:19
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class TaxCollectorLootInformations : TaxCollectorComplementaryInformations
    {
        public const short Id = 372;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int kamas;
        public double experience;
        public int pods;
        public int itemsValue;
        
        public TaxCollectorLootInformations()
        {
        }
        
        public TaxCollectorLootInformations(int kamas, double experience, int pods, int itemsValue)
        {
            this.kamas = kamas;
            this.experience = experience;
            this.pods = pods;
            this.itemsValue = itemsValue;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(kamas);
            writer.WriteDouble(experience);
            writer.WriteInt(pods);
            writer.WriteInt(itemsValue);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            kamas = reader.ReadInt();
            if (kamas < 0)
                throw new Exception("Forbidden value on kamas = " + kamas + ", it doesn't respect the following condition : kamas < 0");
            experience = reader.ReadDouble();
            if (experience < 0 || experience > 9.007199254740992E15)
                throw new Exception("Forbidden value on experience = " + experience + ", it doesn't respect the following condition : experience < 0 || experience > 9.007199254740992E15");
            pods = reader.ReadInt();
            if (pods < 0)
                throw new Exception("Forbidden value on pods = " + pods + ", it doesn't respect the following condition : pods < 0");
            itemsValue = reader.ReadInt();
            if (itemsValue < 0)
                throw new Exception("Forbidden value on itemsValue = " + itemsValue + ", it doesn't respect the following condition : itemsValue < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int) + sizeof(double) + sizeof(int) + sizeof(int);
        }
        
    }
    
}