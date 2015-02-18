

// Generated on 02/18/2015 10:46:27
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ObjectDeletedMessage : Message
    {
        public const uint Id = 3024;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int objectUID;
        
        public ObjectDeletedMessage()
        {
        }
        
        public ObjectDeletedMessage(int objectUID)
        {
            this.objectUID = objectUID;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteVarInt(objectUID);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            objectUID = reader.ReadVarInt();
            if (objectUID < 0)
                throw new Exception("Forbidden value on objectUID = " + objectUID + ", it doesn't respect the following condition : objectUID < 0");
        }
        
    }
    
}