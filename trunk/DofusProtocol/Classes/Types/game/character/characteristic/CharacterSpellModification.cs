using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class CharacterSpellModification : Object
	{
		public const uint protocolId = 215;
		public uint modificationType = 0;
		public uint spellId = 0;
		public CharacterBaseCharacteristic value;
		
		public CharacterSpellModification()
		{
			this.value = new CharacterBaseCharacteristic();
		}
		
		public CharacterSpellModification(uint arg1, uint arg2, CharacterBaseCharacteristic arg3)
			: this()
		{
			initCharacterSpellModification(arg1, arg2, arg3);
		}
		
		public virtual uint getTypeId()
		{
			return 215;
		}
		
		public CharacterSpellModification initCharacterSpellModification(uint arg1 = 0, uint arg2 = 0, CharacterBaseCharacteristic arg3 = null)
		{
			this.modificationType = arg1;
			this.spellId = arg2;
			this.value = arg3;
			return this;
		}
		
		public virtual void reset()
		{
			this.modificationType = 0;
			this.spellId = 0;
			this.value = new CharacterBaseCharacteristic();
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_CharacterSpellModification(arg1);
		}
		
		public void serializeAs_CharacterSpellModification(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.modificationType);
			if ( this.spellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.spellId + ") on element spellId.");
			}
			arg1.WriteShort((short)this.spellId);
			this.value.serializeAs_CharacterBaseCharacteristic(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CharacterSpellModification(arg1);
		}
		
		public void deserializeAs_CharacterSpellModification(BigEndianReader arg1)
		{
			this.modificationType = (uint)arg1.ReadByte();
			if ( this.modificationType < 0 )
			{
				throw new Exception("Forbidden value (" + this.modificationType + ") on element of CharacterSpellModification.modificationType.");
			}
			this.spellId = (uint)arg1.ReadShort();
			if ( this.spellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.spellId + ") on element of CharacterSpellModification.spellId.");
			}
			this.value = new CharacterBaseCharacteristic();
			this.value.deserialize(arg1);
		}
		
	}
}
