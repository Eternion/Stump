using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class JobDescriptionMessage : Message
	{
		public const uint protocolId = 5655;
		internal Boolean _isInitialized = false;
		public List<JobDescription> jobsDescription;
		
		public JobDescriptionMessage()
		{
			this.jobsDescription = new List<JobDescription>();
		}
		
		public JobDescriptionMessage(List<JobDescription> arg1)
			: this()
		{
			initJobDescriptionMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5655;
		}
		
		public JobDescriptionMessage initJobDescriptionMessage(List<JobDescription> arg1)
		{
			this.jobsDescription = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.jobsDescription = new List<JobDescription>();
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
			this.serializeAs_JobDescriptionMessage(arg1);
		}
		
		public void serializeAs_JobDescriptionMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.jobsDescription.Count);
			var loc1 = 0;
			while ( loc1 < this.jobsDescription.Count )
			{
				this.jobsDescription[loc1].serializeAs_JobDescription(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_JobDescriptionMessage(arg1);
		}
		
		public void deserializeAs_JobDescriptionMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new JobDescription()) as JobDescription).deserialize(arg1);
				this.jobsDescription.Add((JobDescription)loc3);
				++loc2;
			}
		}
		
	}
}
