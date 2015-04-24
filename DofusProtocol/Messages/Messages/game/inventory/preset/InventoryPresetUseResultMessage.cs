

// Generated on 04/24/2015 03:38:15
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class InventoryPresetUseResultMessage : Message
    {
        public const uint Id = 6163;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte presetId;
        public sbyte code;
        public IEnumerable<byte> unlinkedPosition;
        
        public InventoryPresetUseResultMessage()
        {
        }
        
        public InventoryPresetUseResultMessage(sbyte presetId, sbyte code, IEnumerable<byte> unlinkedPosition)
        {
            this.presetId = presetId;
            this.code = code;
            this.unlinkedPosition = unlinkedPosition;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(presetId);
            writer.WriteSByte(code);
            var unlinkedPosition_before = writer.Position;
            var unlinkedPosition_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in unlinkedPosition)
            {
                 writer.WriteByte(entry);
                 unlinkedPosition_count++;
            }
            var unlinkedPosition_after = writer.Position;
            writer.Seek((int)unlinkedPosition_before);
            writer.WriteUShort((ushort)unlinkedPosition_count);
            writer.Seek((int)unlinkedPosition_after);

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
            var unlinkedPosition_ = new byte[limit];
            for (int i = 0; i < limit; i++)
            {
                 unlinkedPosition_[i] = reader.ReadByte();
            }
            unlinkedPosition = unlinkedPosition_;
        }
        
    }
    
}