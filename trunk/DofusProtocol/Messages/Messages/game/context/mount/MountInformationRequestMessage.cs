using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class MountInformationRequestMessage : Message
	{
		public const uint protocolId = 5972;
		internal Boolean _isInitialized = false;
		public double id = 0;
		public double time = 0;
		
		public MountInformationRequestMessage()
		{
		}
		
		public MountInformationRequestMessage(double arg1, double arg2)
			: this()
		{
			initMountInformationRequestMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5972;
		}
		
		public MountInformationRequestMessage initMountInformationRequestMessage(double arg1 = 0, double arg2 = 0)
		{
			this.id = arg1;
			this.time = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.id = 0;
			this.time = 0;
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
			this.serializeAs_MountInformationRequestMessage(arg1);
		}
		
		public void serializeAs_MountInformationRequestMessage(BigEndianWriter arg1)
		{
			arg1.WriteDouble(this.id);
			arg1.WriteDouble(this.time);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_MountInformationRequestMessage(arg1);
		}
		
		public void deserializeAs_MountInformationRequestMessage(BigEndianReader arg1)
		{
			this.id = (double)arg1.ReadDouble();
			this.time = (double)arg1.ReadDouble();
		}
		
	}
}
