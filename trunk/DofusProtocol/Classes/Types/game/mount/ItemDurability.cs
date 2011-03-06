using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class ItemDurability : Object
	{
		public const uint protocolId = 168;
		public int durability = 0;
		public int durabilityMax = 0;
		
		public ItemDurability()
		{
		}
		
		public ItemDurability(int arg1, int arg2)
			: this()
		{
			initItemDurability(arg1, arg2);
		}
		
		public virtual uint getTypeId()
		{
			return 168;
		}
		
		public ItemDurability initItemDurability(int arg1 = 0, int arg2 = 0)
		{
			this.durability = arg1;
			this.durabilityMax = arg2;
			return this;
		}
		
		public virtual void reset()
		{
			this.durability = 0;
			this.durabilityMax = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ItemDurability(arg1);
		}
		
		public void serializeAs_ItemDurability(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.durability);
			arg1.WriteShort((short)this.durabilityMax);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ItemDurability(arg1);
		}
		
		public void deserializeAs_ItemDurability(BigEndianReader arg1)
		{
			this.durability = (int)arg1.ReadShort();
			this.durabilityMax = (int)arg1.ReadShort();
		}
		
	}
}
