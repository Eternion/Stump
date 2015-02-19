

// Generated on 02/18/2015 11:05:55
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class Preset
    {
        public const short Id = 355;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public sbyte presetId;
        public sbyte symbolId;
        public bool mount;
        public IEnumerable<Types.PresetItem> objects;
        
        public Preset()
        {
        }
        
        public Preset(sbyte presetId, sbyte symbolId, bool mount, IEnumerable<Types.PresetItem> objects)
        {
            this.presetId = presetId;
            this.symbolId = symbolId;
            this.mount = mount;
            this.objects = objects;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(presetId);
            writer.WriteSByte(symbolId);
            writer.WriteBoolean(mount);
            var objects_before = writer.Position;
            var objects_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in objects)
            {
                 entry.Serialize(writer);
                 objects_count++;
            }
            var objects_after = writer.Position;
            writer.Seek((int)objects_before);
            writer.WriteUShort((ushort)objects_count);
            writer.Seek((int)objects_after);

        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            presetId = reader.ReadSByte();
            if (presetId < 0)
                throw new Exception("Forbidden value on presetId = " + presetId + ", it doesn't respect the following condition : presetId < 0");
            symbolId = reader.ReadSByte();
            if (symbolId < 0)
                throw new Exception("Forbidden value on symbolId = " + symbolId + ", it doesn't respect the following condition : symbolId < 0");
            mount = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            var objects_ = new Types.PresetItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 objects_[i] = new Types.PresetItem();
                 objects_[i].Deserialize(reader);
            }
            objects = objects_;
        }
        
        
    }
    
}