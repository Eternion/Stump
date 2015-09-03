

// Generated on 09/01/2015 10:48:10
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class JobBookSubscriptionMessage : Message
    {
        public const uint Id = 6593;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public bool addedOrDeleted;
        public sbyte jobId;
        
        public JobBookSubscriptionMessage()
        {
        }
        
        public JobBookSubscriptionMessage(bool addedOrDeleted, sbyte jobId)
        {
            this.addedOrDeleted = addedOrDeleted;
            this.jobId = jobId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteBoolean(addedOrDeleted);
            writer.WriteSByte(jobId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            addedOrDeleted = reader.ReadBoolean();
            jobId = reader.ReadSByte();
            if (jobId < 0)
                throw new Exception("Forbidden value on jobId = " + jobId + ", it doesn't respect the following condition : jobId < 0");
        }
        
    }
    
}