

// Generated on 02/19/2015 12:10:39
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class JobCrafterDirectorySettings
    {
        public const short Id = 97;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public sbyte jobId;
        public sbyte minSlot;
        public sbyte userDefinedParams;
        
        public JobCrafterDirectorySettings()
        {
        }
        
        public JobCrafterDirectorySettings(sbyte jobId, sbyte minSlot, sbyte userDefinedParams)
        {
            this.jobId = jobId;
            this.minSlot = minSlot;
            this.userDefinedParams = userDefinedParams;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(jobId);
            writer.WriteSByte(minSlot);
            writer.WriteSByte(userDefinedParams);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            jobId = reader.ReadSByte();
            if (jobId < 0)
                throw new Exception("Forbidden value on jobId = " + jobId + ", it doesn't respect the following condition : jobId < 0");
            minSlot = reader.ReadSByte();
            if (minSlot < 0 || minSlot > 9)
                throw new Exception("Forbidden value on minSlot = " + minSlot + ", it doesn't respect the following condition : minSlot < 0 || minSlot > 9");
            userDefinedParams = reader.ReadSByte();
            if (userDefinedParams < 0)
                throw new Exception("Forbidden value on userDefinedParams = " + userDefinedParams + ", it doesn't respect the following condition : userDefinedParams < 0");
        }
        
        
    }
    
}