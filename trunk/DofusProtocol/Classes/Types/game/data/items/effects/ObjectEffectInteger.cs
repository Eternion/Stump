using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class ObjectEffectInteger : ObjectEffect
	{
		public const uint protocolId = 70;
		public uint value = 0;
		
		public ObjectEffectInteger()
		{
		}
		
		public ObjectEffectInteger(uint arg1, uint arg2)
			: this()
		{
			initObjectEffectInteger(arg1, arg2);
		}
		
		public override uint getTypeId()
		{
			return 70;
		}
		
		public ObjectEffectInteger initObjectEffectInteger(uint arg1 = 0, uint arg2 = 0)
		{
			base.initObjectEffect(arg1);
			this.value = arg2;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.value = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ObjectEffectInteger(arg1);
		}
		
		public void serializeAs_ObjectEffectInteger(BigEndianWriter arg1)
		{
			base.serializeAs_ObjectEffect(arg1);
			if ( this.value < 0 )
			{
				throw new Exception("Forbidden value (" + this.value + ") on element value.");
			}
			arg1.WriteShort((short)this.value);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectEffectInteger(arg1);
		}
		
		public void deserializeAs_ObjectEffectInteger(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.value = (uint)arg1.ReadShort();
			if ( this.value < 0 )
			{
				throw new Exception("Forbidden value (" + this.value + ") on element of ObjectEffectInteger.value.");
			}
		}
		
	}
}
