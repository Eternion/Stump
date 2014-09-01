

// Generated on 09/01/2014 15:51:56
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class DisplayNumericalValueMessage : Message
    {
        public const uint Id = 5808;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public int entityId;
        public int value;
        public sbyte type;
        
        public DisplayNumericalValueMessage()
        {
        }
        
        public DisplayNumericalValueMessage(int entityId, int value, sbyte type)
        {
            this.entityId = entityId;
            this.value = value;
            this.type = type;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteInt(entityId);
            writer.WriteInt(value);
            writer.WriteSByte(type);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            entityId = reader.ReadInt();
            value = reader.ReadInt();
            type = reader.ReadSByte();
            if (type < 0)
                throw new Exception("Forbidden value on type = " + type + ", it doesn't respect the following condition : type < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(int) + sizeof(int) + sizeof(sbyte);
        }
        
    }
    
}