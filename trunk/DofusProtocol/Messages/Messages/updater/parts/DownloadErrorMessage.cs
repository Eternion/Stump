using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class DownloadErrorMessage : Message
	{
		public const uint protocolId = 1513;
		internal Boolean _isInitialized = false;
		public uint errorId = 0;
		
		public DownloadErrorMessage()
		{
		}
		
		public DownloadErrorMessage(uint arg1)
			: this()
		{
			initDownloadErrorMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 1513;
		}
		
		public DownloadErrorMessage initDownloadErrorMessage(uint arg1 = 0)
		{
			this.errorId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.errorId = 0;
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
			this.serializeAs_DownloadErrorMessage(arg1);
		}
		
		public void serializeAs_DownloadErrorMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.errorId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_DownloadErrorMessage(arg1);
		}
		
		public void deserializeAs_DownloadErrorMessage(BigEndianReader arg1)
		{
			this.errorId = (uint)arg1.ReadByte();
			if ( this.errorId < 0 )
			{
				throw new Exception("Forbidden value (" + this.errorId + ") on element of DownloadErrorMessage.errorId.");
			}
		}
		
	}
}
