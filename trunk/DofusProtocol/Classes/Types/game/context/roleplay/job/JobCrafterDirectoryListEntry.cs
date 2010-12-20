using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class JobCrafterDirectoryListEntry : Object
	{
		public const uint protocolId = 196;
		public JobCrafterDirectoryEntryPlayerInfo playerInfo;
		public JobCrafterDirectoryEntryJobInfo jobInfo;
		
		public JobCrafterDirectoryListEntry()
		{
			this.playerInfo = new JobCrafterDirectoryEntryPlayerInfo();
			this.jobInfo = new JobCrafterDirectoryEntryJobInfo();
		}
		
		public JobCrafterDirectoryListEntry(JobCrafterDirectoryEntryPlayerInfo arg1, JobCrafterDirectoryEntryJobInfo arg2)
			: this()
		{
			initJobCrafterDirectoryListEntry(arg1, arg2);
		}
		
		public virtual uint getTypeId()
		{
			return 196;
		}
		
		public JobCrafterDirectoryListEntry initJobCrafterDirectoryListEntry(JobCrafterDirectoryEntryPlayerInfo arg1 = null, JobCrafterDirectoryEntryJobInfo arg2 = null)
		{
			this.playerInfo = arg1;
			this.jobInfo = arg2;
			return this;
		}
		
		public virtual void reset()
		{
			this.playerInfo = new JobCrafterDirectoryEntryPlayerInfo();
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_JobCrafterDirectoryListEntry(arg1);
		}
		
		public void serializeAs_JobCrafterDirectoryListEntry(BigEndianWriter arg1)
		{
			this.playerInfo.serializeAs_JobCrafterDirectoryEntryPlayerInfo(arg1);
			this.jobInfo.serializeAs_JobCrafterDirectoryEntryJobInfo(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_JobCrafterDirectoryListEntry(arg1);
		}
		
		public void deserializeAs_JobCrafterDirectoryListEntry(BigEndianReader arg1)
		{
			this.playerInfo = new JobCrafterDirectoryEntryPlayerInfo();
			this.playerInfo.deserialize(arg1);
			this.jobInfo = new JobCrafterDirectoryEntryJobInfo();
			this.jobInfo.deserialize(arg1);
		}
		
	}
}
