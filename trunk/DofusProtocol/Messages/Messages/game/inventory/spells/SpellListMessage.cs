using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class SpellListMessage : Message
	{
		public const uint protocolId = 1200;
		internal Boolean _isInitialized = false;
		public Boolean spellPrevisualization = false;
		public List<SpellItem> spells;
		
		public SpellListMessage()
		{
			this.spells = new List<SpellItem>();
		}
		
		public SpellListMessage(Boolean arg1, List<SpellItem> arg2)
			: this()
		{
			initSpellListMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 1200;
		}
		
		public SpellListMessage initSpellListMessage(Boolean arg1 = false, List<SpellItem> arg2 = null)
		{
			this.spellPrevisualization = arg1;
			this.spells = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.spellPrevisualization = false;
			this.spells = new List<SpellItem>();
			this._isInitialized = false;
		}
		
		public override void pack(BigEndianWriter arg1)
		{
			this.serialize(arg1);
			WritePacket(arg1, this.getMessageId());
		}
		
		public override void unpack(BigEndianReader arg1, uint arg2)
		{
			this.deserialize(arg1);
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_SpellListMessage(arg1);
		}
		
		public void serializeAs_SpellListMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.spellPrevisualization);
			arg1.WriteShort((short)this.spells.Count);
			var loc1 = 0;
			while ( loc1 < this.spells.Count )
			{
				this.spells[loc1].serializeAs_SpellItem(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_SpellListMessage(arg1);
		}
		
		public void deserializeAs_SpellListMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			this.spellPrevisualization = (Boolean)arg1.ReadBoolean();
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new SpellItem()) as SpellItem).deserialize(arg1);
				this.spells.Add((SpellItem)loc3);
				++loc2;
			}
		}
		
	}
}
