

// Generated on 03/02/2014 20:42:42
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PaddockToSellListRequestMessage : Message
    {
        public const uint Id = 6141;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short pageIndex;
        
        public PaddockToSellListRequestMessage()
        {
        }
        
        public PaddockToSellListRequestMessage(short pageIndex)
        {
            this.pageIndex = pageIndex;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(pageIndex);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            pageIndex = reader.ReadShort();
            if (pageIndex < 0)
                throw new Exception("Forbidden value on pageIndex = " + pageIndex + ", it doesn't respect the following condition : pageIndex < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short);
        }
        
    }
    
}