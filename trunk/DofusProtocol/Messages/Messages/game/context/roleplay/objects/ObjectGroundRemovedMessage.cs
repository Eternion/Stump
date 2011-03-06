using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ObjectGroundRemovedMessage : Message
	{
		public const uint protocolId = 3014;
		internal Boolean _isInitialized = false;
		public uint cell = 0;
		
		public ObjectGroundRemovedMessage()
		{
		}
		
		public ObjectGroundRemovedMessage(uint arg1)
			: this()
		{
			initObjectGroundRemovedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 3014;
		}
		
		public ObjectGroundRemovedMessage initObjectGroundRemovedMessage(uint arg1 = 0)
		{
			this.cell = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.cell = 0;
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
			this.serializeAs_ObjectGroundRemovedMessage(arg1);
		}
		
		public void serializeAs_ObjectGroundRemovedMessage(BigEndianWriter arg1)
		{
			if ( this.cell < 0 || this.cell > 559 )
			{
				throw new Exception("Forbidden value (" + this.cell + ") on element cell.");
			}
			arg1.WriteShort((short)this.cell);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectGroundRemovedMessage(arg1);
		}
		
		public void deserializeAs_ObjectGroundRemovedMessage(BigEndianReader arg1)
		{
			this.cell = (uint)arg1.ReadShort();
			if ( this.cell < 0 || this.cell > 559 )
			{
				throw new Exception("Forbidden value (" + this.cell + ") on element of ObjectGroundRemovedMessage.cell.");
			}
		}
		
	}
}
