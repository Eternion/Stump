using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class InteractiveElementSkill : Object
	{
		public const uint protocolId = 219;
		public uint skillId = 0;
		public uint skillInstanceUid = 0;
		
		public InteractiveElementSkill()
		{
		}
		
		public InteractiveElementSkill(uint arg1, uint arg2)
			: this()
		{
			initInteractiveElementSkill(arg1, arg2);
		}
		
		public virtual uint getTypeId()
		{
			return 219;
		}
		
		public InteractiveElementSkill initInteractiveElementSkill(uint arg1 = 0, uint arg2 = 0)
		{
			this.skillId = arg1;
			this.skillInstanceUid = arg2;
			return this;
		}
		
		public virtual void reset()
		{
			this.skillId = 0;
			this.skillInstanceUid = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_InteractiveElementSkill(arg1);
		}
		
		public void serializeAs_InteractiveElementSkill(BigEndianWriter arg1)
		{
			if ( this.skillId < 0 )
			{
				throw new Exception("Forbidden value (" + this.skillId + ") on element skillId.");
			}
			arg1.WriteInt((int)this.skillId);
			if ( this.skillInstanceUid < 0 )
			{
				throw new Exception("Forbidden value (" + this.skillInstanceUid + ") on element skillInstanceUid.");
			}
			arg1.WriteInt((int)this.skillInstanceUid);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_InteractiveElementSkill(arg1);
		}
		
		public void deserializeAs_InteractiveElementSkill(BigEndianReader arg1)
		{
			this.skillId = (uint)arg1.ReadInt();
			if ( this.skillId < 0 )
			{
				throw new Exception("Forbidden value (" + this.skillId + ") on element of InteractiveElementSkill.skillId.");
			}
			this.skillInstanceUid = (uint)arg1.ReadInt();
			if ( this.skillInstanceUid < 0 )
			{
				throw new Exception("Forbidden value (" + this.skillInstanceUid + ") on element of InteractiveElementSkill.skillInstanceUid.");
			}
		}
		
	}
}
