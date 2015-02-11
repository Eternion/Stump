

// Generated on 02/11/2015 10:20:36
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeBidHouseSearchMessage : Message
    {
        public const uint Id = 5806;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int type;
        public short genId;
        
        public ExchangeBidHouseSearchMessage()
        {
        }
        
        public ExchangeBidHouseSearchMessage(int type, short genId)
        {
            this.type = type;
            this.genId = genId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(type);
            writer.WriteVarShort(genId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            type = reader.ReadVarInt();
            if (type < 0)
                throw new Exception("Forbidden value on type = " + type + ", it doesn't respect the following condition : type < 0");
            genId = reader.ReadVarShort();
            if (genId < 0)
                throw new Exception("Forbidden value on genId = " + genId + ", it doesn't respect the following condition : genId < 0");
        }
        
    }
    
}