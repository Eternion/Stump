using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class MountSetXpRatioRequestMessage : Message
	{
		public const uint protocolId = 5989;
		internal Boolean _isInitialized = false;
		public uint xpRatio = 0;
		
		public MountSetXpRatioRequestMessage()
		{
		}
		
		public MountSetXpRatioRequestMessage(uint arg1)
			: this()
		{
			initMountSetXpRatioRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5989;
		}
		
		public MountSetXpRatioRequestMessage initMountSetXpRatioRequestMessage(uint arg1 = 0)
		{
			this.xpRatio = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.xpRatio = 0;
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
			this.serializeAs_MountSetXpRatioRequestMessage(arg1);
		}
		
		public void serializeAs_MountSetXpRatioRequestMessage(BigEndianWriter arg1)
		{
			if ( this.xpRatio < 0 )
			{
				throw new Exception("Forbidden value (" + this.xpRatio + ") on element xpRatio.");
			}
			arg1.WriteByte((byte)this.xpRatio);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_MountSetXpRatioRequestMessage(arg1);
		}
		
		public void deserializeAs_MountSetXpRatioRequestMessage(BigEndianReader arg1)
		{
			this.xpRatio = (uint)arg1.ReadByte();
			if ( this.xpRatio < 0 )
			{
				throw new Exception("Forbidden value (" + this.xpRatio + ") on element of MountSetXpRatioRequestMessage.xpRatio.");
			}
		}
		
	}
}
