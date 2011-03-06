using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class InteractiveElementNamedSkill : InteractiveElementSkill
	{
		public const uint protocolId = 220;
		public uint nameId = 0;
		
		public InteractiveElementNamedSkill()
		{
		}
		
		public InteractiveElementNamedSkill(uint arg1, uint arg2, uint arg3)
			: this()
		{
			initInteractiveElementNamedSkill(arg1, arg2, arg3);
		}
		
		public override uint getTypeId()
		{
			return 220;
		}
		
		public InteractiveElementNamedSkill initInteractiveElementNamedSkill(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0)
		{
			base.initInteractiveElementSkill(arg1, arg2);
			this.nameId = arg3;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.nameId = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_InteractiveElementNamedSkill(arg1);
		}
		
		public void serializeAs_InteractiveElementNamedSkill(BigEndianWriter arg1)
		{
			base.serializeAs_InteractiveElementSkill(arg1);
			if ( this.nameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.nameId + ") on element nameId.");
			}
			arg1.WriteInt((int)this.nameId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_InteractiveElementNamedSkill(arg1);
		}
		
		public void deserializeAs_InteractiveElementNamedSkill(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.nameId = (uint)arg1.ReadInt();
			if ( this.nameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.nameId + ") on element of InteractiveElementNamedSkill.nameId.");
			}
		}
		
	}
}
