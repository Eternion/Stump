using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ObjectDeletedMessage : Message
	{
		public const uint protocolId = 3024;
		internal Boolean _isInitialized = false;
		public uint objectUID = 0;
		
		public ObjectDeletedMessage()
		{
		}
		
		public ObjectDeletedMessage(uint arg1)
			: this()
		{
			initObjectDeletedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 3024;
		}
		
		public ObjectDeletedMessage initObjectDeletedMessage(uint arg1 = 0)
		{
			this.@objectUID = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.@objectUID = 0;
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
			this.serializeAs_ObjectDeletedMessage(arg1);
		}
		
		public void serializeAs_ObjectDeletedMessage(BigEndianWriter arg1)
		{
			if ( this.@objectUID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectUID + ") on element objectUID.");
			}
			arg1.WriteInt((int)this.@objectUID);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectDeletedMessage(arg1);
		}
		
		public void deserializeAs_ObjectDeletedMessage(BigEndianReader arg1)
		{
			this.@objectUID = (uint)arg1.ReadInt();
			if ( this.@objectUID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectUID + ") on element of ObjectDeletedMessage.objectUID.");
			}
		}
		
	}
}
