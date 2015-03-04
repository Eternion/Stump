

// Generated on 02/19/2015 12:09:25
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class CompassUpdateMessage : Message
    {
        public const uint Id = 5591;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte type;
        public Types.MapCoordinates coords;
        
        public CompassUpdateMessage()
        {
        }
        
        public CompassUpdateMessage(sbyte type, Types.MapCoordinates coords)
        {
            this.type = type;
            this.coords = coords;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(type);
            writer.WriteShort(coords.TypeId);
            coords.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            type = reader.ReadSByte();
            if (type < 0)
                throw new Exception("Forbidden value on type = " + type + ", it doesn't respect the following condition : type < 0");
            coords = Types.ProtocolTypeManager.GetInstance<Types.MapCoordinates>(reader.ReadShort());
            coords.Deserialize(reader);
        }
        
    }
    
}