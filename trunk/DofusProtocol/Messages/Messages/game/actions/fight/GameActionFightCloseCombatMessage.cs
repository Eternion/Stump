using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameActionFightCloseCombatMessage : AbstractGameActionFightTargetedAbilityMessage
	{
		public const uint protocolId = 6116;
		internal Boolean _isInitialized = false;
		public uint weaponGenericId = 0;
		
		public GameActionFightCloseCombatMessage()
		{
		}
		
		public GameActionFightCloseCombatMessage(uint arg1, int arg2, int arg3, uint arg4, Boolean arg5, uint arg6)
			: this()
		{
			initGameActionFightCloseCombatMessage(arg1, arg2, arg3, arg4, arg5, arg6);
		}
		
		public override uint getMessageId()
		{
			return 6116;
		}
		
		public GameActionFightCloseCombatMessage initGameActionFightCloseCombatMessage(uint arg1 = 0, int arg2 = 0, int arg3 = 0, uint arg4 = 1, Boolean arg5 = false, uint arg6 = 0)
		{
			base.initAbstractGameActionFightTargetedAbilityMessage(arg1, arg2, arg3, arg4, arg5);
			this.weaponGenericId = arg6;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.weaponGenericId = 0;
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
			this.serializeAs_GameActionFightCloseCombatMessage(arg1);
		}
		
		public void serializeAs_GameActionFightCloseCombatMessage(BigEndianWriter arg1)
		{
			base.serializeAs_AbstractGameActionFightTargetedAbilityMessage(arg1);
			if ( this.weaponGenericId < 0 )
			{
				throw new Exception("Forbidden value (" + this.weaponGenericId + ") on element weaponGenericId.");
			}
			arg1.WriteInt((int)this.weaponGenericId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameActionFightCloseCombatMessage(arg1);
		}
		
		public void deserializeAs_GameActionFightCloseCombatMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.weaponGenericId = (uint)arg1.ReadInt();
			if ( this.weaponGenericId < 0 )
			{
				throw new Exception("Forbidden value (" + this.weaponGenericId + ") on element of GameActionFightCloseCombatMessage.weaponGenericId.");
			}
		}
		
	}
}
