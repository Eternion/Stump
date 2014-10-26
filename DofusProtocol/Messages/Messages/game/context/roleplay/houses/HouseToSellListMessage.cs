

// Generated on 10/26/2014 23:29:26
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class HouseToSellListMessage : Message
    {
        public const uint Id = 6140;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short pageIndex;
        public short totalPage;
        public IEnumerable<Types.HouseInformationsForSell> houseList;
        
        public HouseToSellListMessage()
        {
        }
        
        public HouseToSellListMessage(short pageIndex, short totalPage, IEnumerable<Types.HouseInformationsForSell> houseList)
        {
            this.pageIndex = pageIndex;
            this.totalPage = totalPage;
            this.houseList = houseList;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(pageIndex);
            writer.WriteShort(totalPage);
            var houseList_before = writer.Position;
            var houseList_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in houseList)
            {
                 entry.Serialize(writer);
                 houseList_count++;
            }
            var houseList_after = writer.Position;
            writer.Seek((int)houseList_before);
            writer.WriteUShort((ushort)houseList_count);
            writer.Seek((int)houseList_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            pageIndex = reader.ReadShort();
            if (pageIndex < 0)
                throw new Exception("Forbidden value on pageIndex = " + pageIndex + ", it doesn't respect the following condition : pageIndex < 0");
            totalPage = reader.ReadShort();
            if (totalPage < 0)
                throw new Exception("Forbidden value on totalPage = " + totalPage + ", it doesn't respect the following condition : totalPage < 0");
            var limit = reader.ReadUShort();
            var houseList_ = new Types.HouseInformationsForSell[limit];
            for (int i = 0; i < limit; i++)
            {
                 houseList_[i] = new Types.HouseInformationsForSell();
                 houseList_[i].Deserialize(reader);
            }
            houseList = houseList_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + sizeof(short) + sizeof(short) + houseList.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}