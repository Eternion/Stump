using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameContextKickMessage : Message
	{
		public const uint protocolId = 6081;
		internal Boolean _isInitialized = false;
		public int targetId = 0;
		
		public GameContextKickMessage()
		{
		}
		
		public GameContextKickMessage(int arg1)
			: this()
		{
			initGameContextKickMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6081;
		}
		
		public GameContextKickMessage initGameContextKickMessage(int arg1 = 0)
		{
			this.targetId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.targetId = 0;
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
			this.serializeAs_GameContextKickMessage(arg1);
		}
		
		public void serializeAs_GameContextKickMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.targetId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameContextKickMessage(arg1);
		}
		
		public void deserializeAs_GameContextKickMessage(BigEndianReader arg1)
		{
			this.targetId = (int)arg1.ReadInt();
		}
		
	}
}
