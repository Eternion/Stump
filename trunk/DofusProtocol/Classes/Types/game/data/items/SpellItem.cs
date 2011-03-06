using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class SpellItem : Item
	{
		public const uint protocolId = 49;
		public uint position = 0;
		public int spellId = 0;
		public int spellLevel = 0;
		
		public SpellItem()
		{
		}
		
		public SpellItem(uint arg1, int arg2, int arg3)
			: this()
		{
			initSpellItem(arg1, arg2, arg3);
		}
		
		public override uint getTypeId()
		{
			return 49;
		}
		
		public SpellItem initSpellItem(uint arg1 = 0, int arg2 = 0, int arg3 = 0)
		{
			this.position = arg1;
			this.spellId = arg2;
			this.spellLevel = arg3;
			return this;
		}
		
		public override void reset()
		{
			this.position = 0;
			this.spellId = 0;
			this.spellLevel = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_SpellItem(arg1);
		}
		
		public void serializeAs_SpellItem(BigEndianWriter arg1)
		{
			base.serializeAs_Item(arg1);
			if ( this.position < 63 || this.position > 255 )
			{
				throw new Exception("Forbidden value (" + this.position + ") on element position.");
			}
			arg1.WriteByte((byte)this.position);
			arg1.WriteInt((int)this.spellId);
			if ( this.spellLevel < 1 || this.spellLevel > 6 )
			{
				throw new Exception("Forbidden value (" + this.spellLevel + ") on element spellLevel.");
			}
			arg1.WriteByte((byte)this.spellLevel);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_SpellItem(arg1);
		}
		
		public void deserializeAs_SpellItem(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.position = (uint)arg1.ReadByte();
			if ( this.position < 63 || this.position > 255 )
			{
				throw new Exception("Forbidden value (" + this.position + ") on element of SpellItem.position.");
			}
			this.spellId = (int)arg1.ReadInt();
			this.spellLevel = (int)arg1.ReadByte();
			if ( this.spellLevel < 1 || this.spellLevel > 6 )
			{
				throw new Exception("Forbidden value (" + this.spellLevel + ") on element of SpellItem.spellLevel.");
			}
		}
		
	}
}
