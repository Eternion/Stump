

// Generated on 12/29/2014 21:12:39
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class JobExperienceMultiUpdateMessage : Message
    {
        public const uint Id = 5809;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.JobExperience> experiencesUpdate;
        
        public JobExperienceMultiUpdateMessage()
        {
        }
        
        public JobExperienceMultiUpdateMessage(IEnumerable<Types.JobExperience> experiencesUpdate)
        {
            this.experiencesUpdate = experiencesUpdate;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            var experiencesUpdate_before = writer.Position;
            var experiencesUpdate_count = 0;
            writer.WriteUShort(0);
            foreach (var entry in experiencesUpdate)
            {
                 entry.Serialize(writer);
                 experiencesUpdate_count++;
            }
            var experiencesUpdate_after = writer.Position;
            writer.Seek((int)experiencesUpdate_before);
            writer.WriteUShort((ushort)experiencesUpdate_count);
            writer.Seek((int)experiencesUpdate_after);

        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            var experiencesUpdate_ = new Types.JobExperience[limit];
            for (int i = 0; i < limit; i++)
            {
                 experiencesUpdate_[i] = new Types.JobExperience();
                 experiencesUpdate_[i].Deserialize(reader);
            }
            experiencesUpdate = experiencesUpdate_;
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + experiencesUpdate.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}