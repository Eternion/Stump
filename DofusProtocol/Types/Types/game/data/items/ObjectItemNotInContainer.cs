

// Generated on 10/28/2014 16:38:04
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class ObjectItemNotInContainer : Item
    {
        public const short Id = 134;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short objectGID;
        public IEnumerable<Types.ObjectEffect> effects;
        public int objectUID;
        public int quantity;
        
        public ObjectItemNotInContainer()
        {
        }
        
        public ObjectItemNotInContainer(short objectGID, IEnumerable<Types.ObjectEffect> effects, int objectUID, int quantity)
        {
            this.objectGID = objectGID;
            this.effects = effects;
            this.objectUID = objectUID;
            this.quantity = quantity;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(objectGID);
            var effects_before = writer.Position;
            var effects_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in effects)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 effects_count++;
            }
            var effects_after = writer.Position;
            writer.Seek((int)effects_before);
            writer.WriteUShort((ushort)effects_count);
            writer.Seek((int)effects_after);

            writer.WriteInt(objectUID);
            writer.WriteInt(quantity);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            objectGID = reader.ReadShort();
            if (objectGID < 0)
                throw new Exception("Forbidden value on objectGID = " + objectGID + ", it doesn't respect the following condition : objectGID < 0");
            var limit = reader.ReadUShort();
            var effects_ = new Types.ObjectEffect[limit];
            for (int i = 0; i < limit; i++)
            {
                 effects_[i] = Types.ProtocolTypeManager.GetInstance<Types.ObjectEffect>(reader.ReadShort());
                 effects_[i].Deserialize(reader);
            }
            effects = effects_;
            objectUID = reader.ReadInt();
            if (objectUID < 0)
                throw new Exception("Forbidden value on objectUID = " + objectUID + ", it doesn't respect the following condition : objectUID < 0");
            quantity = reader.ReadInt();
            if (quantity < 0)
                throw new Exception("Forbidden value on quantity = " + quantity + ", it doesn't respect the following condition : quantity < 0");
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + sizeof(short) + effects.Sum(x => sizeof(short) + x.GetSerializationSize()) + sizeof(int) + sizeof(int);
        }
        
    }
    
}