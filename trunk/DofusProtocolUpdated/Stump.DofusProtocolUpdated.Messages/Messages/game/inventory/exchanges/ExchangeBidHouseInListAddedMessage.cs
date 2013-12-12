

// Generated on 12/12/2013 16:57:14
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeBidHouseInListAddedMessage : Message
    {
        public const uint Id = 5949;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int itemUID;
        public int objGenericId;
        public IEnumerable<Types.ObjectEffect> effects;
        public IEnumerable<int> prices;
        
        public ExchangeBidHouseInListAddedMessage()
        {
        }
        
        public ExchangeBidHouseInListAddedMessage(int itemUID, int objGenericId, IEnumerable<Types.ObjectEffect> effects, IEnumerable<int> prices)
        {
            this.itemUID = itemUID;
            this.objGenericId = objGenericId;
            this.effects = effects;
            this.prices = prices;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(itemUID);
            writer.WriteInt(objGenericId);
            writer.WriteUShort((ushort)effects.Count());
            foreach (var entry in effects)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)prices.Count());
            foreach (var entry in prices)
            {
                 writer.WriteInt(entry);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            itemUID = reader.ReadInt();
            objGenericId = reader.ReadInt();
            var limit = reader.ReadUShort();
            effects = new Types.ObjectEffect[limit];
            for (int i = 0; i < limit; i++)
            {
                 (effects as Types.ObjectEffect[])[i] = Types.ProtocolTypeManager.GetInstance<Types.ObjectEffect>(reader.ReadShort());
                 (effects as Types.ObjectEffect[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            prices = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (prices as int[])[i] = reader.ReadInt();
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int) + sizeof(int) + sizeof(short) + effects.Sum(x => sizeof(short) + x.GetSerializationSize()) + sizeof(short) + prices.Sum(x => sizeof(int));
        }
        
    }
    
}