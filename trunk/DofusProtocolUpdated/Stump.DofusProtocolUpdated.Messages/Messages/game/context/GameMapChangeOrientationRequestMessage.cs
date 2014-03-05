

// Generated on 03/05/2014 20:34:24
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class GameMapChangeOrientationRequestMessage : Message
    {
        public const uint Id = 945;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte direction;
        
        public GameMapChangeOrientationRequestMessage()
        {
        }
        
        public GameMapChangeOrientationRequestMessage(sbyte direction)
        {
            this.direction = direction;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(direction);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            direction = reader.ReadSByte();
            if (direction < 0)
                throw new Exception("Forbidden value on direction = " + direction + ", it doesn't respect the following condition : direction < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(sbyte);
        }
        
    }
    
}