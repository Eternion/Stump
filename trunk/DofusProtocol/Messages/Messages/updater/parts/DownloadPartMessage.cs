using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class DownloadPartMessage : Message
	{
		public const uint protocolId = 1503;
		internal Boolean _isInitialized = false;
		public String id = "";
		
		public DownloadPartMessage()
		{
		}
		
		public DownloadPartMessage(String arg1)
			: this()
		{
			initDownloadPartMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 1503;
		}
		
		public DownloadPartMessage initDownloadPartMessage(String arg1 = "")
		{
			this.id = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.id = "";
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
			this.serializeAs_DownloadPartMessage(arg1);
		}
		
		public void serializeAs_DownloadPartMessage(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.id);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_DownloadPartMessage(arg1);
		}
		
		public void deserializeAs_DownloadPartMessage(BigEndianReader arg1)
		{
			this.id = (String)arg1.ReadUTF();
		}
		
	}
}
