

// Generated on 08/04/2015 13:25:11
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeBidHouseTypeMessage : Message
    {
        public const uint Id = 5803;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int type;
        
        public ExchangeBidHouseTypeMessage()
        {
        }
        
        public ExchangeBidHouseTypeMessage(int type)
        {
            this.type = type;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(type);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            type = reader.ReadVarInt();
            if (type < 0)
                throw new Exception("Forbidden value on type = " + type + ", it doesn't respect the following condition : type < 0");
        }
        
    }
    
}