using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameActionFightSpellImmunityMessage : AbstractGameActionMessage
	{
		public const uint protocolId = 6221;
		internal Boolean _isInitialized = false;
		public int targetId = 0;
		public uint spellId = 0;
		
		public GameActionFightSpellImmunityMessage()
		{
		}
		
		public GameActionFightSpellImmunityMessage(uint arg1, int arg2, int arg3, uint arg4)
			: this()
		{
			initGameActionFightSpellImmunityMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 6221;
		}
		
		public GameActionFightSpellImmunityMessage initGameActionFightSpellImmunityMessage(uint arg1 = 0, int arg2 = 0, int arg3 = 0, uint arg4 = 0)
		{
			base.initAbstractGameActionMessage(arg1, arg2);
			this.targetId = arg3;
			this.spellId = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.targetId = 0;
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
			this.serializeAs_GameActionFightSpellImmunityMessage(arg1);
		}
		
		public void serializeAs_GameActionFightSpellImmunityMessage(BigEndianWriter arg1)
		{
			base.serializeAs_AbstractGameActionMessage(arg1);
			arg1.WriteInt((int)this.targetId);
			if ( this.spellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.spellId + ") on element spellId.");
			}
			arg1.WriteInt((int)this.spellId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameActionFightSpellImmunityMessage(arg1);
		}
		
		public void deserializeAs_GameActionFightSpellImmunityMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.targetId = (int)arg1.ReadInt();
			this.spellId = (uint)arg1.ReadInt();
			if ( this.spellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.spellId + ") on element of GameActionFightSpellImmunityMessage.spellId.");
			}
		}
		
	}
}
