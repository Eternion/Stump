

// Generated on 12/29/2014 21:13:40
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeStartOkTaxCollectorMessage : Message
    {
        public const uint Id = 5780;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int collectorId;
        public IEnumerable<Types.ObjectItem> objectsInfos;
        public int goldInfo;
        
        public ExchangeStartOkTaxCollectorMessage()
        {
        }
        
        public ExchangeStartOkTaxCollectorMessage(int collectorId, IEnumerable<Types.ObjectItem> objectsInfos, int goldInfo)
        {
            this.collectorId = collectorId;
            this.objectsInfos = objectsInfos;
            this.goldInfo = goldInfo;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(collectorId);
            var objectsInfos_before = writer.Position;
            var objectsInfos_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in objectsInfos)
            {
                 entry.Serialize(writer);
                 objectsInfos_count++;
            }
            var objectsInfos_after = writer.Position;
            writer.Seek((int)objectsInfos_before);
            writer.WriteUShort((ushort)objectsInfos_count);
            writer.Seek((int)objectsInfos_after);

            writer.WriteInt(goldInfo);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            collectorId = reader.ReadInt();
            var limit = reader.ReadUShort();
            var objectsInfos_ = new Types.ObjectItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 objectsInfos_[i] = new Types.ObjectItem();
                 objectsInfos_[i].Deserialize(reader);
            }
            objectsInfos = objectsInfos_;
            goldInfo = reader.ReadInt();
            if (goldInfo < 0)
                throw new Exception("Forbidden value on goldInfo = " + goldInfo + ", it doesn't respect the following condition : goldInfo < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int) + sizeof(short) + objectsInfos.Sum(x => x.GetSerializationSize()) + sizeof(int);
        }
        
    }
    
}