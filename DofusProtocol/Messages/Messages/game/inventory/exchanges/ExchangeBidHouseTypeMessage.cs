

// Generated on 10/26/2014 23:29:36
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
            writer.WriteInt(type);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            type = reader.ReadInt();
            if (type < 0)
                throw new Exception("Forbidden value on type = " + type + ", it doesn't respect the following condition : type < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int);
        }
        
    }
    
}