

// Generated on 10/30/2016 16:20:48
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class AccessoryPreviewRequestMessage : Message
    {
        public const uint Id = 6518;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<short> genericId;
        
        public AccessoryPreviewRequestMessage()
        {
        }
        
        public AccessoryPreviewRequestMessage(IEnumerable<short> genericId)
        {
            this.genericId = genericId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var genericId_before = writer.Position;
            var genericId_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in genericId)
            {
                 writer.WriteVarShort(entry);
                 genericId_count++;
            }
            var genericId_after = writer.Position;
            writer.Seek((int)genericId_before);
            writer.WriteUShort((ushort)genericId_count);
            writer.Seek((int)genericId_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var genericId_ = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 genericId_[i] = reader.ReadVarShort();
            }
            genericId = genericId_;
        }
        
    }
    
}