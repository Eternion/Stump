

// Generated on 03/02/2014 20:42:53
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeStartOkNpcShopMessage : Message
    {
        public const uint Id = 5761;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int npcSellerId;
        public int tokenId;
        public IEnumerable<Types.ObjectItemToSellInNpcShop> objectsInfos;
        
        public ExchangeStartOkNpcShopMessage()
        {
        }
        
        public ExchangeStartOkNpcShopMessage(int npcSellerId, int tokenId, IEnumerable<Types.ObjectItemToSellInNpcShop> objectsInfos)
        {
            this.npcSellerId = npcSellerId;
            this.tokenId = tokenId;
            this.objectsInfos = objectsInfos;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(npcSellerId);
            writer.WriteInt(tokenId);
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

        }
        
        public override void Deserialize(IDataReader reader)
        {
            npcSellerId = reader.ReadInt();
            tokenId = reader.ReadInt();
            if (tokenId < 0)
                throw new Exception("Forbidden value on tokenId = " + tokenId + ", it doesn't respect the following condition : tokenId < 0");
            var limit = reader.ReadUShort();
            var objectsInfos_ = new Types.ObjectItemToSellInNpcShop[limit];
            for (int i = 0; i < limit; i++)
            {
                 objectsInfos_[i] = new Types.ObjectItemToSellInNpcShop();
                 objectsInfos_[i].Deserialize(reader);
            }
            objectsInfos = objectsInfos_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int) + sizeof(int) + sizeof(short) + objectsInfos.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}