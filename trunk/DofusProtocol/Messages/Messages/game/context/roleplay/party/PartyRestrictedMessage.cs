// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'PartyRestrictedMessage.xml' the '03/10/2011 12:47:02'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class PartyRestrictedMessage : AbstractPartyMessage
	{
		public const uint Id = 6175;
		public override uint MessageId
		{
			get
			{
				return 6175;
			}
		}
		
		public bool restricted;
		
		public PartyRestrictedMessage()
		{
		}
		
		public PartyRestrictedMessage(int partyId, bool restricted)
			 : base(partyId)
		{
			this.restricted = restricted;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			writer.WriteBoolean(restricted);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			restricted = reader.ReadBoolean();
		}
	}
}
