using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ObjectsQuantityMessage : Message
	{
		public const uint protocolId = 6206;
		internal Boolean _isInitialized = false;
		public List<ObjectItemQuantity> objectsUIDAndQty;
		
		public ObjectsQuantityMessage()
		{
			this.@objectsUIDAndQty = new List<ObjectItemQuantity>();
		}
		
		public ObjectsQuantityMessage(List<ObjectItemQuantity> arg1)
			: this()
		{
			initObjectsQuantityMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6206;
		}
		
		public ObjectsQuantityMessage initObjectsQuantityMessage(List<ObjectItemQuantity> arg1)
		{
			this.@objectsUIDAndQty = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.@objectsUIDAndQty = new List<ObjectItemQuantity>();
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
			this.serializeAs_ObjectsQuantityMessage(arg1);
		}
		
		public void serializeAs_ObjectsQuantityMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.@objectsUIDAndQty.Count);
			var loc1 = 0;
			while ( loc1 < this.@objectsUIDAndQty.Count )
			{
				this.@objectsUIDAndQty[loc1].serializeAs_ObjectItemQuantity(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectsQuantityMessage(arg1);
		}
		
		public void deserializeAs_ObjectsQuantityMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new ObjectItemQuantity()) as ObjectItemQuantity).deserialize(arg1);
				this.@objectsUIDAndQty.Add((ObjectItemQuantity)loc3);
				++loc2;
			}
		}
		
	}
}
