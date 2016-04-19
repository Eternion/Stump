

// Generated on 04/19/2016 10:17:37
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class ObjectJobAddedMessage : Message
    {
        public const uint Id = 6014;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte jobId;
        
        public ObjectJobAddedMessage()
        {
        }
        
        public ObjectJobAddedMessage(sbyte jobId)
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
        
    }
    
}