using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameActionFightInvisibleTacklerMessage : AbstractGameActionMessage
	{
		public const uint protocolId = 6233;
		internal Boolean _isInitialized = false;
		public uint extraActionPointLoss = 0;
		public uint extraMouvementPointLost = 0;
		
		public GameActionFightInvisibleTacklerMessage()
		{
		}
		
		public GameActionFightInvisibleTacklerMessage(uint arg1, int arg2, uint arg3, uint arg4)
			: this()
		{
			initGameActionFightInvisibleTacklerMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 6233;
		}
		
		public GameActionFightInvisibleTacklerMessage initGameActionFightInvisibleTacklerMessage(uint arg1 = 0, int arg2 = 0, uint arg3 = 0, uint arg4 = 0)
		{
			base.initAbstractGameActionMessage(arg1, arg2);
			this.extraActionPointLoss = arg3;
			this.extraMouvementPointLost = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.extraActionPointLoss = 0;
			this.extraMouvementPointLost = 0;
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
			this.serializeAs_GameActionFightInvisibleTacklerMessage(arg1);
		}
		
		public void serializeAs_GameActionFightInvisibleTacklerMessage(BigEndianWriter arg1)
		{
			base.serializeAs_AbstractGameActionMessage(arg1);
			if ( this.extraActionPointLoss < 0 )
			{
				throw new Exception("Forbidden value (" + this.extraActionPointLoss + ") on element extraActionPointLoss.");
			}
			arg1.WriteInt((int)this.extraActionPointLoss);
			if ( this.extraMouvementPointLost < 0 )
			{
				throw new Exception("Forbidden value (" + this.extraMouvementPointLost + ") on element extraMouvementPointLost.");
			}
			arg1.WriteInt((int)this.extraMouvementPointLost);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameActionFightInvisibleTacklerMessage(arg1);
		}
		
		public void deserializeAs_GameActionFightInvisibleTacklerMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.extraActionPointLoss = (uint)arg1.ReadInt();
			if ( this.extraActionPointLoss < 0 )
			{
				throw new Exception("Forbidden value (" + this.extraActionPointLoss + ") on element of GameActionFightInvisibleTacklerMessage.extraActionPointLoss.");
			}
			this.extraMouvementPointLost = (uint)arg1.ReadInt();
			if ( this.extraMouvementPointLost < 0 )
			{
				throw new Exception("Forbidden value (" + this.extraMouvementPointLost + ") on element of GameActionFightInvisibleTacklerMessage.extraMouvementPointLost.");
			}
		}
		
	}
}
