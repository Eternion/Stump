using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class JobAllowMultiCraftRequestSetMessage : Message
	{
		public const uint protocolId = 5749;
		internal Boolean _isInitialized = false;
		public Boolean enabled = false;
		
		public JobAllowMultiCraftRequestSetMessage()
		{
		}
		
		public JobAllowMultiCraftRequestSetMessage(Boolean arg1)
			: this()
		{
			initJobAllowMultiCraftRequestSetMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5749;
		}
		
		public JobAllowMultiCraftRequestSetMessage initJobAllowMultiCraftRequestSetMessage(Boolean arg1 = false)
		{
			this.enabled = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.enabled = false;
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
			this.serializeAs_JobAllowMultiCraftRequestSetMessage(arg1);
		}
		
		public void serializeAs_JobAllowMultiCraftRequestSetMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.enabled);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_JobAllowMultiCraftRequestSetMessage(arg1);
		}
		
		public void deserializeAs_JobAllowMultiCraftRequestSetMessage(BigEndianReader arg1)
		{
			this.enabled = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
