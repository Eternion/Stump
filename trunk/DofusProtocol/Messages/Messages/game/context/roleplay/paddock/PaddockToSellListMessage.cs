

// Generated on 07/29/2013 23:08:01
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PaddockToSellListMessage : Message
    {
        public const uint Id = 6138;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short pageIndex;
        public short totalPage;
        public IEnumerable<Types.PaddockInformationsForSell> paddockList;
        
        public PaddockToSellListMessage()
        {
        }
        
        public PaddockToSellListMessage(short pageIndex, short totalPage, IEnumerable<Types.PaddockInformationsForSell> paddockList)
        {
            this.pageIndex = pageIndex;
            this.totalPage = totalPage;
            this.paddockList = paddockList;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(pageIndex);
            writer.WriteShort(totalPage);
            writer.WriteUShort((ushort)paddockList.Count());
            foreach (var entry in paddockList)
            {
                 entry.Serialize(writer);
            }
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
            paddockList = new Types.PaddockInformationsForSell[limit];
            for (int i = 0; i < limit; i++)
            {
                 (paddockList as Types.PaddockInformationsForSell[])[i] = new Types.PaddockInformationsForSell();
                 (paddockList as Types.PaddockInformationsForSell[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + sizeof(short) + sizeof(short) + paddockList.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}