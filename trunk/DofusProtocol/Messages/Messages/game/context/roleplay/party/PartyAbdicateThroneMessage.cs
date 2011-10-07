// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'PartyAbdicateThroneMessage.xml' the '03/10/2011 12:47:01'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class PartyAbdicateThroneMessage : AbstractPartyMessage
	{
		public const uint Id = 6080;
		public override uint MessageId
		{
			get
			{
				return 6080;
			}
		}
		
		public int playerId;
		
		public PartyAbdicateThroneMessage()
		{
		}
		
		public PartyAbdicateThroneMessage(int partyId, int playerId)
			 : base(partyId)
		{
			this.playerId = playerId;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			writer.WriteInt(playerId);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			playerId = reader.ReadInt();
			if ( playerId < 0 )
			{
				throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
			}
		}
	}
}
