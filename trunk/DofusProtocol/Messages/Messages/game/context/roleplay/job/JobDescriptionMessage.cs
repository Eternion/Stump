

// Generated on 07/29/2013 23:07:59
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class JobDescriptionMessage : Message
    {
        public const uint Id = 5655;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public IEnumerable<Types.JobDescription> jobsDescription;
        
        public JobDescriptionMessage()
        {
        }
        
        public JobDescriptionMessage(IEnumerable<Types.JobDescription> jobsDescription)
        {
            this.jobsDescription = jobsDescription;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteUShort((ushort)jobsDescription.Count());
            foreach (var entry in jobsDescription)
            {
                 entry.Serialize(writer);
            }
        }
        
        public override void Deserialize(IDataReader reader)
        {
            var limit = reader.ReadUShort();
            jobsDescription = new Types.JobDescription[limit];
            for (int i = 0; i < limit; i++)
            {
                 (jobsDescription as Types.JobDescription[])[i] = new Types.JobDescription();
                 (jobsDescription as Types.JobDescription[])[i].Deserialize(reader);
            }
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(short) + jobsDescription.Sum(x => x.GetSerializationSize());
        }
        
    }
    
}