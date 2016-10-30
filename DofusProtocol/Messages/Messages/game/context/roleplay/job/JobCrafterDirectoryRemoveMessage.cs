

// Generated on 10/30/2016 16:20:31
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.Types;

namespace Stump.DofusProtocol.Messages
{
    public class JobCrafterDirectoryRemoveMessage : Message
    {
        public const uint Id = 5653;
        public override uint MessageId
        {
            get { return Id; }
        }
        
        public sbyte jobId;
        public long playerId;
        
        public JobCrafterDirectoryRemoveMessage()
        {
        }
        
        public JobCrafterDirectoryRemoveMessage(sbyte jobId, long playerId)
        {
            this.jobId = jobId;
            this.playerId = playerId;
        }
        
        public override void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(jobId);
            writer.WriteVarLong(playerId);
        }
        
        public override void Deserialize(IDataReader reader)
        {
            jobId = reader.ReadSByte();
            if (jobId < 0)
                throw new Exception("Forbidden value on jobId = " + jobId + ", it doesn't respect the following condition : jobId < 0");
            playerId = reader.ReadVarLong();
            if (playerId < 0 || playerId > 9007199254740990)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0 || playerId > 9007199254740990");
        }
        
    }
    
}