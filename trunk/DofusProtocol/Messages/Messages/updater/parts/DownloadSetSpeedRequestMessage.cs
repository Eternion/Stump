using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class DownloadSetSpeedRequestMessage : Message
	{
		public const uint protocolId = 1512;
		internal Boolean _isInitialized = false;
		public uint downloadSpeed = 0;
		
		public DownloadSetSpeedRequestMessage()
		{
		}
		
		public DownloadSetSpeedRequestMessage(uint arg1)
			: this()
		{
			initDownloadSetSpeedRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 1512;
		}
		
		public DownloadSetSpeedRequestMessage initDownloadSetSpeedRequestMessage(uint arg1 = 0)
		{
			this.downloadSpeed = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.downloadSpeed = 0;
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
			this.serializeAs_DownloadSetSpeedRequestMessage(arg1);
		}
		
		public void serializeAs_DownloadSetSpeedRequestMessage(BigEndianWriter arg1)
		{
			if ( this.downloadSpeed < 1 || this.downloadSpeed > 10 )
			{
				throw new Exception("Forbidden value (" + this.downloadSpeed + ") on element downloadSpeed.");
			}
			arg1.WriteByte((byte)this.downloadSpeed);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_DownloadSetSpeedRequestMessage(arg1);
		}
		
		public void deserializeAs_DownloadSetSpeedRequestMessage(BigEndianReader arg1)
		{
			this.downloadSpeed = (uint)arg1.ReadByte();
			if ( this.downloadSpeed < 1 || this.downloadSpeed > 10 )
			{
				throw new Exception("Forbidden value (" + this.downloadSpeed + ") on element of DownloadSetSpeedRequestMessage.downloadSpeed.");
			}
		}
		
	}
}
