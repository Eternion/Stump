using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class StorageInventoryContentMessage : InventoryContentMessage
	{
		public const uint protocolId = 5646;
		internal Boolean _isInitialized = false;
		
		public StorageInventoryContentMessage()
		{
		}
		
		public StorageInventoryContentMessage(List<ObjectItem> arg1, uint arg2)
			: this()
		{
			initStorageInventoryContentMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5646;
		}
		
		public StorageInventoryContentMessage initStorageInventoryContentMessage(List<ObjectItem> arg1, uint arg2 = 0)
		{
			base.initInventoryContentMessage(arg1, arg2);
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
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
			this.serializeAs_StorageInventoryContentMessage(arg1);
		}
		
		public void serializeAs_StorageInventoryContentMessage(BigEndianWriter arg1)
		{
			base.serializeAs_InventoryContentMessage(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_StorageInventoryContentMessage(arg1);
		}
		
		public void deserializeAs_StorageInventoryContentMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
		}
		
	}
}
