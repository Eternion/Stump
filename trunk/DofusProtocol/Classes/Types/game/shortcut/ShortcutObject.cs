using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class ShortcutObject : Shortcut
	{
		public const uint protocolId = 367;
		
		public ShortcutObject()
		{
		}
		
		public ShortcutObject(uint arg1)
			: this()
		{
			initShortcutObject(arg1);
		}
		
		public override uint getTypeId()
		{
			return 367;
		}
		
		public ShortcutObject initShortcutObject(uint arg1 = 0)
		{
			base.initShortcut(arg1);
			return this;
		}
		
		public override void reset()
		{
			base.reset();
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ShortcutObject(arg1);
		}
		
		public void serializeAs_ShortcutObject(BigEndianWriter arg1)
		{
			base.serializeAs_Shortcut(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ShortcutObject(arg1);
		}
		
		public void deserializeAs_ShortcutObject(BigEndianReader arg1)
		{
			base.deserialize(arg1);
		}
		
	}
}
