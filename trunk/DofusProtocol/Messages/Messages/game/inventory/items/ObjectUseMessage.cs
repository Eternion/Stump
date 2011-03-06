using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ObjectUseMessage : Message
	{
		public const uint protocolId = 3019;
		internal Boolean _isInitialized = false;
		public uint objectUID = 0;
		
		public ObjectUseMessage()
		{
		}
		
		public ObjectUseMessage(uint arg1)
			: this()
		{
			initObjectUseMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 3019;
		}
		
		public ObjectUseMessage initObjectUseMessage(uint arg1 = 0)
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
			this.serializeAs_ObjectUseMessage(arg1);
		}
		
		public void serializeAs_ObjectUseMessage(BigEndianWriter arg1)
		{
			if ( this.@objectUID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectUID + ") on element objectUID.");
			}
			arg1.WriteInt((int)this.@objectUID);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectUseMessage(arg1);
		}
		
		public void deserializeAs_ObjectUseMessage(BigEndianReader arg1)
		{
			this.@objectUID = (uint)arg1.ReadInt();
			if ( this.@objectUID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectUID + ") on element of ObjectUseMessage.objectUID.");
			}
		}
		
	}
}
