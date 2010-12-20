using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class MountSterilizedMessage : Message
	{
		public const uint protocolId = 5977;
		internal Boolean _isInitialized = false;
		public double mountId = 0;
		
		public MountSterilizedMessage()
		{
		}
		
		public MountSterilizedMessage(double arg1)
			: this()
		{
			initMountSterilizedMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5977;
		}
		
		public MountSterilizedMessage initMountSterilizedMessage(double arg1 = 0)
		{
			this.mountId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
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
			this.serializeAs_MountSterilizedMessage(arg1);
		}
		
		public void serializeAs_MountSterilizedMessage(BigEndianWriter arg1)
		{
			arg1.WriteDouble(this.mountId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_MountSterilizedMessage(arg1);
		}
		
		public void deserializeAs_MountSterilizedMessage(BigEndianReader arg1)
		{
			this.mountId = (double)arg1.ReadDouble();
		}
		
	}
}
