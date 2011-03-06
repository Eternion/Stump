using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class InteractiveUsedMessage : Message
	{
		public const uint protocolId = 5745;
		internal Boolean _isInitialized = false;
		public uint entityId = 0;
		public uint elemId = 0;
		public uint skillId = 0;
		public uint duration = 0;
		
		public InteractiveUsedMessage()
		{
		}
		
		public InteractiveUsedMessage(uint arg1, uint arg2, uint arg3, uint arg4)
			: this()
		{
			initInteractiveUsedMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 5745;
		}
		
		public InteractiveUsedMessage initInteractiveUsedMessage(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0, uint arg4 = 0)
		{
			this.entityId = arg1;
			this.elemId = arg2;
			this.skillId = arg3;
			this.duration = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.entityId = 0;
			this.elemId = 0;
			this.skillId = 0;
			this.duration = 0;
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
			this.serializeAs_InteractiveUsedMessage(arg1);
		}
		
		public void serializeAs_InteractiveUsedMessage(BigEndianWriter arg1)
		{
			if ( this.entityId < 0 )
			{
				throw new Exception("Forbidden value (" + this.entityId + ") on element entityId.");
			}
			arg1.WriteInt((int)this.entityId);
			if ( this.elemId < 0 )
			{
				throw new Exception("Forbidden value (" + this.elemId + ") on element elemId.");
			}
			arg1.WriteInt((int)this.elemId);
			if ( this.skillId < 0 )
			{
				throw new Exception("Forbidden value (" + this.skillId + ") on element skillId.");
			}
			arg1.WriteShort((short)this.skillId);
			if ( this.duration < 0 )
			{
				throw new Exception("Forbidden value (" + this.duration + ") on element duration.");
			}
			arg1.WriteShort((short)this.duration);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_InteractiveUsedMessage(arg1);
		}
		
		public void deserializeAs_InteractiveUsedMessage(BigEndianReader arg1)
		{
			this.entityId = (uint)arg1.ReadInt();
			if ( this.entityId < 0 )
			{
				throw new Exception("Forbidden value (" + this.entityId + ") on element of InteractiveUsedMessage.entityId.");
			}
			this.elemId = (uint)arg1.ReadInt();
			if ( this.elemId < 0 )
			{
				throw new Exception("Forbidden value (" + this.elemId + ") on element of InteractiveUsedMessage.elemId.");
			}
			this.skillId = (uint)arg1.ReadShort();
			if ( this.skillId < 0 )
			{
				throw new Exception("Forbidden value (" + this.skillId + ") on element of InteractiveUsedMessage.skillId.");
			}
			this.duration = (uint)arg1.ReadShort();
			if ( this.duration < 0 )
			{
				throw new Exception("Forbidden value (" + this.duration + ") on element of InteractiveUsedMessage.duration.");
			}
		}
		
	}
}
