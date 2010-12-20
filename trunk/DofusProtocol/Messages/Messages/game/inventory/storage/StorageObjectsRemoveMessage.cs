using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class StorageObjectsRemoveMessage : Message
	{
		public const uint protocolId = 6035;
		internal Boolean _isInitialized = false;
		public List<uint> objectUIDList;
		
		public StorageObjectsRemoveMessage()
		{
			this.@objectUIDList = new List<uint>();
		}
		
		public StorageObjectsRemoveMessage(List<uint> arg1)
			: this()
		{
			initStorageObjectsRemoveMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6035;
		}
		
		public StorageObjectsRemoveMessage initStorageObjectsRemoveMessage(List<uint> arg1)
		{
			this.@objectUIDList = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.@objectUIDList = new List<uint>();
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
			this.serializeAs_StorageObjectsRemoveMessage(arg1);
		}
		
		public void serializeAs_StorageObjectsRemoveMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.@objectUIDList.Count);
			var loc1 = 0;
			while ( loc1 < this.@objectUIDList.Count )
			{
				if ( this.@objectUIDList[loc1] < 0 )
				{
					throw new Exception("Forbidden value (" + this.@objectUIDList[loc1] + ") on element 1 (starting at 1) of objectUIDList.");
				}
				arg1.WriteInt((int)this.@objectUIDList[loc1]);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_StorageObjectsRemoveMessage(arg1);
		}
		
		public void deserializeAs_StorageObjectsRemoveMessage(BigEndianReader arg1)
		{
			var loc3 = 0;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				if ( (loc3 = arg1.ReadInt()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc3 + ") on elements of objectUIDList.");
				}
				this.@objectUIDList.Add((uint)loc3);
				++loc2;
			}
		}
		
	}
}
