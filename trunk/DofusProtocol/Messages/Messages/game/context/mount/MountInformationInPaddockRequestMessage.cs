using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class MountInformationInPaddockRequestMessage : Message
	{
		public const uint protocolId = 5975;
		internal Boolean _isInitialized = false;
		public int mapRideId = 0;
		
		public MountInformationInPaddockRequestMessage()
		{
		}
		
		public MountInformationInPaddockRequestMessage(int arg1)
			: this()
		{
			initMountInformationInPaddockRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5975;
		}
		
		public MountInformationInPaddockRequestMessage initMountInformationInPaddockRequestMessage(int arg1 = 0)
		{
			this.mapRideId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.mapRideId = 0;
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
			this.serializeAs_MountInformationInPaddockRequestMessage(arg1);
		}
		
		public void serializeAs_MountInformationInPaddockRequestMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.mapRideId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_MountInformationInPaddockRequestMessage(arg1);
		}
		
		public void deserializeAs_MountInformationInPaddockRequestMessage(BigEndianReader arg1)
		{
			this.mapRideId = (int)arg1.ReadInt();
		}
		
	}
}
