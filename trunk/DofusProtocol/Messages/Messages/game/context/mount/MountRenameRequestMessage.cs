using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class MountRenameRequestMessage : Message
	{
		public const uint protocolId = 5987;
		internal Boolean _isInitialized = false;
		public String name = "";
		public double mountId = 0;
		
		public MountRenameRequestMessage()
		{
		}
		
		public MountRenameRequestMessage(String arg1, double arg2)
			: this()
		{
			initMountRenameRequestMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5987;
		}
		
		public MountRenameRequestMessage initMountRenameRequestMessage(String arg1 = "", double arg2 = 0)
		{
			this.name = arg1;
			this.mountId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.name = "";
			this.mountId = 0;
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
			this.serializeAs_MountRenameRequestMessage(arg1);
		}
		
		public void serializeAs_MountRenameRequestMessage(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.name);
			arg1.WriteDouble(this.mountId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_MountRenameRequestMessage(arg1);
		}
		
		public void deserializeAs_MountRenameRequestMessage(BigEndianReader arg1)
		{
			this.name = (String)arg1.ReadUTF();
			this.mountId = (double)arg1.ReadDouble();
		}
		
	}
}
