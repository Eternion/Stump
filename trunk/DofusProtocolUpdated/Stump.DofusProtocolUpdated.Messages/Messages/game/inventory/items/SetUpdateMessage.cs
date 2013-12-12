

// Generated on 12/12/2013 16:57:21
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class SetUpdateMessage : Message
    {
        public const uint Id = 5503;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public short setId;
        public IEnumerable<short> setObjects;
        public IEnumerable<Types.ObjectEffect> setEffects;
        
        public SetUpdateMessage()
        {
        }
        
        public SetUpdateMessage(short setId, IEnumerable<short> setObjects, IEnumerable<Types.ObjectEffect> setEffects)
        {
            this.setId = setId;
            this.setObjects = setObjects;
            this.setEffects = setEffects;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteShort(setId);
            writer.WriteUShort((ushort)setObjects.Count());
            foreach (var entry in setObjects)
            {
                 writer.WriteShort(entry);
            }
            writer.WriteUShort((ushort)setEffects.Count());
            foreach (var entry in setEffects)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            setId = reader.ReadShort();
            if (setId < 0)
                throw new Exception("Forbidden value on setId = " + setId + ", it doesn't respect the following condition : setId < 0");
            var limit = reader.ReadUShort();
            setObjects = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (setObjects as short[])[i] = reader.ReadShort();
            }
            limit = reader.ReadUShort();
            setEffects = new Types.ObjectEffect[limit];
            for (int i = 0; i < limit; i++)
            {
                 (setEffects as Types.ObjectEffect[])[i] = Types.ProtocolTypeManager.GetInstance<Types.ObjectEffect>(reader.ReadShort());
                 (setEffects as Types.ObjectEffect[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + sizeof(short) + setObjects.Sum(x => sizeof(short)) + sizeof(short) + setEffects.Sum(x => sizeof(short) + x.GetSerializationSize());
        }
        
    }
    
}