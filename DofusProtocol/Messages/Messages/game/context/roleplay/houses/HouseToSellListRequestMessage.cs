

// Generated on 04/24/2015 03:38:04
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class HouseToSellListRequestMessage : Message
    {
        public const uint Id = 6139;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short pageIndex;
        
        public HouseToSellListRequestMessage()
        {
        }
        
        public HouseToSellListRequestMessage(short pageIndex)
        {
            this.pageIndex = pageIndex;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(pageIndex);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            pageIndex = reader.ReadVarShort();
            if (pageIndex < 0)
                throw new Exception("Forbidden value on pageIndex = " + pageIndex + ", it doesn't respect the following condition : pageIndex < 0");
        }
        
    }
    
}