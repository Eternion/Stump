using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ObjectDeleteMessage : Message
	{
		public const uint protocolId = 3022;
		internal Boolean _isInitialized = false;
		public uint objectUID = 0;
		public uint quantity = 0;
		
		public ObjectDeleteMessage()
		{
		}
		
		public ObjectDeleteMessage(uint arg1, uint arg2)
			: this()
		{
			initObjectDeleteMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 3022;
		}
		
		public ObjectDeleteMessage initObjectDeleteMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.@objectUID = arg1;
			this.quantity = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.@objectUID = 0;
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
			this.serializeAs_ObjectDeleteMessage(arg1);
		}
		
		public void serializeAs_ObjectDeleteMessage(BigEndianWriter arg1)
		{
			if ( this.@objectUID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectUID + ") on element objectUID.");
			}
			arg1.WriteInt((int)this.@objectUID);
			if ( this.quantity < 0 )
			{
				throw new Exception("Forbidden value (" + this.quantity + ") on element quantity.");
			}
			arg1.WriteInt((int)this.quantity);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectDeleteMessage(arg1);
		}
		
		public void deserializeAs_ObjectDeleteMessage(BigEndianReader arg1)
		{
			this.@objectUID = (uint)arg1.ReadInt();
			if ( this.@objectUID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectUID + ") on element of ObjectDeleteMessage.objectUID.");
			}
			this.quantity = (uint)arg1.ReadInt();
			if ( this.quantity < 0 )
			{
				throw new Exception("Forbidden value (" + this.quantity + ") on element of ObjectDeleteMessage.quantity.");
			}
		}
		
	}
}
