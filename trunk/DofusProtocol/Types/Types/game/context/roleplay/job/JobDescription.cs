// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'JobDescription.xml' the '03/10/2011 12:47:12'
using System;
using Stump.Core.IO;
using System.Collections.Generic;
using System.Linq;

namespace Stump.DofusProtocol.Types
{
	public class JobDescription
	{
		public const uint Id = 101;
		public virtual short TypeId
		{
			get
			{
				return 101;
			}
		}
		
		public sbyte jobId;
		public IEnumerable<Types.SkillActionDescription> skills;
		
		public JobDescription()
		{
		}
		
		public JobDescription(sbyte jobId, IEnumerable<Types.SkillActionDescription> skills)
		{
			this.jobId = jobId;
			this.skills = skills;
		}
		
		public virtual void Serialize(IDataWriter writer)
		{
			writer.WriteSByte(jobId);
			writer.WriteUShort((ushort)skills.Count());
			foreach (var entry in skills)
			{
				writer.WriteShort(entry.TypeId);
				entry.Serialize(writer);
			}
		}
		
		public virtual void Deserialize(IDataReader reader)
		{
			jobId = reader.ReadSByte();
			if ( jobId < 0 )
			{
				throw new Exception("Forbidden value on jobId = " + jobId + ", it doesn't respect the following condition : jobId < 0");
			}
			int limit = reader.ReadUShort();
			skills = new Types.SkillActionDescription[limit];
			for (int i = 0; i < limit; i++)
			{
				(skills as Types.SkillActionDescription[])[i] = ProtocolTypeManager.GetInstance<Types.SkillActionDescription>(reader.ReadShort());
				(skills as Types.SkillActionDescription[])[i].Deserialize(reader);
			}
		}
	}
}
