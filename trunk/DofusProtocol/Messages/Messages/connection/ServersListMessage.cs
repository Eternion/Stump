using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ServersListMessage : Message
	{
		public const uint protocolId = 30;
		internal Boolean _isInitialized = false;
		public List<GameServerInformations> servers;
		
		public ServersListMessage()
		{
			this.servers = new List<GameServerInformations>();
		}
		
		public ServersListMessage(List<GameServerInformations> arg1)
			: this()
		{
			initServersListMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 30;
		}
		
		public ServersListMessage initServersListMessage(List<GameServerInformations> arg1)
		{
			this.servers = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.servers = new List<GameServerInformations>();
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
			this.serializeAs_ServersListMessage(arg1);
		}
		
		public void serializeAs_ServersListMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.servers.Count);
			var loc1 = 0;
			while ( loc1 < this.servers.Count )
			{
				this.servers[loc1].serializeAs_GameServerInformations(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ServersListMessage(arg1);
		}
		
		public void deserializeAs_ServersListMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new GameServerInformations()) as GameServerInformations).deserialize(arg1);
				this.servers.Add((GameServerInformations)loc3);
				++loc2;
			}
		}
		
	}
}
