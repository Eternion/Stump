using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class MountXpRatioMessage : Message
	{
		public const uint protocolId = 5970;
		internal Boolean _isInitialized = false;
		public uint ratio = 0;
		
		public MountXpRatioMessage()
		{
		}
		
		public MountXpRatioMessage(uint arg1)
			: this()
		{
			initMountXpRatioMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5970;
		}
		
		public MountXpRatioMessage initMountXpRatioMessage(uint arg1 = 0)
		{
			this.ratio = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.ratio = 0;
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
			this.serializeAs_MountXpRatioMessage(arg1);
		}
		
		public void serializeAs_MountXpRatioMessage(BigEndianWriter arg1)
		{
			if ( this.ratio < 0 )
			{
				throw new Exception("Forbidden value (" + this.ratio + ") on element ratio.");
			}
			arg1.WriteByte((byte)this.ratio);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_MountXpRatioMessage(arg1);
		}
		
		public void deserializeAs_MountXpRatioMessage(BigEndianReader arg1)
		{
			this.ratio = (uint)arg1.ReadByte();
			if ( this.ratio < 0 )
			{
				throw new Exception("Forbidden value (" + this.ratio + ") on element of MountXpRatioMessage.ratio.");
			}
		}
		
	}
}
