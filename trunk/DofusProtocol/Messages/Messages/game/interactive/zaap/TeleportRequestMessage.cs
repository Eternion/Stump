using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class TeleportRequestMessage : Message
	{
		public const uint protocolId = 5961;
		internal Boolean _isInitialized = false;
		public uint teleporterType = 0;
		public uint mapId = 0;
		
		public TeleportRequestMessage()
		{
		}
		
		public TeleportRequestMessage(uint arg1, uint arg2)
			: this()
		{
			initTeleportRequestMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5961;
		}
		
		public TeleportRequestMessage initTeleportRequestMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.teleporterType = arg1;
			this.mapId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.teleporterType = 0;
			this.mapId = 0;
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
			this.serializeAs_TeleportRequestMessage(arg1);
		}
		
		public void serializeAs_TeleportRequestMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.teleporterType);
			if ( this.mapId < 0 )
			{
				throw new Exception("Forbidden value (" + this.mapId + ") on element mapId.");
			}
			arg1.WriteInt((int)this.mapId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_TeleportRequestMessage(arg1);
		}
		
		public void deserializeAs_TeleportRequestMessage(BigEndianReader arg1)
		{
			this.teleporterType = (uint)arg1.ReadByte();
			if ( this.teleporterType < 0 )
			{
				throw new Exception("Forbidden value (" + this.teleporterType + ") on element of TeleportRequestMessage.teleporterType.");
			}
			this.mapId = (uint)arg1.ReadInt();
			if ( this.mapId < 0 )
			{
				throw new Exception("Forbidden value (" + this.mapId + ") on element of TeleportRequestMessage.mapId.");
			}
		}
		
	}
}
