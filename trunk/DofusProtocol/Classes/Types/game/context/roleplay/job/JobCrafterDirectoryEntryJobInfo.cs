using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class JobCrafterDirectoryEntryJobInfo : Object
	{
		public const uint protocolId = 195;
		public uint jobId = 0;
		public uint jobLevel = 0;
		public uint userDefinedParams = 0;
		public uint minSlots = 0;
		
		public JobCrafterDirectoryEntryJobInfo()
		{
		}
		
		public JobCrafterDirectoryEntryJobInfo(uint arg1, uint arg2, uint arg3, uint arg4)
			: this()
		{
			initJobCrafterDirectoryEntryJobInfo(arg1, arg2, arg3, arg4);
		}
		
		public virtual uint getTypeId()
		{
			return 195;
		}
		
		public JobCrafterDirectoryEntryJobInfo initJobCrafterDirectoryEntryJobInfo(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0, uint arg4 = 0)
		{
			this.jobId = arg1;
			this.jobLevel = arg2;
			this.userDefinedParams = arg3;
			this.minSlots = arg4;
			return this;
		}
		
		public virtual void reset()
		{
			this.jobId = 0;
			this.jobLevel = 0;
			this.userDefinedParams = 0;
			this.minSlots = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_JobCrafterDirectoryEntryJobInfo(arg1);
		}
		
		public void serializeAs_JobCrafterDirectoryEntryJobInfo(BigEndianWriter arg1)
		{
			if ( this.jobId < 0 )
			{
				throw new Exception("Forbidden value (" + this.jobId + ") on element jobId.");
			}
			arg1.WriteByte((byte)this.jobId);
			if ( this.jobLevel < 1 || this.jobLevel > 100 )
			{
				throw new Exception("Forbidden value (" + this.jobLevel + ") on element jobLevel.");
			}
			arg1.WriteByte((byte)this.jobLevel);
			if ( this.userDefinedParams < 0 )
			{
				throw new Exception("Forbidden value (" + this.userDefinedParams + ") on element userDefinedParams.");
			}
			arg1.WriteByte((byte)this.userDefinedParams);
			if ( this.minSlots < 0 || this.minSlots > 9 )
			{
				throw new Exception("Forbidden value (" + this.minSlots + ") on element minSlots.");
			}
			arg1.WriteByte((byte)this.minSlots);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_JobCrafterDirectoryEntryJobInfo(arg1);
		}
		
		public void deserializeAs_JobCrafterDirectoryEntryJobInfo(BigEndianReader arg1)
		{
			this.jobId = (uint)arg1.ReadByte();
			if ( this.jobId < 0 )
			{
				throw new Exception("Forbidden value (" + this.jobId + ") on element of JobCrafterDirectoryEntryJobInfo.jobId.");
			}
			this.jobLevel = (uint)arg1.ReadByte();
			if ( this.jobLevel < 1 || this.jobLevel > 100 )
			{
				throw new Exception("Forbidden value (" + this.jobLevel + ") on element of JobCrafterDirectoryEntryJobInfo.jobLevel.");
			}
			this.userDefinedParams = (uint)arg1.ReadByte();
			if ( this.userDefinedParams < 0 )
			{
				throw new Exception("Forbidden value (" + this.userDefinedParams + ") on element of JobCrafterDirectoryEntryJobInfo.userDefinedParams.");
			}
			this.minSlots = (uint)arg1.ReadByte();
			if ( this.minSlots < 0 || this.minSlots > 9 )
			{
				throw new Exception("Forbidden value (" + this.minSlots + ") on element of JobCrafterDirectoryEntryJobInfo.minSlots.");
			}
		}
		
	}
}
