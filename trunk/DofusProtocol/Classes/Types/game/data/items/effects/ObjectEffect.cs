using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class ObjectEffect : Object
	{
		public const uint protocolId = 76;
		public uint actionId = 0;
		
		public ObjectEffect()
		{
		}
		
		public ObjectEffect(uint arg1)
			: this()
		{
			initObjectEffect(arg1);
		}
		
		public virtual uint getTypeId()
		{
			return 76;
		}
		
		public ObjectEffect initObjectEffect(uint arg1 = 0)
		{
			this.actionId = arg1;
			return this;
		}
		
		public virtual void reset()
		{
			this.actionId = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ObjectEffect(arg1);
		}
		
		public void serializeAs_ObjectEffect(BigEndianWriter arg1)
		{
			if ( this.actionId < 0 )
			{
				throw new Exception("Forbidden value (" + this.actionId + ") on element actionId.");
			}
			arg1.WriteShort((short)this.actionId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectEffect(arg1);
		}
		
		public void deserializeAs_ObjectEffect(BigEndianReader arg1)
		{
			this.actionId = (uint)arg1.ReadShort();
			if ( this.actionId < 0 )
			{
				throw new Exception("Forbidden value (" + this.actionId + ") on element of ObjectEffect.actionId.");
			}
		}
		
	}
}
