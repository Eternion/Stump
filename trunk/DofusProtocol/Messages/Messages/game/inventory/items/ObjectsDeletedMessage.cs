using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ObjectsDeletedMessage : Message
	{
		public const uint protocolId = 6034;
		internal Boolean _isInitialized = false;
		public List<uint> objectUID;
		
		public ObjectsDeletedMessage()
		{
			this.@objectUID = new List<uint>();
		}
		
		public ObjectsDeletedMessage(List<uint> arg1)
			: this()
		{
			initObjectsDeletedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6034;
		}
		
		public ObjectsDeletedMessage initObjectsDeletedMessage(List<uint> arg1)
		{
			this.@objectUID = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.@objectUID = new List<uint>();
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
			this.serializeAs_ObjectsDeletedMessage(arg1);
		}
		
		public void serializeAs_ObjectsDeletedMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.@objectUID.Count);
			var loc1 = 0;
			while ( loc1 < this.@objectUID.Count )
			{
				if ( this.@objectUID[loc1] < 0 )
				{
					throw new Exception("Forbidden value (" + this.@objectUID[loc1] + ") on element 1 (starting at 1) of objectUID.");
				}
				arg1.WriteInt((int)this.@objectUID[loc1]);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectsDeletedMessage(arg1);
		}
		
		public void deserializeAs_ObjectsDeletedMessage(BigEndianReader arg1)
		{
			var loc3 = 0;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				if ( (loc3 = arg1.ReadInt()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc3 + ") on elements of objectUID.");
				}
				this.@objectUID.Add((uint)loc3);
				++loc2;
			}
		}
		
	}
}
