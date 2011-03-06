using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ServerStatusUpdateMessage : Message
	{
		public const uint protocolId = 50;
		internal Boolean _isInitialized = false;
		public GameServerInformations server;
		
		public ServerStatusUpdateMessage()
		{
			this.server = new GameServerInformations();
		}
		
		public ServerStatusUpdateMessage(GameServerInformations arg1)
			: this()
		{
			initServerStatusUpdateMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 50;
		}
		
		public ServerStatusUpdateMessage initServerStatusUpdateMessage(GameServerInformations arg1 = null)
		{
			this.server = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.server = new GameServerInformations();
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
			this.serializeAs_ServerStatusUpdateMessage(arg1);
		}
		
		public void serializeAs_ServerStatusUpdateMessage(BigEndianWriter arg1)
		{
			this.server.serializeAs_GameServerInformations(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ServerStatusUpdateMessage(arg1);
		}
		
		public void deserializeAs_ServerStatusUpdateMessage(BigEndianReader arg1)
		{
			this.server = new GameServerInformations();
			this.server.deserialize(arg1);
		}
		
	}
}
