using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class JobDescription : Object
	{
		public const uint protocolId = 101;
		public uint jobId = 0;
		public List<SkillActionDescription> skills;
		
		public JobDescription()
		{
			this.skills = new List<SkillActionDescription>();
		}
		
		public JobDescription(uint arg1, List<SkillActionDescription> arg2)
			: this()
		{
			initJobDescription(arg1, arg2);
		}
		
		public virtual uint getTypeId()
		{
			return 101;
		}
		
		public JobDescription initJobDescription(uint arg1 = 0, List<SkillActionDescription> arg2 = null)
		{
			this.jobId = arg1;
			this.skills = arg2;
			return this;
		}
		
		public virtual void reset()
		{
			this.jobId = 0;
			this.skills = new List<SkillActionDescription>();
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_JobDescription(arg1);
		}
		
		public void serializeAs_JobDescription(BigEndianWriter arg1)
		{
			if ( this.jobId < 0 )
			{
				throw new Exception("Forbidden value (" + this.jobId + ") on element jobId.");
			}
			arg1.WriteByte((byte)this.jobId);
			arg1.WriteShort((short)this.skills.Count);
			var loc1 = 0;
			while ( loc1 < this.skills.Count )
			{
				arg1.WriteShort((short)this.skills[loc1].getTypeId());
				this.skills[loc1].serialize(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_JobDescription(arg1);
		}
		
		public void deserializeAs_JobDescription(BigEndianReader arg1)
		{
			var loc3 = 0;
			object loc4 = null;
			this.jobId = (uint)arg1.ReadByte();
			if ( this.jobId < 0 )
			{
				throw new Exception("Forbidden value (" + this.jobId + ") on element of JobDescription.jobId.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc3 = (ushort)arg1.ReadUShort();
				(( loc4 = ProtocolTypeManager.GetInstance<SkillActionDescription>((uint)loc3)) as SkillActionDescription).deserialize(arg1);
				this.skills.Add((SkillActionDescription)loc4);
				++loc2;
			}
		}
		
	}
}
