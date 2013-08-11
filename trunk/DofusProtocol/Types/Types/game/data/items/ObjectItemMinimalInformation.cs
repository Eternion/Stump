

// Generated on 08/11/2013 11:29:16
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class ObjectItemMinimalInformation : Item
    {
        public const short Id = 124;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public short objectGID;
        public short powerRate;
        public bool overMax;
        public IEnumerable<Types.ObjectEffect> effects;
        
        public ObjectItemMinimalInformation()
        {
        }
        
        public ObjectItemMinimalInformation(short objectGID, short powerRate, bool overMax, IEnumerable<Types.ObjectEffect> effects)
        {
            this.objectGID = objectGID;
            this.powerRate = powerRate;
            this.overMax = overMax;
            this.effects = effects;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteShort(objectGID);
            writer.WriteShort(powerRate);
            writer.WriteBoolean(overMax);
            writer.WriteUShort((ushort)effects.Count());
            foreach (var entry in effects)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            objectGID = reader.ReadShort();
            if (objectGID < 0)
                throw new Exception("Forbidden value on objectGID = " + objectGID + ", it doesn't respect the following condition : objectGID < 0");
            powerRate = reader.ReadShort();
            overMax = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            effects = new Types.ObjectEffect[limit];
            for (int i = 0; i < limit; i++)
            {
                 (effects as Types.ObjectEffect[])[i] = Types.ProtocolTypeManager.GetInstance<Types.ObjectEffect>(reader.ReadShort());
                 (effects as Types.ObjectEffect[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return base.GetSerializationSize() + sizeof(short) + sizeof(short) + sizeof(bool) + sizeof(short) + effects.Sum(x => sizeof(short) + x.GetSerializationSize());
        }
        
    }
    
}