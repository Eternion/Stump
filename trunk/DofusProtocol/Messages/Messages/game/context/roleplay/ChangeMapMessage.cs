using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ChangeMapMessage : Message
	{
		public const uint protocolId = 221;
		internal Boolean _isInitialized = false;
		public uint mapId = 0;
		
		public ChangeMapMessage()
		{
		}
		
		public ChangeMapMessage(uint arg1)
			: this()
		{
			initChangeMapMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 221;
		}
		
		public ChangeMapMessage initChangeMapMessage(uint arg1 = 0)
		{
			this.mapId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
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
			this.serializeAs_ChangeMapMessage(arg1);
		}
		
		public void serializeAs_ChangeMapMessage(BigEndianWriter arg1)
		{
			if ( this.mapId < 0 )
			{
				throw new Exception("Forbidden value (" + this.mapId + ") on element mapId.");
			}
			arg1.WriteInt((int)this.mapId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ChangeMapMessage(arg1);
		}
		
		public void deserializeAs_ChangeMapMessage(BigEndianReader arg1)
		{
			this.mapId = (uint)arg1.ReadInt();
			if ( this.mapId < 0 )
			{
				throw new Exception("Forbidden value (" + this.mapId + ") on element of ChangeMapMessage.mapId.");
			}
		}
		
	}
}
