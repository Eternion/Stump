
// Generated on 03/25/2013 19:24:23
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class StorageObjectUpdateMessage : Message
    {
        public const uint Id = 5647;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.ObjectItem @object;
        
        public StorageObjectUpdateMessage()
        {
        }
        
        public StorageObjectUpdateMessage(Types.ObjectItem @object)
        {
            this.@object = @object;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            @object.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            @object = new Types.ObjectItem();
            @object.Deserialize(reader);
        }
        
    }
    
}