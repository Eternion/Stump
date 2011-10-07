// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'InteractiveElementSkill.xml' the '03/10/2011 12:47:12'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
	public class InteractiveElementSkill
	{
		public const uint Id = 219;
		public virtual short TypeId
		{
			get
			{
				return 219;
			}
		}
		
		public int skillId;
		public int skillInstanceUid;
		
		public InteractiveElementSkill()
		{
		}
		
		public InteractiveElementSkill(int skillId, int skillInstanceUid)
		{
			this.skillId = skillId;
			this.skillInstanceUid = skillInstanceUid;
		}
		
		public virtual void Serialize(IDataWriter writer)
		{
			writer.WriteInt(skillId);
			writer.WriteInt(skillInstanceUid);
		}
		
		public virtual void Deserialize(IDataReader reader)
		{
			skillId = reader.ReadInt();
			if ( skillId < 0 )
			{
				throw new Exception("Forbidden value on skillId = " + skillId + ", it doesn't respect the following condition : skillId < 0");
			}
			skillInstanceUid = reader.ReadInt();
			if ( skillInstanceUid < 0 )
			{
				throw new Exception("Forbidden value on skillInstanceUid = " + skillInstanceUid + ", it doesn't respect the following condition : skillInstanceUid < 0");
			}
		}
	}
}
