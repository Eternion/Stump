using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class AcquaintanceServerListMessage : Message
	{
		public const uint protocolId = 6142;
		internal Boolean _isInitialized = false;
		public List<int> servers;
		
		public AcquaintanceServerListMessage()
		{
			this.servers = new List<int>();
		}
		
		public AcquaintanceServerListMessage(List<int> arg1)
			: this()
		{
			initAcquaintanceServerListMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6142;
		}
		
		public AcquaintanceServerListMessage initAcquaintanceServerListMessage(List<int> arg1)
		{
			this.servers = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.servers = new List<int>();
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
			this.serializeAs_AcquaintanceServerListMessage(arg1);
		}
		
		public void serializeAs_AcquaintanceServerListMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.servers.Count);
			var loc1 = 0;
			while ( loc1 < this.servers.Count )
			{
				arg1.WriteShort((short)this.servers[loc1]);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AcquaintanceServerListMessage(arg1);
		}
		
		public void deserializeAs_AcquaintanceServerListMessage(BigEndianReader arg1)
		{
			var loc3 = 0;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc3 = arg1.ReadShort();
				this.servers.Add((int)loc3);
				++loc2;
			}
		}
		
	}
}
