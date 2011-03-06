using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class JobLevelUpMessage : Message
	{
		public const uint protocolId = 5656;
		internal Boolean _isInitialized = false;
		public uint newLevel = 0;
		public JobDescription jobsDescription;
		
		public JobLevelUpMessage()
		{
			this.jobsDescription = new JobDescription();
		}
		
		public JobLevelUpMessage(uint arg1, JobDescription arg2)
			: this()
		{
			initJobLevelUpMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5656;
		}
		
		public JobLevelUpMessage initJobLevelUpMessage(uint arg1 = 0, JobDescription arg2 = null)
		{
			this.newLevel = arg1;
			this.jobsDescription = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.newLevel = 0;
			this.jobsDescription = new JobDescription();
			this._isInitialized = false;
		}
		
		public override void pack(BigEndianWriter arg1)
		{
			this.serialize(arg1);
			WritePacket(arg1, this.getMessageId());
		}
		
		public override void unpack(BigEndianReader arg1, uint arg2)
		{
			this.deserialize(arg1);
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_JobLevelUpMessage(arg1);
		}
		
		public void serializeAs_JobLevelUpMessage(BigEndianWriter arg1)
		{
			if ( this.newLevel < 0 )
			{
				throw new Exception("Forbidden value (" + this.newLevel + ") on element newLevel.");
			}
			arg1.WriteByte((byte)this.newLevel);
			this.jobsDescription.serializeAs_JobDescription(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_JobLevelUpMessage(arg1);
		}
		
		public void deserializeAs_JobLevelUpMessage(BigEndianReader arg1)
		{
			this.newLevel = (uint)arg1.ReadByte();
			if ( this.newLevel < 0 )
			{
				throw new Exception("Forbidden value (" + this.newLevel + ") on element of JobLevelUpMessage.newLevel.");
			}
			this.jobsDescription = new JobDescription();
			this.jobsDescription.deserialize(arg1);
		}
		
	}
}
