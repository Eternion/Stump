using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ObjectsAddedMessage : Message
	{
		public const uint protocolId = 6033;
		internal Boolean _isInitialized = false;
		public List<ObjectItem> @object;
		
		public ObjectsAddedMessage()
		{
			this.@object = new List<ObjectItem>();
		}
		
		public ObjectsAddedMessage(List<ObjectItem> arg1)
			: this()
		{
			initObjectsAddedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6033;
		}
		
		public ObjectsAddedMessage initObjectsAddedMessage(List<ObjectItem> arg1)
		{
			this.@object = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.@object = new List<ObjectItem>();
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
			this.serializeAs_ObjectsAddedMessage(arg1);
		}
		
		public void serializeAs_ObjectsAddedMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.@object.Count);
			var loc1 = 0;
			while ( loc1 < this.@object.Count )
			{
				this.@object[loc1].serializeAs_ObjectItem(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectsAddedMessage(arg1);
		}
		
		public void deserializeAs_ObjectsAddedMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new ObjectItem()) as ObjectItem).deserialize(arg1);
				this.@object.Add((ObjectItem)loc3);
				++loc2;
			}
		}
		
	}
}
