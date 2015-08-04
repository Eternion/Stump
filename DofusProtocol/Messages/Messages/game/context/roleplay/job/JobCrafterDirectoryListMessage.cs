

// Generated on 08/04/2015 13:25:00
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class JobCrafterDirectoryListMessage : Message
    {
        public const uint Id = 6046;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.JobCrafterDirectoryListEntry> listEntries;
        
        public JobCrafterDirectoryListMessage()
        {
        }
        
        public JobCrafterDirectoryListMessage(IEnumerable<Types.JobCrafterDirectoryListEntry> listEntries)
        {
            this.listEntries = listEntries;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var listEntries_before = writer.Position;
            var listEntries_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in listEntries)
            {
                 entry.Serialize(writer);
                 listEntries_count++;
            }
            var listEntries_after = writer.Position;
            writer.Seek((int)listEntries_before);
            writer.WriteUShort((ushort)listEntries_count);
            writer.Seek((int)listEntries_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var listEntries_ = new Types.JobCrafterDirectoryListEntry[limit];
            for (int i = 0; i < limit; i++)
            {
                 listEntries_[i] = new Types.JobCrafterDirectoryListEntry();
                 listEntries_[i].Deserialize(reader);
            }
            listEntries = listEntries_;
        }
        
    }
    
}