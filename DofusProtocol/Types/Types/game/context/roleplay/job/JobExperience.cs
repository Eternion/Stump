

// Generated on 10/30/2016 16:20:56
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class JobExperience
    {
        public const short Id = 98;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public sbyte jobId;
        public byte jobLevel;
        public long jobXP;
        public long jobXpLevelFloor;
        public long jobXpNextLevelFloor;
        
        public JobExperience()
        {
        }
        
        public JobExperience(sbyte jobId, byte jobLevel, long jobXP, long jobXpLevelFloor, long jobXpNextLevelFloor)
        {
            this.jobId = jobId;
            this.jobLevel = jobLevel;
            this.jobXP = jobXP;
            this.jobXpLevelFloor = jobXpLevelFloor;
            this.jobXpNextLevelFloor = jobXpNextLevelFloor;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            writer.WriteSByte(jobId);
            writer.WriteByte(jobLevel);
            writer.WriteVarLong(jobXP);
            writer.WriteVarLong(jobXpLevelFloor);
            writer.WriteVarLong(jobXpNextLevelFloor);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            jobId = reader.ReadSByte();
            if (jobId < 0)
                throw new Exception("Forbidden value on jobId = " + jobId + ", it doesn't respect the following condition : jobId < 0");
            jobLevel = reader.ReadByte();
            if (jobLevel < 0 || jobLevel > 255)
                throw new Exception("Forbidden value on jobLevel = " + jobLevel + ", it doesn't respect the following condition : jobLevel < 0 || jobLevel > 255");
            jobXP = reader.ReadVarLong();
            if (jobXP < 0 || jobXP > 9007199254740990)
                throw new Exception("Forbidden value on jobXP = " + jobXP + ", it doesn't respect the following condition : jobXP < 0 || jobXP > 9007199254740990");
            jobXpLevelFloor = reader.ReadVarLong();
            if (jobXpLevelFloor < 0 || jobXpLevelFloor > 9007199254740990)
                throw new Exception("Forbidden value on jobXpLevelFloor = " + jobXpLevelFloor + ", it doesn't respect the following condition : jobXpLevelFloor < 0 || jobXpLevelFloor > 9007199254740990");
            jobXpNextLevelFloor = reader.ReadVarLong();
            if (jobXpNextLevelFloor < 0 || jobXpNextLevelFloor > 9007199254740990)
                throw new Exception("Forbidden value on jobXpNextLevelFloor = " + jobXpNextLevelFloor + ", it doesn't respect the following condition : jobXpNextLevelFloor < 0 || jobXpNextLevelFloor > 9007199254740990");
        }
        
        
    }
    
}