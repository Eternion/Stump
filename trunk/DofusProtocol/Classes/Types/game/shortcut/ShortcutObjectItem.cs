using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class ShortcutObjectItem : ShortcutObject
	{
		public const uint protocolId = 371;
		public int itemUID = 0;
		
		public ShortcutObjectItem()
		{
		}
		
		public ShortcutObjectItem(uint arg1, int arg2)
			: this()
		{
			initShortcutObjectItem(arg1, arg2);
		}
		
		public override uint getTypeId()
		{
			return 371;
		}
		
		public ShortcutObjectItem initShortcutObjectItem(uint arg1 = 0, int arg2 = 0)
		{
			base.initShortcutObject(arg1);
			this.itemUID = arg2;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.itemUID = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ShortcutObjectItem(arg1);
		}
		
		public void serializeAs_ShortcutObjectItem(BigEndianWriter arg1)
		{
			base.serializeAs_ShortcutObject(arg1);
			arg1.WriteInt((int)this.itemUID);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ShortcutObjectItem(arg1);
		}
		
		public void deserializeAs_ShortcutObjectItem(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.itemUID = (int)arg1.ReadInt();
		}
		
	}
}
