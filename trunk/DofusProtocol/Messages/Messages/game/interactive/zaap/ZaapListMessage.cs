using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ZaapListMessage : TeleportDestinationsListMessage
	{
		public const uint protocolId = 1604;
		internal Boolean _isInitialized = false;
		public uint spawnMapId = 0;
		
		public ZaapListMessage()
		{
		}
		
		public ZaapListMessage(uint arg1, List<uint> arg2, List<uint> arg3, List<uint> arg4, uint arg5)
			: this()
		{
			initZaapListMessage(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getMessageId()
		{
			return 1604;
		}
		
		public ZaapListMessage initZaapListMessage(uint arg1 = 0, List<uint> arg2 = null, List<uint> arg3 = null, List<uint> arg4 = null, uint arg5 = 0)
		{
			base.initTeleportDestinationsListMessage(arg1, arg2, arg3, arg4);
			this.spawnMapId = arg5;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.spawnMapId = 0;
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
			this.serializeAs_ZaapListMessage(arg1);
		}
		
		public void serializeAs_ZaapListMessage(BigEndianWriter arg1)
		{
			base.serializeAs_TeleportDestinationsListMessage(arg1);
			if ( this.spawnMapId < 0 )
			{
				throw new Exception("Forbidden value (" + this.spawnMapId + ") on element spawnMapId.");
			}
			arg1.WriteInt((int)this.spawnMapId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ZaapListMessage(arg1);
		}
		
		public void deserializeAs_ZaapListMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.spawnMapId = (uint)arg1.ReadInt();
			if ( this.spawnMapId < 0 )
			{
				throw new Exception("Forbidden value (" + this.spawnMapId + ") on element of ZaapListMessage.spawnMapId.");
			}
		}
		
	}
}
