using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class JobAllowMultiCraftRequestMessage : Message
	{
		public const uint protocolId = 5748;
		internal Boolean _isInitialized = false;
		public Boolean enabled = false;
		
		public JobAllowMultiCraftRequestMessage()
		{
		}
		
		public JobAllowMultiCraftRequestMessage(Boolean arg1)
			: this()
		{
			initJobAllowMultiCraftRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5748;
		}
		
		public JobAllowMultiCraftRequestMessage initJobAllowMultiCraftRequestMessage(Boolean arg1 = false)
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
			this.serializeAs_JobAllowMultiCraftRequestMessage(arg1);
		}
		
		public void serializeAs_JobAllowMultiCraftRequestMessage(BigEndianWriter arg1)
		{
			arg1.WriteBoolean(this.enabled);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_JobAllowMultiCraftRequestMessage(arg1);
		}
		
		public void deserializeAs_JobAllowMultiCraftRequestMessage(BigEndianReader arg1)
		{
			this.enabled = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
