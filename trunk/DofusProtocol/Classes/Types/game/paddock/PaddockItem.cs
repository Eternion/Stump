using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class PaddockItem : ObjectItemInRolePlay
	{
		public const uint protocolId = 185;
		public ItemDurability durability;
		
		public PaddockItem()
		{
			this.durability = new ItemDurability();
		}
		
		public PaddockItem(uint arg1, uint arg2, ItemDurability arg3)
			: this()
		{
			initPaddockItem(arg1, arg2, arg3);
		}
		
		public override uint getTypeId()
		{
			return 185;
		}
		
		public PaddockItem initPaddockItem(uint arg1 = 0, uint arg2 = 0, ItemDurability arg3 = null)
		{
			base.initObjectItemInRolePlay(arg1, arg2);
			this.durability = arg3;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.durability = new ItemDurability();
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_PaddockItem(arg1);
		}
		
		public void serializeAs_PaddockItem(BigEndianWriter arg1)
		{
			base.serializeAs_ObjectItemInRolePlay(arg1);
			this.durability.serializeAs_ItemDurability(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PaddockItem(arg1);
		}
		
		public void deserializeAs_PaddockItem(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.durability = new ItemDurability();
			this.durability.deserialize(arg1);
		}
		
	}
}
