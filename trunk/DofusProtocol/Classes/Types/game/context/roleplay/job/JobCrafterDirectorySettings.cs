using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class JobCrafterDirectorySettings : Object
	{
		public const uint protocolId = 97;
		public uint jobId = 0;
		public uint minSlot = 0;
		public uint userDefinedParams = 0;
		
		public JobCrafterDirectorySettings()
		{
		}
		
		public JobCrafterDirectorySettings(uint arg1, uint arg2, uint arg3)
			: this()
		{
			initJobCrafterDirectorySettings(arg1, arg2, arg3);
		}
		
		public virtual uint getTypeId()
		{
			return 97;
		}
		
		public JobCrafterDirectorySettings initJobCrafterDirectorySettings(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0)
		{
			this.jobId = arg1;
			this.minSlot = arg2;
			this.userDefinedParams = arg3;
			return this;
		}
		
		public virtual void reset()
		{
			this.jobId = 0;
			this.minSlot = 0;
			this.userDefinedParams = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_JobCrafterDirectorySettings(arg1);
		}
		
		public void serializeAs_JobCrafterDirectorySettings(BigEndianWriter arg1)
		{
			if ( this.jobId < 0 )
			{
				throw new Exception("Forbidden value (" + this.jobId + ") on element jobId.");
			}
			arg1.WriteByte((byte)this.jobId);
			if ( this.minSlot < 0 || this.minSlot > 9 )
			{
				throw new Exception("Forbidden value (" + this.minSlot + ") on element minSlot.");
			}
			arg1.WriteByte((byte)this.minSlot);
			if ( this.userDefinedParams < 0 )
			{
				throw new Exception("Forbidden value (" + this.userDefinedParams + ") on element userDefinedParams.");
			}
			arg1.WriteByte((byte)this.userDefinedParams);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_JobCrafterDirectorySettings(arg1);
		}
		
		public void deserializeAs_JobCrafterDirectorySettings(BigEndianReader arg1)
		{
			this.jobId = (uint)arg1.ReadByte();
			if ( this.jobId < 0 )
			{
				throw new Exception("Forbidden value (" + this.jobId + ") on element of JobCrafterDirectorySettings.jobId.");
			}
			this.minSlot = (uint)arg1.ReadByte();
			if ( this.minSlot < 0 || this.minSlot > 9 )
			{
				throw new Exception("Forbidden value (" + this.minSlot + ") on element of JobCrafterDirectorySettings.minSlot.");
			}
			this.userDefinedParams = (uint)arg1.ReadByte();
			if ( this.userDefinedParams < 0 )
			{
				throw new Exception("Forbidden value (" + this.userDefinedParams + ") on element of JobCrafterDirectorySettings.userDefinedParams.");
			}
		}
		
	}
}
