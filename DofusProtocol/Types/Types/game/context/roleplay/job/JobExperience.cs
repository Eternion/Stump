

// Generated on 12/29/2014 21:14:31
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
        public sbyte jobLevel;
        public long jobXP;
        public long jobXpLevelFloor;
        public long jobXpNextLevelFloor;
        
        public JobExperience()
        {
        }
        
        public JobExperience(sbyte jobId, sbyte jobLevel, long jobXP, long jobXpLevelFloor, long jobXpNextLevelFloor)
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
            writer.WriteSByte(jobLevel);
            writer.WriteLong(jobXP);
            writer.WriteLong(jobXpLevelFloor);
            writer.WriteLong(jobXpNextLevelFloor);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            jobId = reader.ReadSByte();
            if (jobId < 0)
                throw new Exception("Forbidden value on jobId = " + jobId + ", it doesn't respect the following condition : jobId < 0");
            jobLevel = reader.ReadSByte();
            if (jobLevel < 0)
                throw new Exception("Forbidden value on jobLevel = " + jobLevel + ", it doesn't respect the following condition : jobLevel < 0");
            jobXP = reader.ReadLong();
            if (jobXP < 0 || jobXP > 9.007199254740992E15)
                throw new Exception("Forbidden value on jobXP = " + jobXP + ", it doesn't respect the following condition : jobXP < 0 || jobXP > 9.007199254740992E15");
            jobXpLevelFloor = reader.ReadLong();
            if (jobXpLevelFloor < 0 || jobXpLevelFloor > 9.007199254740992E15)
                throw new Exception("Forbidden value on jobXpLevelFloor = " + jobXpLevelFloor + ", it doesn't respect the following condition : jobXpLevelFloor < 0 || jobXpLevelFloor > 9.007199254740992E15");
            jobXpNextLevelFloor = reader.ReadLong();
            if (jobXpNextLevelFloor < 0 || jobXpNextLevelFloor > 9.007199254740992E15)
                throw new Exception("Forbidden value on jobXpNextLevelFloor = " + jobXpNextLevelFloor + ", it doesn't respect the following condition : jobXpNextLevelFloor < 0 || jobXpNextLevelFloor > 9.007199254740992E15");
        }
        
        public virtual int GetSerializationSize()
        {
            return sizeof(sbyte) + sizeof(sbyte) + sizeof(long) + sizeof(long) + sizeof(long);
        }
        
    }
    
}