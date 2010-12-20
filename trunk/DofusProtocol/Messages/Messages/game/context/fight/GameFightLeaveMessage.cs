using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameFightLeaveMessage : Message
	{
		public const uint protocolId = 721;
		internal Boolean _isInitialized = false;
		public int charId = 0;
		
		public GameFightLeaveMessage()
		{
		}
		
		public GameFightLeaveMessage(int arg1)
			: this()
		{
			initGameFightLeaveMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 721;
		}
		
		public GameFightLeaveMessage initGameFightLeaveMessage(int arg1 = 0)
		{
			this.charId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.charId = 0;
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
			this.serializeAs_GameFightLeaveMessage(arg1);
		}
		
		public void serializeAs_GameFightLeaveMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.charId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightLeaveMessage(arg1);
		}
		
		public void deserializeAs_GameFightLeaveMessage(BigEndianReader arg1)
		{
			this.charId = (int)arg1.ReadInt();
		}
		
	}
}
