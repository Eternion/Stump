

// Generated on 02/19/2015 12:09:41
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeBidHouseGenericItemAddedMessage : Message
    {
        public const uint Id = 5947;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short objGenericId;
        
        public ExchangeBidHouseGenericItemAddedMessage()
        {
        }
        
        public ExchangeBidHouseGenericItemAddedMessage(short objGenericId)
        {
            this.objGenericId = objGenericId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarShort(objGenericId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            objGenericId = reader.ReadVarShort();
            if (objGenericId < 0)
                throw new Exception("Forbidden value on objGenericId = " + objGenericId + ", it doesn't respect the following condition : objGenericId < 0");
        }
        
    }
    
}