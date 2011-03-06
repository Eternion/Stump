using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class JobExperienceUpdateMessage : Message
	{
		public const uint protocolId = 5654;
		internal Boolean _isInitialized = false;
		public JobExperience experiencesUpdate;
		
		public JobExperienceUpdateMessage()
		{
			this.experiencesUpdate = new JobExperience();
		}
		
		public JobExperienceUpdateMessage(JobExperience arg1)
			: this()
		{
			initJobExperienceUpdateMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5654;
		}
		
		public JobExperienceUpdateMessage initJobExperienceUpdateMessage(JobExperience arg1 = null)
		{
			this.experiencesUpdate = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.experiencesUpdate = new JobExperience();
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
			this.serializeAs_JobExperienceUpdateMessage(arg1);
		}
		
		public void serializeAs_JobExperienceUpdateMessage(BigEndianWriter arg1)
		{
			this.experiencesUpdate.serializeAs_JobExperience(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_JobExperienceUpdateMessage(arg1);
		}
		
		public void deserializeAs_JobExperienceUpdateMessage(BigEndianReader arg1)
		{
			this.experiencesUpdate = new JobExperience();
			this.experiencesUpdate.deserialize(arg1);
		}
		
	}
}
