using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class SubEntity : Object
	{
		public const uint protocolId = 54;
		public uint bindingPointCategory = 0;
		public uint bindingPointIndex = 0;
		public EntityLook subEntityLook;
		
		public SubEntity()
		{
			this.subEntityLook = new EntityLook();
		}
		
		public SubEntity(uint arg1, uint arg2, EntityLook arg3)
			: this()
		{
			initSubEntity(arg1, arg2, arg3);
		}
		
		public virtual uint getTypeId()
		{
			return 54;
		}
		
		public SubEntity initSubEntity(uint arg1 = 0, uint arg2 = 0, EntityLook arg3 = null)
		{
			this.bindingPointCategory = arg1;
			this.bindingPointIndex = arg2;
			this.subEntityLook = arg3;
			return this;
		}
		
		public virtual void reset()
		{
			this.bindingPointCategory = 0;
			this.bindingPointIndex = 0;
			this.subEntityLook = new EntityLook();
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_SubEntity(arg1);
		}
		
		public void serializeAs_SubEntity(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.bindingPointCategory);
			if ( this.bindingPointIndex < 0 )
			{
				throw new Exception("Forbidden value (" + this.bindingPointIndex + ") on element bindingPointIndex.");
			}
			arg1.WriteByte((byte)this.bindingPointIndex);
			this.subEntityLook.serializeAs_EntityLook(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_SubEntity(arg1);
		}
		
		public void deserializeAs_SubEntity(BigEndianReader arg1)
		{
			this.bindingPointCategory = (uint)arg1.ReadByte();
			if ( this.bindingPointCategory < 0 )
			{
				throw new Exception("Forbidden value (" + this.bindingPointCategory + ") on element of SubEntity.bindingPointCategory.");
			}
			this.bindingPointIndex = (uint)arg1.ReadByte();
			if ( this.bindingPointIndex < 0 )
			{
				throw new Exception("Forbidden value (" + this.bindingPointIndex + ") on element of SubEntity.bindingPointIndex.");
			}
			this.subEntityLook = new EntityLook();
			this.subEntityLook.deserialize(arg1);
		}
		
	}
}
