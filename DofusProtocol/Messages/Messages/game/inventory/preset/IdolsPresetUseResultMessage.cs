

// Generated on 04/19/2016 10:17:38
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class IdolsPresetUseResultMessage : Message
    {
        public const uint Id = 6614;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte presetId;
        public sbyte code;
        public IEnumerable<short> missingIdols;
        
        public IdolsPresetUseResultMessage()
        {
        }
        
        public IdolsPresetUseResultMessage(sbyte presetId, sbyte code, IEnumerable<short> missingIdols)
        {
            this.presetId = presetId;
            this.code = code;
            this.missingIdols = missingIdols;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(presetId);
            writer.WriteSByte(code);
            var missingIdols_before = writer.Position;
            var missingIdols_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in missingIdols)
            {
                 writer.WriteVarShort(entry);
                 missingIdols_count++;
            }
            var missingIdols_after = writer.Position;
            writer.Seek((int)missingIdols_before);
            writer.WriteUShort((ushort)missingIdols_count);
            writer.Seek((int)missingIdols_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            presetId = reader.ReadSByte();
            if (presetId < 0)
                throw new Exception("Forbidden value on presetId = " + presetId + ", it doesn't respect the following condition : presetId < 0");
            code = reader.ReadSByte();
            if (code < 0)
                throw new Exception("Forbidden value on code = " + code + ", it doesn't respect the following condition : code < 0");
            var limit = reader.ReadUShort();
            var missingIdols_ = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 missingIdols_[i] = reader.ReadVarShort();
            }
            missingIdols = missingIdols_;
        }
        
    }
    
}