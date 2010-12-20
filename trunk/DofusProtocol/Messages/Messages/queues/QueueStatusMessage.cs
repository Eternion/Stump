using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class QueueStatusMessage : Message
	{
		public const uint protocolId = 6100;
		internal Boolean _isInitialized = false;
		public uint position = 0;
		public uint total = 0;
		
		public QueueStatusMessage()
		{
		}
		
		public QueueStatusMessage(uint arg1, uint arg2)
			: this()
		{
			initQueueStatusMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6100;
		}
		
		public QueueStatusMessage initQueueStatusMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.position = arg1;
			this.total = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.position = 0;
			this.total = 0;
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
			this.serializeAs_QueueStatusMessage(arg1);
		}
		
		public void serializeAs_QueueStatusMessage(BigEndianWriter arg1)
		{
			if ( this.position < 0 || this.position > 65535 )
			{
				throw new Exception("Forbidden value (" + this.position + ") on element position.");
			}
			arg1.WriteShort((short)this.position);
			if ( this.total < 0 || this.total > 65535 )
			{
				throw new Exception("Forbidden value (" + this.total + ") on element total.");
			}
			arg1.WriteShort((short)this.total);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_QueueStatusMessage(arg1);
		}
		
		public void deserializeAs_QueueStatusMessage(BigEndianReader arg1)
		{
			this.position = (uint)arg1.ReadUShort();
			if ( this.position < 0 || this.position > 65535 )
			{
				throw new Exception("Forbidden value (" + this.position + ") on element of QueueStatusMessage.position.");
			}
			this.total = (uint)arg1.ReadUShort();
			if ( this.total < 0 || this.total > 65535 )
			{
				throw new Exception("Forbidden value (" + this.total + ") on element of QueueStatusMessage.total.");
			}
		}
		
	}
}
