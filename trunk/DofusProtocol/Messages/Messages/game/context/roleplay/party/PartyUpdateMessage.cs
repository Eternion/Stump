using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PartyUpdateMessage : Message
	{
		public const uint protocolId = 5575;
		internal Boolean _isInitialized = false;
		public PartyMemberInformations memberInformations;
		
		public PartyUpdateMessage()
		{
			this.memberInformations = new PartyMemberInformations();
		}
		
		public PartyUpdateMessage(PartyMemberInformations arg1)
			: this()
		{
			initPartyUpdateMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5575;
		}
		
		public PartyUpdateMessage initPartyUpdateMessage(PartyMemberInformations arg1 = null)
		{
			this.memberInformations = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.memberInformations = new PartyMemberInformations();
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
			this.serializeAs_PartyUpdateMessage(arg1);
		}
		
		public void serializeAs_PartyUpdateMessage(BigEndianWriter arg1)
		{
			this.memberInformations.serializeAs_PartyMemberInformations(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PartyUpdateMessage(arg1);
		}
		
		public void deserializeAs_PartyUpdateMessage(BigEndianReader arg1)
		{
			this.memberInformations = new PartyMemberInformations();
			this.memberInformations.deserialize(arg1);
		}
		
	}
}
