using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class ShortcutSpell : Shortcut
	{
		public const uint protocolId = 368;
		public uint spellId = 0;
		
		public ShortcutSpell()
		{
		}
		
		public ShortcutSpell(uint arg1, uint arg2)
			: this()
		{
			initShortcutSpell(arg1, arg2);
		}
		
		public override uint getTypeId()
		{
			return 368;
		}
		
		public ShortcutSpell initShortcutSpell(uint arg1 = 0, uint arg2 = 0)
		{
			base.initShortcut(arg1);
			this.spellId = arg2;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.spellId = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ShortcutSpell(arg1);
		}
		
		public void serializeAs_ShortcutSpell(BigEndianWriter arg1)
		{
			base.serializeAs_Shortcut(arg1);
			if ( this.spellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.spellId + ") on element spellId.");
			}
			arg1.WriteShort((short)this.spellId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ShortcutSpell(arg1);
		}
		
		public void deserializeAs_ShortcutSpell(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.spellId = (uint)arg1.ReadShort();
			if ( this.spellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.spellId + ") on element of ShortcutSpell.spellId.");
			}
		}
		
	}
}
