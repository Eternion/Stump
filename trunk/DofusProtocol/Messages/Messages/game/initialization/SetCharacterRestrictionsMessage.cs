using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class SetCharacterRestrictionsMessage : Message
	{
		public const uint protocolId = 170;
		internal Boolean _isInitialized = false;
		public ActorRestrictionsInformations restrictions;
		
		public SetCharacterRestrictionsMessage()
		{
			this.restrictions = new ActorRestrictionsInformations();
		}
		
		public SetCharacterRestrictionsMessage(ActorRestrictionsInformations arg1)
			: this()
		{
			initSetCharacterRestrictionsMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 170;
		}
		
		public SetCharacterRestrictionsMessage initSetCharacterRestrictionsMessage(ActorRestrictionsInformations arg1 = null)
		{
			this.restrictions = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.restrictions = new ActorRestrictionsInformations();
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
			this.serializeAs_SetCharacterRestrictionsMessage(arg1);
		}
		
		public void serializeAs_SetCharacterRestrictionsMessage(BigEndianWriter arg1)
		{
			this.restrictions.serializeAs_ActorRestrictionsInformations(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_SetCharacterRestrictionsMessage(arg1);
		}
		
		public void deserializeAs_SetCharacterRestrictionsMessage(BigEndianReader arg1)
		{
			this.restrictions = new ActorRestrictionsInformations();
			this.restrictions.deserialize(arg1);
		}
		
	}
}
