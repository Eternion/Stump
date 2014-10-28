

// Generated on 10/28/2014 16:38:03
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
    public class JobCrafterDirectoryListEntry
    {
        public const short Id = 196;
        public virtual short TypeId
        {
            get { return Id; }
        }
        
        public Types.JobCrafterDirectoryEntryPlayerInfo playerInfo;
        public Types.JobCrafterDirectoryEntryJobInfo jobInfo;
        
        public JobCrafterDirectoryListEntry()
        {
        }
        
        public JobCrafterDirectoryListEntry(Types.JobCrafterDirectoryEntryPlayerInfo playerInfo, Types.JobCrafterDirectoryEntryJobInfo jobInfo)
        {
            this.playerInfo = playerInfo;
            this.jobInfo = jobInfo;
        }
        
        public virtual void Serialize(IDataWriter writer)
        {
            playerInfo.Serialize(writer);
            jobInfo.Serialize(writer);
        }
        
        public virtual void Deserialize(IDataReader reader)
        {
            playerInfo = new Types.JobCrafterDirectoryEntryPlayerInfo();
            playerInfo.Deserialize(reader);
            jobInfo = new Types.JobCrafterDirectoryEntryJobInfo();
            jobInfo.Deserialize(reader);
        }
        
        public virtual int GetSerializationSize()
        {
            return playerInfo.GetSerializationSize() + jobInfo.GetSerializationSize();
        }
        
    }
    
}