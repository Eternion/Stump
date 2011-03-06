using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class InteractiveUseEndedMessage : Message
	{
		public const uint protocolId = 6112;
		internal Boolean _isInitialized = false;
		public uint elemId = 0;
		public uint skillId = 0;
		
		public InteractiveUseEndedMessage()
		{
		}
		
		public InteractiveUseEndedMessage(uint arg1, uint arg2)
			: this()
		{
			initInteractiveUseEndedMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6112;
		}
		
		public InteractiveUseEndedMessage initInteractiveUseEndedMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.elemId = arg1;
			this.skillId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.elemId = 0;
			this.skillId = 0;
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
			this.serializeAs_InteractiveUseEndedMessage(arg1);
		}
		
		public void serializeAs_InteractiveUseEndedMessage(BigEndianWriter arg1)
		{
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
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_InteractiveUseEndedMessage(arg1);
		}
		
		public void deserializeAs_InteractiveUseEndedMessage(BigEndianReader arg1)
		{
			this.elemId = (uint)arg1.ReadInt();
			if ( this.elemId < 0 )
			{
				throw new Exception("Forbidden value (" + this.elemId + ") on element of InteractiveUseEndedMessage.elemId.");
			}
			this.skillId = (uint)arg1.ReadShort();
			if ( this.skillId < 0 )
			{
				throw new Exception("Forbidden value (" + this.skillId + ") on element of InteractiveUseEndedMessage.skillId.");
			}
		}
		
	}
}
