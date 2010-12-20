using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class SpellUpgradeSuccessMessage : Message
	{
		public const uint protocolId = 1201;
		internal Boolean _isInitialized = false;
		public int spellId = 0;
		public int spellLevel = 0;
		
		public SpellUpgradeSuccessMessage()
		{
		}
		
		public SpellUpgradeSuccessMessage(int arg1, int arg2)
			: this()
		{
			initSpellUpgradeSuccessMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 1201;
		}
		
		public SpellUpgradeSuccessMessage initSpellUpgradeSuccessMessage(int arg1 = 0, int arg2 = 0)
		{
			this.spellId = arg1;
			this.spellLevel = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.spellId = 0;
			this.spellLevel = 0;
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
			this.serializeAs_SpellUpgradeSuccessMessage(arg1);
		}
		
		public void serializeAs_SpellUpgradeSuccessMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.spellId);
			if ( this.spellLevel < 1 || this.spellLevel > 6 )
			{
				throw new Exception("Forbidden value (" + this.spellLevel + ") on element spellLevel.");
			}
			arg1.WriteByte((byte)this.spellLevel);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_SpellUpgradeSuccessMessage(arg1);
		}
		
		public void deserializeAs_SpellUpgradeSuccessMessage(BigEndianReader arg1)
		{
			this.spellId = (int)arg1.ReadInt();
			this.spellLevel = (int)arg1.ReadByte();
			if ( this.spellLevel < 1 || this.spellLevel > 6 )
			{
				throw new Exception("Forbidden value (" + this.spellLevel + ") on element of SpellUpgradeSuccessMessage.spellLevel.");
			}
		}
		
	}
}
