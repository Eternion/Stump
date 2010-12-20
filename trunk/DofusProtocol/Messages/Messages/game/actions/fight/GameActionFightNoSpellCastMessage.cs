using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameActionFightNoSpellCastMessage : Message
	{
		public const uint protocolId = 6132;
		internal Boolean _isInitialized = false;
		public uint spellLevelId = 0;
		
		public GameActionFightNoSpellCastMessage()
		{
		}
		
		public GameActionFightNoSpellCastMessage(uint arg1)
			: this()
		{
			initGameActionFightNoSpellCastMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6132;
		}
		
		public GameActionFightNoSpellCastMessage initGameActionFightNoSpellCastMessage(uint arg1 = 0)
		{
			this.spellLevelId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.spellLevelId = 0;
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
			this.serializeAs_GameActionFightNoSpellCastMessage(arg1);
		}
		
		public void serializeAs_GameActionFightNoSpellCastMessage(BigEndianWriter arg1)
		{
			if ( this.spellLevelId < 0 )
			{
				throw new Exception("Forbidden value (" + this.spellLevelId + ") on element spellLevelId.");
			}
			arg1.WriteInt((int)this.spellLevelId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameActionFightNoSpellCastMessage(arg1);
		}
		
		public void deserializeAs_GameActionFightNoSpellCastMessage(BigEndianReader arg1)
		{
			this.spellLevelId = (uint)arg1.ReadInt();
			if ( this.spellLevelId < 0 )
			{
				throw new Exception("Forbidden value (" + this.spellLevelId + ") on element of GameActionFightNoSpellCastMessage.spellLevelId.");
			}
		}
		
	}
}
