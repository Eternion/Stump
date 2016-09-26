

// Generated on 09/26/2016 01:50:03
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class PaddockPropertiesMessage : Message
    {
        public const uint Id = 5824;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.PaddockInformations properties;
        
        public PaddockPropertiesMessage()
        {
        }
        
        public PaddockPropertiesMessage(Types.PaddockInformations properties)
        {
            this.properties = properties;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(properties.TypeId);
            properties.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            properties = Types.ProtocolTypeManager.GetInstance<Types.PaddockInformations>(reader.ReadShort());
            properties.Deserialize(reader);
        }
        
    }
    
}