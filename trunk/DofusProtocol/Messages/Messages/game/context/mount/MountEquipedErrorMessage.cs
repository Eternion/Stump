using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class MountEquipedErrorMessage : Message
	{
		public const uint protocolId = 5963;
		internal Boolean _isInitialized = false;
		public uint errorType = 0;
		
		public MountEquipedErrorMessage()
		{
		}
		
		public MountEquipedErrorMessage(uint arg1)
			: this()
		{
			initMountEquipedErrorMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5963;
		}
		
		public MountEquipedErrorMessage initMountEquipedErrorMessage(uint arg1 = 0)
		{
			this.errorType = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.errorType = 0;
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
			this.serializeAs_MountEquipedErrorMessage(arg1);
		}
		
		public void serializeAs_MountEquipedErrorMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.errorType);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_MountEquipedErrorMessage(arg1);
		}
		
		public void deserializeAs_MountEquipedErrorMessage(BigEndianReader arg1)
		{
			this.errorType = (uint)arg1.ReadByte();
			if ( this.errorType < 0 )
			{
				throw new Exception("Forbidden value (" + this.errorType + ") on element of MountEquipedErrorMessage.errorType.");
			}
		}
		
	}
}
