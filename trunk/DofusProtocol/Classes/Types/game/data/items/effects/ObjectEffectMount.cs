using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class ObjectEffectMount : ObjectEffect
	{
		public const uint protocolId = 179;
		public uint mountId = 0;
		public double date = 0;
		public uint modelId = 0;
		
		public ObjectEffectMount()
		{
		}
		
		public ObjectEffectMount(uint arg1, uint arg2, double arg3, uint arg4)
			: this()
		{
			initObjectEffectMount(arg1, arg2, arg3, arg4);
		}
		
		public override uint getTypeId()
		{
			return 179;
		}
		
		public ObjectEffectMount initObjectEffectMount(uint arg1 = 0, uint arg2 = 0, double arg3 = 0, uint arg4 = 0)
		{
			base.initObjectEffect(arg1);
			this.mountId = arg2;
			this.date = arg3;
			this.modelId = arg4;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.mountId = 0;
			this.date = 0;
			this.modelId = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ObjectEffectMount(arg1);
		}
		
		public void serializeAs_ObjectEffectMount(BigEndianWriter arg1)
		{
			base.serializeAs_ObjectEffect(arg1);
			if ( this.mountId < 0 )
			{
				throw new Exception("Forbidden value (" + this.mountId + ") on element mountId.");
			}
			arg1.WriteInt((int)this.mountId);
			arg1.WriteDouble(this.date);
			if ( this.modelId < 0 )
			{
				throw new Exception("Forbidden value (" + this.modelId + ") on element modelId.");
			}
			arg1.WriteShort((short)this.modelId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectEffectMount(arg1);
		}
		
		public void deserializeAs_ObjectEffectMount(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.mountId = (uint)arg1.ReadInt();
			if ( this.mountId < 0 )
			{
				throw new Exception("Forbidden value (" + this.mountId + ") on element of ObjectEffectMount.mountId.");
			}
			this.date = (double)arg1.ReadDouble();
			this.modelId = (uint)arg1.ReadShort();
			if ( this.modelId < 0 )
			{
				throw new Exception("Forbidden value (" + this.modelId + ") on element of ObjectEffectMount.modelId.");
			}
		}
		
	}
}
