using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class GoldItem : Item
	{
		public const uint protocolId = 123;
		public uint sum = 0;
		
		public GoldItem()
		{
		}
		
		public GoldItem(uint arg1)
			: this()
		{
			initGoldItem(arg1);
		}
		
		public override uint getTypeId()
		{
			return 123;
		}
		
		public GoldItem initGoldItem(uint arg1 = 0)
		{
			this.sum = arg1;
			return this;
		}
		
		public override void reset()
		{
			this.sum = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GoldItem(arg1);
		}
		
		public void serializeAs_GoldItem(BigEndianWriter arg1)
		{
			base.serializeAs_Item(arg1);
			if ( this.sum < 0 )
			{
				throw new Exception("Forbidden value (" + this.sum + ") on element sum.");
			}
			arg1.WriteInt((int)this.sum);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GoldItem(arg1);
		}
		
		public void deserializeAs_GoldItem(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.sum = (uint)arg1.ReadInt();
			if ( this.sum < 0 )
			{
				throw new Exception("Forbidden value (" + this.sum + ") on element of GoldItem.sum.");
			}
		}
		
	}
}
