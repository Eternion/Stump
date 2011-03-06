using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class StorageKamasUpdateMessage : Message
	{
		public const uint protocolId = 5645;
		internal Boolean _isInitialized = false;
		public int kamasTotal = 0;
		
		public StorageKamasUpdateMessage()
		{
		}
		
		public StorageKamasUpdateMessage(int arg1)
			: this()
		{
			initStorageKamasUpdateMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5645;
		}
		
		public StorageKamasUpdateMessage initStorageKamasUpdateMessage(int arg1 = 0)
		{
			this.kamasTotal = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.kamasTotal = 0;
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
			this.serializeAs_StorageKamasUpdateMessage(arg1);
		}
		
		public void serializeAs_StorageKamasUpdateMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.kamasTotal);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_StorageKamasUpdateMessage(arg1);
		}
		
		public void deserializeAs_StorageKamasUpdateMessage(BigEndianReader arg1)
		{
			this.kamasTotal = (int)arg1.ReadInt();
		}
		
	}
}
