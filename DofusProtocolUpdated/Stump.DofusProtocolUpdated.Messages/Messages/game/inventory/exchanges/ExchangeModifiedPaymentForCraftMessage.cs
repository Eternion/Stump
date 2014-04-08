

// Generated on 03/06/2014 18:50:22
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeModifiedPaymentForCraftMessage : Message
    {
        public const uint Id = 5832;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool onlySuccess;
        public Types.ObjectItemNotInContainer @object;
        
        public ExchangeModifiedPaymentForCraftMessage()
        {
        }
        
        public ExchangeModifiedPaymentForCraftMessage(bool onlySuccess, Types.ObjectItemNotInContainer @object)
        {
            this.onlySuccess = onlySuccess;
            this.@object = @object;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(onlySuccess);
            @object.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            onlySuccess = reader.ReadBoolean();
            @object = new Types.ObjectItemNotInContainer();
            @object.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(bool) + @object.GetSerializationSize();
        }
        
    }
    
}