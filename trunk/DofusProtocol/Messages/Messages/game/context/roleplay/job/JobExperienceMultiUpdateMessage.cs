
// Generated on 03/25/2013 19:24:12
using System;
using System.Collections.Generic;
using System.Linq;
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
            writer.WriteUShort((ushort)experiencesUpdate.Count());
            foreach (var entry in experiencesUpdate)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            experiencesUpdate = new Types.JobExperience[limit];
            for (int i = 0; i < limit; i++)
            {
                 (experiencesUpdate as Types.JobExperience[])[i] = new Types.JobExperience();
                 (experiencesUpdate as Types.JobExperience[])[i].Deserialize(reader);
            }
        }
        
    }
    
}