using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PartDownloadInfoMessage : PartInfoMessage
	{
		public const uint protocolId = 1508;
		internal Boolean _isInitialized = false;
		public int totalSize = 0;
		public uint downloadedSize = 0;
		public int timeElapsed = 0;
		public double bandwidth = 0;
		
		public PartDownloadInfoMessage()
		{
		}
		
		public PartDownloadInfoMessage(ContentPart arg1, int arg2, uint arg3, int arg4, double arg5)
			: this()
		{
			initPartDownloadInfoMessage(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getMessageId()
		{
			return 1508;
		}
		
		public PartDownloadInfoMessage initPartDownloadInfoMessage(ContentPart arg1 = null, int arg2 = 0, uint arg3 = 0, int arg4 = 0, double arg5 = 0)
		{
			base.initPartInfoMessage(arg1);
			this.totalSize = arg2;
			this.downloadedSize = arg3;
			this.timeElapsed = arg4;
			this.bandwidth = arg5;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.totalSize = 0;
			this.downloadedSize = 0;
			this.timeElapsed = 0;
			this.bandwidth = 0;
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
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_PartDownloadInfoMessage(arg1);
		}
		
		public void serializeAs_PartDownloadInfoMessage(BigEndianWriter arg1)
		{
			base.serializeAs_PartInfoMessage(arg1);
			arg1.WriteInt((int)this.totalSize);
			if ( this.downloadedSize < 0 )
			{
				throw new Exception("Forbidden value (" + this.downloadedSize + ") on element downloadedSize.");
			}
			arg1.WriteInt((int)this.downloadedSize);
			arg1.WriteInt((int)this.timeElapsed);
			arg1.WriteFloat((uint)this.bandwidth);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PartDownloadInfoMessage(arg1);
		}
		
		public void deserializeAs_PartDownloadInfoMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.totalSize = (int)arg1.ReadInt();
			this.downloadedSize = (uint)arg1.ReadInt();
			if ( this.downloadedSize < 0 )
			{
				throw new Exception("Forbidden value (" + this.downloadedSize + ") on element of PartDownloadInfoMessage.downloadedSize.");
			}
			this.timeElapsed = (int)arg1.ReadInt();
			this.bandwidth = (double)arg1.ReadFloat();
		}
		
	}
}
