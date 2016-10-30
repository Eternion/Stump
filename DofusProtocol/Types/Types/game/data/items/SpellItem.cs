

// Generated on 10/30/2016 16:20:57
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class SpellItem : Item
    {
        public const short Id = 49;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public int spellId;
        public sbyte spellLevel;
        
        public SpellItem()
        {
        }
        
        public SpellItem(int spellId, sbyte spellLevel)
        {
            this.spellId = spellId;
            this.spellLevel = spellLevel;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteInt(spellId);
            writer.WriteSByte(spellLevel);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            spellId = reader.ReadInt();
            spellLevel = reader.ReadSByte();
            if (spellLevel < 1 || spellLevel > 6)
                throw new Exception("Forbidden value on spellLevel = " + spellLevel + ", it doesn't respect the following condition : spellLevel < 1 || spellLevel > 6");
        }
        
        
    }
    
}