using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class ObjectEffectMinMax : ObjectEffect
	{
		public const uint protocolId = 82;
		public uint min = 0;
		public uint max = 0;
		
		public ObjectEffectMinMax()
		{
		}
		
		public ObjectEffectMinMax(uint arg1, uint arg2, uint arg3)
			: this()
		{
			initObjectEffectMinMax(arg1, arg2, arg3);
		}
		
		public override uint getTypeId()
		{
			return 82;
		}
		
		public ObjectEffectMinMax initObjectEffectMinMax(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0)
		{
			base.initObjectEffect(arg1);
			this.min = arg2;
			this.max = arg3;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.min = 0;
			this.max = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ObjectEffectMinMax(arg1);
		}
		
		public void serializeAs_ObjectEffectMinMax(BigEndianWriter arg1)
		{
			base.serializeAs_ObjectEffect(arg1);
			if ( this.min < 0 )
			{
				throw new Exception("Forbidden value (" + this.min + ") on element min.");
			}
			arg1.WriteShort((short)this.min);
			if ( this.max < 0 )
			{
				throw new Exception("Forbidden value (" + this.max + ") on element max.");
			}
			arg1.WriteShort((short)this.max);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectEffectMinMax(arg1);
		}
		
		public void deserializeAs_ObjectEffectMinMax(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.min = (uint)arg1.ReadShort();
			if ( this.min < 0 )
			{
				throw new Exception("Forbidden value (" + this.min + ") on element of ObjectEffectMinMax.min.");
			}
			this.max = (uint)arg1.ReadShort();
			if ( this.max < 0 )
			{
				throw new Exception("Forbidden value (" + this.max + ") on element of ObjectEffectMinMax.max.");
			}
		}
		
	}
}
