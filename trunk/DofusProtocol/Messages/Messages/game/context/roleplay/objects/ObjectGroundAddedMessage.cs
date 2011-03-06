using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ObjectGroundAddedMessage : Message
	{
		public const uint protocolId = 3017;
		internal Boolean _isInitialized = false;
		public uint cellId = 0;
		public uint objectGID = 0;
		
		public ObjectGroundAddedMessage()
		{
		}
		
		public ObjectGroundAddedMessage(uint arg1, uint arg2)
			: this()
		{
			initObjectGroundAddedMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 3017;
		}
		
		public ObjectGroundAddedMessage initObjectGroundAddedMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.cellId = arg1;
			this.@objectGID = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.cellId = 0;
			this.@objectGID = 0;
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
			this.serializeAs_ObjectGroundAddedMessage(arg1);
		}
		
		public void serializeAs_ObjectGroundAddedMessage(BigEndianWriter arg1)
		{
			if ( this.cellId < 0 || this.cellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.cellId + ") on element cellId.");
			}
			arg1.WriteShort((short)this.cellId);
			if ( this.@objectGID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectGID + ") on element objectGID.");
			}
			arg1.WriteShort((short)this.@objectGID);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectGroundAddedMessage(arg1);
		}
		
		public void deserializeAs_ObjectGroundAddedMessage(BigEndianReader arg1)
		{
			this.cellId = (uint)arg1.ReadShort();
			if ( this.cellId < 0 || this.cellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.cellId + ") on element of ObjectGroundAddedMessage.cellId.");
			}
			this.@objectGID = (uint)arg1.ReadShort();
			if ( this.@objectGID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectGID + ") on element of ObjectGroundAddedMessage.objectGID.");
			}
		}
		
	}
}
