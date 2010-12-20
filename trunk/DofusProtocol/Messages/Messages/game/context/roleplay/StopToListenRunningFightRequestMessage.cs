using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class StopToListenRunningFightRequestMessage : Message
	{
		public const uint protocolId = 6124;
		
		public StopToListenRunningFightRequestMessage()
		{
		}
		
		public override uint getMessageId()
		{
			return 6124;
		}
		
		public StopToListenRunningFightRequestMessage initStopToListenRunningFightRequestMessage()
		{
			return this;
		}
		
		public override void reset()
		{
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
		}
		
		public void serializeAs_StopToListenRunningFightRequestMessage(BigEndianWriter arg1)
		{
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
		}
		
		public void deserializeAs_StopToListenRunningFightRequestMessage(BigEndianReader arg1)
		{
		}
		
	}
}
