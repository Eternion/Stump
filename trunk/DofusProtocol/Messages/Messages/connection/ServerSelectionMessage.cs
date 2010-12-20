using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ServerSelectionMessage : Message
	{
		public const uint protocolId = 40;
		internal Boolean _isInitialized = false;
		public int serverId = 0;
		
		public ServerSelectionMessage()
		{
		}
		
		public ServerSelectionMessage(int arg1)
			: this()
		{
			initServerSelectionMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 40;
		}
		
		public ServerSelectionMessage initServerSelectionMessage(int arg1 = 0)
		{
			this.serverId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.serverId = 0;
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
			this.serializeAs_ServerSelectionMessage(arg1);
		}
		
		public void serializeAs_ServerSelectionMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.serverId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ServerSelectionMessage(arg1);
		}
		
		public void deserializeAs_ServerSelectionMessage(BigEndianReader arg1)
		{
			this.serverId = (int)arg1.ReadShort();
		}
		
	}
}
