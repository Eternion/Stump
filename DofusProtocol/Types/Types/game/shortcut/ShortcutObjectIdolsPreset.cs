

// Generated on 04/19/2016 10:17:54
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class ShortcutObjectIdolsPreset : ShortcutObject
    {
        public const short Id = 492;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public sbyte presetId;
        
        public ShortcutObjectIdolsPreset()
        {
        }
        
        public ShortcutObjectIdolsPreset(sbyte slot, sbyte presetId)
         : base(slot)
        {
            this.presetId = presetId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(presetId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            presetId = reader.ReadSByte();
            if (presetId < 0)
                throw new Exception("Forbidden value on presetId = " + presetId + ", it doesn't respect the following condition : presetId < 0");
        }
        
        
    }
    
}