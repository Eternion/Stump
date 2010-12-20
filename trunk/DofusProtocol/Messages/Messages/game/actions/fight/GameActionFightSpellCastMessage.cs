using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameActionFightSpellCastMessage : AbstractGameActionFightTargetedAbilityMessage
	{
		public const uint protocolId = 1010;
		internal Boolean _isInitialized = false;
		public uint spellId = 0;
		public uint spellLevel = 0;
		
		public GameActionFightSpellCastMessage()
		{
		}
		
		public GameActionFightSpellCastMessage(uint arg1, int arg2, int arg3, uint arg4, Boolean arg5, uint arg6, uint arg7)
			: this()
		{
			initGameActionFightSpellCastMessage(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}
		
		public override uint getMessageId()
		{
			return 1010;
		}
		
		public GameActionFightSpellCastMessage initGameActionFightSpellCastMessage(uint arg1 = 0, int arg2 = 0, int arg3 = 0, uint arg4 = 1, Boolean arg5 = false, uint arg6 = 0, uint arg7 = 0)
		{
			base.initAbstractGameActionFightTargetedAbilityMessage(arg1, arg2, arg3, arg4, arg5);
			this.spellId = arg6;
			this.spellLevel = arg7;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
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
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameActionFightSpellCastMessage(arg1);
		}
		
		public void serializeAs_GameActionFightSpellCastMessage(BigEndianWriter arg1)
		{
			base.serializeAs_AbstractGameActionFightTargetedAbilityMessage(arg1);
			if ( this.spellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.spellId + ") on element spellId.");
			}
			arg1.WriteShort((short)this.spellId);
			if ( this.spellLevel < 1 || this.spellLevel > 6 )
			{
				throw new Exception("Forbidden value (" + this.spellLevel + ") on element spellLevel.");
			}
			arg1.WriteByte((byte)this.spellLevel);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameActionFightSpellCastMessage(arg1);
		}
		
		public void deserializeAs_GameActionFightSpellCastMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.spellId = (uint)arg1.ReadShort();
			if ( this.spellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.spellId + ") on element of GameActionFightSpellCastMessage.spellId.");
			}
			this.spellLevel = (uint)arg1.ReadByte();
			if ( this.spellLevel < 1 || this.spellLevel > 6 )
			{
				throw new Exception("Forbidden value (" + this.spellLevel + ") on element of GameActionFightSpellCastMessage.spellLevel.");
			}
		}
		
	}
}
