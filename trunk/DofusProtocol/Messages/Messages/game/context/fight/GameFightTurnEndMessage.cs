using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameFightTurnEndMessage : Message
	{
		public const uint protocolId = 719;
		internal Boolean _isInitialized = false;
		public int id = 0;
		
		public GameFightTurnEndMessage()
		{
		}
		
		public GameFightTurnEndMessage(int arg1)
			: this()
		{
			initGameFightTurnEndMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 719;
		}
		
		public GameFightTurnEndMessage initGameFightTurnEndMessage(int arg1 = 0)
		{
			this.id = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.id = 0;
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
			this.serializeAs_GameFightTurnEndMessage(arg1);
		}
		
		public void serializeAs_GameFightTurnEndMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.id);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightTurnEndMessage(arg1);
		}
		
		public void deserializeAs_GameFightTurnEndMessage(BigEndianReader arg1)
		{
			this.id = (int)arg1.ReadInt();
		}
		
	}
}
