using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameActionFightDispellSpellMessage : GameActionFightDispellMessage
	{
		public const uint protocolId = 6176;
		internal Boolean _isInitialized = false;
		public uint spellId = 0;
		
		public GameActionFightDispellSpellMessage()
		{
		}
		
		public GameActionFightDispellSpellMessage(uint arg1, int arg2, int arg3, uint arg4)
			: this()
		{
			initGameActionFightDispellSpellMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 6176;
		}
		
		public GameActionFightDispellSpellMessage initGameActionFightDispellSpellMessage(uint arg1 = 0, int arg2 = 0, int arg3 = 0, uint arg4 = 0)
		{
			base.initGameActionFightDispellMessage(arg1, arg2, arg3);
			this.spellId = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.spellId = 0;
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
			this.serializeAs_GameActionFightDispellSpellMessage(arg1);
		}
		
		public void serializeAs_GameActionFightDispellSpellMessage(BigEndianWriter arg1)
		{
			base.serializeAs_GameActionFightDispellMessage(arg1);
			if ( this.spellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.spellId + ") on element spellId.");
			}
			arg1.WriteInt((int)this.spellId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameActionFightDispellSpellMessage(arg1);
		}
		
		public void deserializeAs_GameActionFightDispellSpellMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.spellId = (uint)arg1.ReadInt();
			if ( this.spellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.spellId + ") on element of GameActionFightDispellSpellMessage.spellId.");
			}
		}
		
	}
}
