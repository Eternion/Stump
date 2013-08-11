

// Generated on 08/11/2013 11:28:31
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class HousePropertiesMessage : Message
    {
        public const uint Id = 5734;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.HouseInformations properties;
        
        public HousePropertiesMessage()
        {
        }
        
        public HousePropertiesMessage(Types.HouseInformations properties)
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
            properties = Types.ProtocolTypeManager.GetInstance<Types.HouseInformations>(reader.ReadShort());
            properties.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + properties.GetSerializationSize();
        }
        
    }
    
}