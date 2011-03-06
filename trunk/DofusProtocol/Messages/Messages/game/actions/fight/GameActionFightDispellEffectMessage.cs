using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameActionFightDispellEffectMessage : GameActionFightDispellMessage
	{
		public const uint protocolId = 6113;
		internal Boolean _isInitialized = false;
		public uint boostUID = 0;
		
		public GameActionFightDispellEffectMessage()
		{
		}
		
		public GameActionFightDispellEffectMessage(uint arg1, int arg2, int arg3, uint arg4)
			: this()
		{
			initGameActionFightDispellEffectMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 6113;
		}
		
		public GameActionFightDispellEffectMessage initGameActionFightDispellEffectMessage(uint arg1 = 0, int arg2 = 0, int arg3 = 0, uint arg4 = 0)
		{
			base.initGameActionFightDispellMessage(arg1, arg2, arg3);
			this.boostUID = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.boostUID = 0;
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
			this.serializeAs_GameActionFightDispellEffectMessage(arg1);
		}
		
		public void serializeAs_GameActionFightDispellEffectMessage(BigEndianWriter arg1)
		{
			base.serializeAs_GameActionFightDispellMessage(arg1);
			if ( this.boostUID < 0 )
			{
				throw new Exception("Forbidden value (" + this.boostUID + ") on element boostUID.");
			}
			arg1.WriteInt((int)this.boostUID);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameActionFightDispellEffectMessage(arg1);
		}
		
		public void deserializeAs_GameActionFightDispellEffectMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.boostUID = (uint)arg1.ReadInt();
			if ( this.boostUID < 0 )
			{
				throw new Exception("Forbidden value (" + this.boostUID + ") on element of GameActionFightDispellEffectMessage.boostUID.");
			}
		}
		
	}
}
