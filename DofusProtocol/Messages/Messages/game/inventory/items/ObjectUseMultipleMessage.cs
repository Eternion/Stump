

// Generated on 10/26/2014 23:29:41
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ObjectUseMultipleMessage : ObjectUseMessage
    {
        public const uint Id = 6234;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int quantity;
        
        public ObjectUseMultipleMessage()
        {
        }
        
        public ObjectUseMultipleMessage(int objectUID, int quantity)
         : base(objectUID)
        {
            this.quantity = quantity;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(quantity);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            quantity = reader.ReadInt();
            if (quantity < 0)
                throw new Exception("Forbidden value on quantity = " + quantity + ", it doesn't respect the following condition : quantity < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int);
        }
        
    }
    
}