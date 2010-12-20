using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class StorageObjectsUpdateMessage : Message
	{
		public const uint protocolId = 6036;
		internal Boolean _isInitialized = false;
		public List<ObjectItem> objectList;
		
		public StorageObjectsUpdateMessage()
		{
			this.@objectList = new List<ObjectItem>();
		}
		
		public StorageObjectsUpdateMessage(List<ObjectItem> arg1)
			: this()
		{
			initStorageObjectsUpdateMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6036;
		}
		
		public StorageObjectsUpdateMessage initStorageObjectsUpdateMessage(List<ObjectItem> arg1)
		{
			this.@objectList = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.@objectList = new List<ObjectItem>();
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
			this.serializeAs_StorageObjectsUpdateMessage(arg1);
		}
		
		public void serializeAs_StorageObjectsUpdateMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.@objectList.Count);
			var loc1 = 0;
			while ( loc1 < this.@objectList.Count )
			{
				this.@objectList[loc1].serializeAs_ObjectItem(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_StorageObjectsUpdateMessage(arg1);
		}
		
		public void deserializeAs_StorageObjectsUpdateMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new ObjectItem()) as ObjectItem).deserialize(arg1);
				this.@objectList.Add((ObjectItem)loc3);
				++loc2;
			}
		}
		
	}
}
