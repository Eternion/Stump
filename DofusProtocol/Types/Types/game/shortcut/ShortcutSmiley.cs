

// Generated on 02/18/2015 11:05:55
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class ShortcutSmiley : Shortcut
    {
        public const short Id = 388;
        public override short TypeId
        {
            get { return Id; }
        }
        
        public sbyte smileyId;
        
        public ShortcutSmiley()
        {
        }
        
        public ShortcutSmiley(sbyte slot, sbyte smileyId)
         : base(slot)
        {
            this.smileyId = smileyId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            base.Serialize(writer);
            writer.WriteSByte(smileyId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            base.Deserialize(reader);
            smileyId = reader.ReadSByte();
            if (smileyId < 0)
                throw new Exception("Forbidden value on smileyId = " + smileyId + ", it doesn't respect the following condition : smileyId < 0");
        }
        
        
    }
    
}