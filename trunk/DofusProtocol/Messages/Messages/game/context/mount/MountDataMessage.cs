using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class MountDataMessage : Message
	{
		public const uint protocolId = 5973;
		internal Boolean _isInitialized = false;
		public MountClientData mountData;
		
		public MountDataMessage()
		{
			this.mountData = new MountClientData();
		}
		
		public MountDataMessage(MountClientData arg1)
			: this()
		{
			initMountDataMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5973;
		}
		
		public MountDataMessage initMountDataMessage(MountClientData arg1 = null)
		{
			this.mountData = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.mountData = new MountClientData();
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
			this.serializeAs_MountDataMessage(arg1);
		}
		
		public void serializeAs_MountDataMessage(BigEndianWriter arg1)
		{
			this.mountData.serializeAs_MountClientData(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_MountDataMessage(arg1);
		}
		
		public void deserializeAs_MountDataMessage(BigEndianReader arg1)
		{
			this.mountData = new MountClientData();
			this.mountData.deserialize(arg1);
		}
		
	}
}
