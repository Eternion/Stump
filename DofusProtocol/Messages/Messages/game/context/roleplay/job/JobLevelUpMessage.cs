

// Generated on 10/30/2016 16:20:32
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class JobLevelUpMessage : Message
    {
        public const uint Id = 5656;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public byte newLevel;
        public Types.JobDescription jobsDescription;
        
        public JobLevelUpMessage()
        {
        }
        
        public JobLevelUpMessage(byte newLevel, Types.JobDescription jobsDescription)
        {
            this.newLevel = newLevel;
            this.jobsDescription = jobsDescription;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteByte(newLevel);
            jobsDescription.Serialize(writer);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            newLevel = reader.ReadByte();
            if (newLevel < 0 || newLevel > 255)
                throw new Exception("Forbidden value on newLevel = " + newLevel + ", it doesn't respect the following condition : newLevel < 0 || newLevel > 255");
            jobsDescription = new Types.JobDescription();
            jobsDescription.Deserialize(reader);
        }
        
    }
    
}