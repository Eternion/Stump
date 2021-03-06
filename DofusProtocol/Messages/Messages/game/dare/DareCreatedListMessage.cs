

// Generated on 10/30/2016 16:20:36
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class DareCreatedListMessage : Message
    {
        public const uint Id = 6663;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.DareInformations> daresFixedInfos;
        public IEnumerable<Types.DareVersatileInformations> daresVersatilesInfos;
        
        public DareCreatedListMessage()
        {
        }
        
        public DareCreatedListMessage(IEnumerable<Types.DareInformations> daresFixedInfos, IEnumerable<Types.DareVersatileInformations> daresVersatilesInfos)
        {
            this.daresFixedInfos = daresFixedInfos;
            this.daresVersatilesInfos = daresVersatilesInfos;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var daresFixedInfos_before = writer.Position;
            var daresFixedInfos_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in daresFixedInfos)
            {
                 entry.Serialize(writer);
                 daresFixedInfos_count++;
            }
            var daresFixedInfos_after = writer.Position;
            writer.Seek((int)daresFixedInfos_before);
            writer.WriteUShort((ushort)daresFixedInfos_count);
            writer.Seek((int)daresFixedInfos_after);

            var daresVersatilesInfos_before = writer.Position;
            var daresVersatilesInfos_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in daresVersatilesInfos)
            {
                 entry.Serialize(writer);
                 daresVersatilesInfos_count++;
            }
            var daresVersatilesInfos_after = writer.Position;
            writer.Seek((int)daresVersatilesInfos_before);
            writer.WriteUShort((ushort)daresVersatilesInfos_count);
            writer.Seek((int)daresVersatilesInfos_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var daresFixedInfos_ = new Types.DareInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 daresFixedInfos_[i] = new Types.DareInformations();
                 daresFixedInfos_[i].Deserialize(reader);
            }
            daresFixedInfos = daresFixedInfos_;
            limit = reader.ReadUShort();
            var daresVersatilesInfos_ = new Types.DareVersatileInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 daresVersatilesInfos_[i] = new Types.DareVersatileInformations();
                 daresVersatilesInfos_[i].Deserialize(reader);
            }
            daresVersatilesInfos = daresVersatilesInfos_;
        }
        
    }
    
}