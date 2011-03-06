using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class MonsterInGroupInformations : Object
	{
		public const uint protocolId = 144;
		public int creatureGenericId = 0;
		public uint level = 0;
		public EntityLook look;
		
		public MonsterInGroupInformations()
		{
			this.look = new EntityLook();
		}
		
		public MonsterInGroupInformations(int arg1, uint arg2, EntityLook arg3)
			: this()
		{
			initMonsterInGroupInformations(arg1, arg2, arg3);
		}
		
		public virtual uint getTypeId()
		{
			return 144;
		}
		
		public MonsterInGroupInformations initMonsterInGroupInformations(int arg1 = 0, uint arg2 = 0, EntityLook arg3 = null)
		{
			this.creatureGenericId = arg1;
			this.level = arg2;
			this.look = arg3;
			return this;
		}
		
		public virtual void reset()
		{
			this.creatureGenericId = 0;
			this.level = 0;
			this.look = new EntityLook();
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_MonsterInGroupInformations(arg1);
		}
		
		public void serializeAs_MonsterInGroupInformations(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.creatureGenericId);
			if ( this.level < 0 )
			{
				throw new Exception("Forbidden value (" + this.level + ") on element level.");
			}
			arg1.WriteShort((short)this.level);
			this.look.serializeAs_EntityLook(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_MonsterInGroupInformations(arg1);
		}
		
		public void deserializeAs_MonsterInGroupInformations(BigEndianReader arg1)
		{
			this.creatureGenericId = (int)arg1.ReadInt();
			this.level = (uint)arg1.ReadShort();
			if ( this.level < 0 )
			{
				throw new Exception("Forbidden value (" + this.level + ") on element of MonsterInGroupInformations.level.");
			}
			this.look = new EntityLook();
			this.look.deserialize(arg1);
		}
		
	}
}
