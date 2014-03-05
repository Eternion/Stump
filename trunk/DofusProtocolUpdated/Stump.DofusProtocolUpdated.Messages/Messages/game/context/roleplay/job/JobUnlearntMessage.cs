

// Generated on 03/05/2014 20:34:29
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class JobUnlearntMessage : Message
    {
        public const uint Id = 5657;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte jobId;
        
        public JobUnlearntMessage()
        {
        }
        
        public JobUnlearntMessage(sbyte jobId)
        {
            this.jobId = jobId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(jobId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            jobId = reader.ReadSByte();
            if (jobId < 0)
                throw new Exception("Forbidden value on jobId = " + jobId + ", it doesn't respect the following condition : jobId < 0");
        }
        
        public override int GetSerializationSize()
        {
            return sizeof(sbyte);
        }
        
    }
    
}