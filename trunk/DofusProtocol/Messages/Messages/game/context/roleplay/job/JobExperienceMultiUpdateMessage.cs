using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class JobExperienceMultiUpdateMessage : Message
	{
		public const uint protocolId = 5809;
		internal Boolean _isInitialized = false;
		public List<JobExperience> experiencesUpdate;
		
		public JobExperienceMultiUpdateMessage()
		{
			this.experiencesUpdate = new List<JobExperience>();
		}
		
		public JobExperienceMultiUpdateMessage(List<JobExperience> arg1)
			: this()
		{
			initJobExperienceMultiUpdateMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5809;
		}
		
		public JobExperienceMultiUpdateMessage initJobExperienceMultiUpdateMessage(List<JobExperience> arg1)
		{
			this.experiencesUpdate = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.experiencesUpdate = new List<JobExperience>();
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
			this.serializeAs_JobExperienceMultiUpdateMessage(arg1);
		}
		
		public void serializeAs_JobExperienceMultiUpdateMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.experiencesUpdate.Count);
			var loc1 = 0;
			while ( loc1 < this.experiencesUpdate.Count )
			{
				this.experiencesUpdate[loc1].serializeAs_JobExperience(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_JobExperienceMultiUpdateMessage(arg1);
		}
		
		public void deserializeAs_JobExperienceMultiUpdateMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new JobExperience()) as JobExperience).deserialize(arg1);
				this.experiencesUpdate.Add((JobExperience)loc3);
				++loc2;
			}
		}
		
	}
}
