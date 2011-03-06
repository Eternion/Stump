using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ObjectSetPositionMessage : Message
	{
		public const uint protocolId = 3021;
		internal Boolean _isInitialized = false;
		public uint objectUID = 0;
		public uint position = 63;
		public uint quantity = 0;
		
		public ObjectSetPositionMessage()
		{
		}
		
		public ObjectSetPositionMessage(uint arg1, uint arg2, uint arg3)
			: this()
		{
			initObjectSetPositionMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 3021;
		}
		
		public ObjectSetPositionMessage initObjectSetPositionMessage(uint arg1 = 0, uint arg2 = 63, uint arg3 = 0)
		{
			this.@objectUID = arg1;
			this.position = arg2;
			this.quantity = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.@objectUID = 0;
			this.position = 63;
			this.quantity = 0;
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
			this.serializeAs_ObjectSetPositionMessage(arg1);
		}
		
		public void serializeAs_ObjectSetPositionMessage(BigEndianWriter arg1)
		{
			if ( this.@objectUID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectUID + ") on element objectUID.");
			}
			arg1.WriteInt((int)this.@objectUID);
			arg1.WriteByte((byte)this.position);
			if ( this.quantity < 0 )
			{
				throw new Exception("Forbidden value (" + this.quantity + ") on element quantity.");
			}
			arg1.WriteInt((int)this.quantity);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectSetPositionMessage(arg1);
		}
		
		public void deserializeAs_ObjectSetPositionMessage(BigEndianReader arg1)
		{
			this.@objectUID = (uint)arg1.ReadInt();
			if ( this.@objectUID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectUID + ") on element of ObjectSetPositionMessage.objectUID.");
			}
			this.position = (uint)arg1.ReadByte();
			if ( this.position < 0 || this.position > 255 )
			{
				throw new Exception("Forbidden value (" + this.position + ") on element of ObjectSetPositionMessage.position.");
			}
			this.quantity = (uint)arg1.ReadInt();
			if ( this.quantity < 0 )
			{
				throw new Exception("Forbidden value (" + this.quantity + ") on element of ObjectSetPositionMessage.quantity.");
			}
		}
		
	}
}
