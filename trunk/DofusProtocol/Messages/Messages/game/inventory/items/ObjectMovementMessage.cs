using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ObjectMovementMessage : Message
	{
		public const uint protocolId = 3010;
		internal Boolean _isInitialized = false;
		public uint objectUID = 0;
		public uint position = 63;
		
		public ObjectMovementMessage()
		{
		}
		
		public ObjectMovementMessage(uint arg1, uint arg2)
			: this()
		{
			initObjectMovementMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 3010;
		}
		
		public ObjectMovementMessage initObjectMovementMessage(uint arg1 = 0, uint arg2 = 63)
		{
			this.@objectUID = arg1;
			this.position = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.@objectUID = 0;
			this.position = 63;
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
			this.serializeAs_ObjectMovementMessage(arg1);
		}
		
		public void serializeAs_ObjectMovementMessage(BigEndianWriter arg1)
		{
			if ( this.@objectUID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectUID + ") on element objectUID.");
			}
			arg1.WriteInt((int)this.@objectUID);
			arg1.WriteByte((byte)this.position);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectMovementMessage(arg1);
		}
		
		public void deserializeAs_ObjectMovementMessage(BigEndianReader arg1)
		{
			this.@objectUID = (uint)arg1.ReadInt();
			if ( this.@objectUID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectUID + ") on element of ObjectMovementMessage.objectUID.");
			}
			this.position = (uint)arg1.ReadByte();
			if ( this.position < 0 || this.position > 255 )
			{
				throw new Exception("Forbidden value (" + this.position + ") on element of ObjectMovementMessage.position.");
			}
		}
		
	}
}
