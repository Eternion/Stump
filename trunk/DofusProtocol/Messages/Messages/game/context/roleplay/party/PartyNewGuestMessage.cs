// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'PartyNewGuestMessage.xml' the '03/10/2011 12:47:02'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class PartyNewGuestMessage : AbstractPartyEventMessage
	{
		public const uint Id = 6260;
		public override uint MessageId
		{
			get
			{
				return 6260;
			}
		}
		
		public Types.PartyGuestInformations guest;
		
		public PartyNewGuestMessage()
		{
		}
		
		public PartyNewGuestMessage(int partyId, Types.PartyGuestInformations guest)
			 : base(partyId)
		{
			this.guest = guest;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			guest.Serialize(writer);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			guest = new Types.PartyGuestInformations();
			guest.Deserialize(reader);
		}
	}
}
