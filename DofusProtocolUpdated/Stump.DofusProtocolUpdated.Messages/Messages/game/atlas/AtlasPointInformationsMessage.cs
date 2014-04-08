

// Generated on 03/06/2014 18:50:04
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AtlasPointInformationsMessage : Message
    {
        public const uint Id = 5956;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public Types.AtlasPointsInformations type;
        
        public AtlasPointInformationsMessage()
        {
        }
        
        public AtlasPointInformationsMessage(Types.AtlasPointsInformations type)
        {
            this.type = type;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            type.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            type = new Types.AtlasPointsInformations();
            type.Deserialize(reader);
        }
        
        public override int GetSerializationSize()
        {
            return type.GetSerializationSize();
        }
        
    }
    
}