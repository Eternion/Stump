using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class InteractiveUseRequestMessage : Message
	{
		public const uint protocolId = 5001;
		internal Boolean _isInitialized = false;
		public uint elemId = 0;
		public uint skillInstanceUid = 0;
		
		public InteractiveUseRequestMessage()
		{
		}
		
		public InteractiveUseRequestMessage(uint arg1, uint arg2)
			: this()
		{
			initInteractiveUseRequestMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5001;
		}
		
		public InteractiveUseRequestMessage initInteractiveUseRequestMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.elemId = arg1;
			this.skillInstanceUid = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.elemId = 0;
			this.skillInstanceUid = 0;
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
			this.serializeAs_InteractiveUseRequestMessage(arg1);
		}
		
		public void serializeAs_InteractiveUseRequestMessage(BigEndianWriter arg1)
		{
			if ( this.elemId < 0 )
			{
				throw new Exception("Forbidden value (" + this.elemId + ") on element elemId.");
			}
			arg1.WriteInt((int)this.elemId);
			if ( this.skillInstanceUid < 0 )
			{
				throw new Exception("Forbidden value (" + this.skillInstanceUid + ") on element skillInstanceUid.");
			}
			arg1.WriteInt((int)this.skillInstanceUid);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_InteractiveUseRequestMessage(arg1);
		}
		
		public void deserializeAs_InteractiveUseRequestMessage(BigEndianReader arg1)
		{
			this.elemId = (uint)arg1.ReadInt();
			if ( this.elemId < 0 )
			{
				throw new Exception("Forbidden value (" + this.elemId + ") on element of InteractiveUseRequestMessage.elemId.");
			}
			this.skillInstanceUid = (uint)arg1.ReadInt();
			if ( this.skillInstanceUid < 0 )
			{
				throw new Exception("Forbidden value (" + this.skillInstanceUid + ") on element of InteractiveUseRequestMessage.skillInstanceUid.");
			}
		}
		
	}
}
