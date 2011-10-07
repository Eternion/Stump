// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'PartyMemberRemoveMessage.xml' the '03/10/2011 12:47:01'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class PartyMemberRemoveMessage : AbstractPartyEventMessage
	{
		public const uint Id = 5579;
		public override uint MessageId
		{
			get
			{
				return 5579;
			}
		}
		
		public int leavingPlayerId;
		
		public PartyMemberRemoveMessage()
		{
		}
		
		public PartyMemberRemoveMessage(int partyId, int leavingPlayerId)
			 : base(partyId)
		{
			this.leavingPlayerId = leavingPlayerId;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			writer.WriteInt(leavingPlayerId);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			leavingPlayerId = reader.ReadInt();
			if ( leavingPlayerId < 0 )
			{
				throw new Exception("Forbidden value on leavingPlayerId = " + leavingPlayerId + ", it doesn't respect the following condition : leavingPlayerId < 0");
			}
		}
	}
}
