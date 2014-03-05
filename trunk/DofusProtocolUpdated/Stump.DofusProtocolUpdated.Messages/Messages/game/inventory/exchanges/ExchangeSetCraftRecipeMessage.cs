

// Generated on 03/05/2014 20:34:39
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ExchangeSetCraftRecipeMessage : Message
    {
        public const uint Id = 6389;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short objectGID;
        
        public ExchangeSetCraftRecipeMessage()
        {
        }
        
        public ExchangeSetCraftRecipeMessage(short objectGID)
        {
            this.objectGID = objectGID;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(objectGID);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            objectGID = reader.ReadShort();
            if (objectGID < 0)
                throw new Exception("Forbidden value on objectGID = " + objectGID + ", it doesn't respect the following condition : objectGID < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short);
        }
        
    }
    
}