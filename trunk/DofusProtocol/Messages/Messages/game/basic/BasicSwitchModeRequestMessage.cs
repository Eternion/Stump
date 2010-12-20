using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class BasicSwitchModeRequestMessage : Message
	{
		public const uint protocolId = 6101;
		internal Boolean _isInitialized = false;
		public int mode = 0;
		
		public BasicSwitchModeRequestMessage()
		{
		}
		
		public BasicSwitchModeRequestMessage(int arg1)
			: this()
		{
			initBasicSwitchModeRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6101;
		}
		
		public BasicSwitchModeRequestMessage initBasicSwitchModeRequestMessage(int arg1 = 0)
		{
			this.mode = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.mode = 0;
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
			this.serializeAs_BasicSwitchModeRequestMessage(arg1);
		}
		
		public void serializeAs_BasicSwitchModeRequestMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.mode);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_BasicSwitchModeRequestMessage(arg1);
		}
		
		public void deserializeAs_BasicSwitchModeRequestMessage(BigEndianReader arg1)
		{
			this.mode = (int)arg1.ReadByte();
		}
		
	}
}
