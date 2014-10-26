

// Generated on 10/26/2014 23:29:36
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeCraftResultWithObjectIdMessage : ExchangeCraftResultMessage
    {
        public const uint Id = 6000;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int objectGenericId;
        
        public ExchangeCraftResultWithObjectIdMessage()
        {
        }
        
        public ExchangeCraftResultWithObjectIdMessage(sbyte craftResult, int objectGenericId)
         : base(craftResult)
        {
            this.objectGenericId = objectGenericId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(objectGenericId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            objectGenericId = reader.ReadInt();
            if (objectGenericId < 0)
                throw new Exception("Forbidden value on objectGenericId = " + objectGenericId + ", it doesn't respect the following condition : objectGenericId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(int);
        }
        
    }
    
}