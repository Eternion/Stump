using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class DocumentReadingBeginMessage : Message
	{
		public const uint protocolId = 5675;
		internal Boolean _isInitialized = false;
		public uint documentId = 0;
		
		public DocumentReadingBeginMessage()
		{
		}
		
		public DocumentReadingBeginMessage(uint arg1)
			: this()
		{
			initDocumentReadingBeginMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5675;
		}
		
		public DocumentReadingBeginMessage initDocumentReadingBeginMessage(uint arg1 = 0)
		{
			this.documentId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.documentId = 0;
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
			this.serializeAs_DocumentReadingBeginMessage(arg1);
		}
		
		public void serializeAs_DocumentReadingBeginMessage(BigEndianWriter arg1)
		{
			if ( this.documentId < 0 )
			{
				throw new Exception("Forbidden value (" + this.documentId + ") on element documentId.");
			}
			arg1.WriteShort((short)this.documentId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_DocumentReadingBeginMessage(arg1);
		}
		
		public void deserializeAs_DocumentReadingBeginMessage(BigEndianReader arg1)
		{
			this.documentId = (uint)arg1.ReadShort();
			if ( this.documentId < 0 )
			{
				throw new Exception("Forbidden value (" + this.documentId + ") on element of DocumentReadingBeginMessage.documentId.");
			}
		}
		
	}
}
