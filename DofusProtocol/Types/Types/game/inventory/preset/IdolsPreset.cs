

// Generated on 12/20/2015 17:31:00
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class IdolsPreset
    {
        public const short Id = 491;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public sbyte presetId;
        public sbyte symbolId;
        public IEnumerable<short> idolId;
        
        public IdolsPreset()
        {
        }
        
        public IdolsPreset(sbyte presetId, sbyte symbolId, IEnumerable<short> idolId)
        {
            this.presetId = presetId;
            this.symbolId = symbolId;
            this.idolId = idolId;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(presetId);
            writer.WriteSByte(symbolId);
            var idolId_before = writer.Position;
            var idolId_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in idolId)
            {
                 writer.WriteVarShort(entry);
                 idolId_count++;
            }
            var idolId_after = writer.Position;
            writer.Seek((int)idolId_before);
            writer.WriteUShort((ushort)idolId_count);
            writer.Seek((int)idolId_after);

        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            presetId = reader.ReadSByte();
            if (presetId < 0)
                throw new Exception("Forbidden value on presetId = " + presetId + ", it doesn't respect the following condition : presetId < 0");
            symbolId = reader.ReadSByte();
            if (symbolId < 0)
                throw new Exception("Forbidden value on symbolId = " + symbolId + ", it doesn't respect the following condition : symbolId < 0");
            var limit = reader.ReadUShort();
            var idolId_ = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 idolId_[i] = reader.ReadVarShort();
            }
            idolId = idolId_;
        }
        
        
    }
    
}