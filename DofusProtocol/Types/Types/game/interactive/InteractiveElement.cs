

// Generated on 04/19/2016 10:17:54
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class InteractiveElement
    {
        public const short Id = 80;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public int elementId;
        public int elementTypeId;
        public IEnumerable<Types.InteractiveElementSkill> enabledSkills;
        public IEnumerable<Types.InteractiveElementSkill> disabledSkills;
        
        public InteractiveElement()
        {
        }
        
        public InteractiveElement(int elementId, int elementTypeId, IEnumerable<Types.InteractiveElementSkill> enabledSkills, IEnumerable<Types.InteractiveElementSkill> disabledSkills)
        {
            this.elementId = elementId;
            this.elementTypeId = elementTypeId;
            this.enabledSkills = enabledSkills;
            this.disabledSkills = disabledSkills;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteInt(elementId);
            writer.WriteInt(elementTypeId);
            var enabledSkills_before = writer.Position;
            var enabledSkills_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in enabledSkills)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 enabledSkills_count++;
            }
            var enabledSkills_after = writer.Position;
            writer.Seek((int)enabledSkills_before);
            writer.WriteUShort((ushort)enabledSkills_count);
            writer.Seek((int)enabledSkills_after);

            var disabledSkills_before = writer.Position;
            var disabledSkills_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in disabledSkills)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
                 disabledSkills_count++;
            }
            var disabledSkills_after = writer.Position;
            writer.Seek((int)disabledSkills_before);
            writer.WriteUShort((ushort)disabledSkills_count);
            writer.Seek((int)disabledSkills_after);

        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            elementId = reader.ReadInt();
            if (elementId < 0)
                throw new Exception("Forbidden value on elementId = " + elementId + ", it doesn't respect the following condition : elementId < 0");
            elementTypeId = reader.ReadInt();
            var limit = reader.ReadUShort();
            var enabledSkills_ = new Types.InteractiveElementSkill[limit];
            for (int i = 0; i < limit; i++)
            {
                 enabledSkills_[i] = Types.ProtocolTypeManager.GetInstance<Types.InteractiveElementSkill>(reader.ReadShort());
                 enabledSkills_[i].Deserialize(reader);
            }
            enabledSkills = enabledSkills_;
            limit = reader.ReadUShort();
            var disabledSkills_ = new Types.InteractiveElementSkill[limit];
            for (int i = 0; i < limit; i++)
            {
                 disabledSkills_[i] = Types.ProtocolTypeManager.GetInstance<Types.InteractiveElementSkill>(reader.ReadShort());
                 disabledSkills_[i].Deserialize(reader);
            }
            disabledSkills = disabledSkills_;
        }
        
        
    }
    
}