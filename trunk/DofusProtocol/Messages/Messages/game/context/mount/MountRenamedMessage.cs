using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class MountRenamedMessage : Message
	{
		public const uint protocolId = 5983;
		internal Boolean _isInitialized = false;
		public double mountId = 0;
		public String name = "";
		
		public MountRenamedMessage()
		{
		}
		
		public MountRenamedMessage(double arg1, String arg2)
			: this()
		{
			initMountRenamedMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5983;
		}
		
		public MountRenamedMessage initMountRenamedMessage(double arg1 = 0, String arg2 = "")
		{
			this.mountId = arg1;
			this.name = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.mountId = 0;
			this.name = "";
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
			this.serializeAs_MountRenamedMessage(arg1);
		}
		
		public void serializeAs_MountRenamedMessage(BigEndianWriter arg1)
		{
			arg1.WriteDouble(this.mountId);
			arg1.WriteUTF((string)this.name);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_MountRenamedMessage(arg1);
		}
		
		public void deserializeAs_MountRenamedMessage(BigEndianReader arg1)
		{
			this.mountId = (double)arg1.ReadDouble();
			this.name = (String)arg1.ReadUTF();
		}
		
	}
}
