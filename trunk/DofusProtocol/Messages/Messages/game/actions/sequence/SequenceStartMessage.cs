using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class SequenceStartMessage : Message
	{
		public const uint protocolId = 955;
		internal Boolean _isInitialized = false;
		public int sequenceType = 0;
		public int authorId = 0;
		
		public SequenceStartMessage()
		{
		}
		
		public SequenceStartMessage(int arg1, int arg2)
			: this()
		{
			initSequenceStartMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 955;
		}
		
		public SequenceStartMessage initSequenceStartMessage(int arg1 = 0, int arg2 = 0)
		{
			this.sequenceType = arg1;
			this.authorId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.sequenceType = 0;
			this.authorId = 0;
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
			this.serializeAs_SequenceStartMessage(arg1);
		}
		
		public void serializeAs_SequenceStartMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.sequenceType);
			arg1.WriteInt((int)this.authorId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_SequenceStartMessage(arg1);
		}
		
		public void deserializeAs_SequenceStartMessage(BigEndianReader arg1)
		{
			this.sequenceType = (int)arg1.ReadByte();
			this.authorId = (int)arg1.ReadInt();
		}
		
	}
}
