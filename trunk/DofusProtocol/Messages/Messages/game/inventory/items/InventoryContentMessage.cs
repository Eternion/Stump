using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class InventoryContentMessage : Message
	{
		public const uint protocolId = 3016;
		internal Boolean _isInitialized = false;
		public List<ObjectItem> objects;
		public uint kamas = 0;
		
		public InventoryContentMessage()
		{
			this.@objects = new List<ObjectItem>();
		}
		
		public InventoryContentMessage(List<ObjectItem> arg1, uint arg2)
			: this()
		{
			initInventoryContentMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 3016;
		}
		
		public InventoryContentMessage initInventoryContentMessage(List<ObjectItem> arg1, uint arg2 = 0)
		{
			this.@objects = arg1;
			this.kamas = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.@objects = new List<ObjectItem>();
			this.kamas = 0;
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
			this.serializeAs_InventoryContentMessage(arg1);
		}
		
		public void serializeAs_InventoryContentMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.@objects.Count);
			var loc1 = 0;
			while ( loc1 < this.@objects.Count )
			{
				this.@objects[loc1].serializeAs_ObjectItem(arg1);
				++loc1;
			}
			if ( this.kamas < 0 )
			{
				throw new Exception("Forbidden value (" + this.kamas + ") on element kamas.");
			}
			arg1.WriteInt((int)this.kamas);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_InventoryContentMessage(arg1);
		}
		
		public void deserializeAs_InventoryContentMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new ObjectItem()) as ObjectItem).deserialize(arg1);
				this.@objects.Add((ObjectItem)loc3);
				++loc2;
			}
			this.kamas = (uint)arg1.ReadInt();
			if ( this.kamas < 0 )
			{
				throw new Exception("Forbidden value (" + this.kamas + ") on element of InventoryContentMessage.kamas.");
			}
		}
		
	}
}
