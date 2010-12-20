using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class SpellUpgradeFailureMessage : Message
	{
		public const uint protocolId = 1202;
		
		public SpellUpgradeFailureMessage()
		{
		}
		
		public override uint getMessageId()
		{
			return 1202;
		}
		
		public SpellUpgradeFailureMessage initSpellUpgradeFailureMessage()
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
		
		public void serializeAs_SpellUpgradeFailureMessage(BigEndianWriter arg1)
		{
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
		}
		
		public void deserializeAs_SpellUpgradeFailureMessage(BigEndianReader arg1)
		{
		}
		
	}
}
