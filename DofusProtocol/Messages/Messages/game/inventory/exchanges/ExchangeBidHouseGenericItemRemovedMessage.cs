

// Generated on 12/29/2014 21:13:25
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeBidHouseGenericItemRemovedMessage : Message
    {
        public const uint Id = 5948;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short objGenericId;
        
        public ExchangeBidHouseGenericItemRemovedMessage()
        {
        }
        
        public ExchangeBidHouseGenericItemRemovedMessage(short objGenericId)
        {
            this.objGenericId = objGenericId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(objGenericId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            objGenericId = reader.ReadShort();
            if (objGenericId < 0)
                throw new Exception("Forbidden value on objGenericId = " + objGenericId + ", it doesn't respect the following condition : objGenericId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short);
        }
        
    }
    
}