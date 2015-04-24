

// Generated on 04/24/2015 03:38:11
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeBidHouseListMessage : Message
    {
        public const uint Id = 5807;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short id;
        
        public ExchangeBidHouseListMessage()
        {
        }
        
        public ExchangeBidHouseListMessage(short id)
        {
            this.id = id;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(id);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            id = reader.ReadVarShort();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
        }
        
    }
    
}