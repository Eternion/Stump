

// Generated on 08/11/2013 11:29:16
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class ObjectItemInformationWithQuantity : ObjectItemMinimalInformation
    {
        public const short Id = 387;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int quantity;
        
        public ObjectItemInformationWithQuantity()
        {
        }
        
        public ObjectItemInformationWithQuantity(short objectGID, short powerRate, bool overMax, IEnumerable<Types.ObjectEffect> effects, int quantity)
         : base(objectGID, powerRate, overMax, effects)
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