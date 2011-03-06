using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class SpellItemBoostMessage : Message
	{
		public const uint protocolId = 6011;
		internal Boolean _isInitialized = false;
		public uint statId = 0;
		public uint spellId = 0;
		public int value = 0;
		
		public SpellItemBoostMessage()
		{
		}
		
		public SpellItemBoostMessage(uint arg1, uint arg2, int arg3)
			: this()
		{
			initSpellItemBoostMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 6011;
		}
		
		public SpellItemBoostMessage initSpellItemBoostMessage(uint arg1 = 0, uint arg2 = 0, int arg3 = 0)
		{
			this.statId = arg1;
			this.spellId = arg2;
			this.value = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.statId = 0;
			this.spellId = 0;
			this.value = 0;
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
			this.serializeAs_SpellItemBoostMessage(arg1);
		}
		
		public void serializeAs_SpellItemBoostMessage(BigEndianWriter arg1)
		{
			if ( this.statId < 0 )
			{
				throw new Exception("Forbidden value (" + this.statId + ") on element statId.");
			}
			arg1.WriteInt((int)this.statId);
			if ( this.spellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.spellId + ") on element spellId.");
			}
			arg1.WriteShort((short)this.spellId);
			arg1.WriteShort((short)this.value);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_SpellItemBoostMessage(arg1);
		}
		
		public void deserializeAs_SpellItemBoostMessage(BigEndianReader arg1)
		{
			this.statId = (uint)arg1.ReadInt();
			if ( this.statId < 0 )
			{
				throw new Exception("Forbidden value (" + this.statId + ") on element of SpellItemBoostMessage.statId.");
			}
			this.spellId = (uint)arg1.ReadShort();
			if ( this.spellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.spellId + ") on element of SpellItemBoostMessage.spellId.");
			}
			this.value = (int)arg1.ReadShort();
		}
		
	}
}
