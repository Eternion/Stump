// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'FriendAddedMessage.xml' the '03/10/2011 12:47:02'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class FriendAddedMessage : Message
	{
		public const uint Id = 5599;
		public override uint MessageId
		{
			get
			{
				return 5599;
			}
		}
		
		public Types.FriendInformations friendAdded;
		
		public FriendAddedMessage()
		{
		}
		
		public FriendAddedMessage(Types.FriendInformations friendAdded)
		{
			this.friendAdded = friendAdded;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteShort(friendAdded.TypeId);
			friendAdded.Serialize(writer);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			friendAdded = Types.ProtocolTypeManager.GetInstance<Types.FriendInformations>(reader.ReadShort());
			friendAdded.Deserialize(reader);
		}
	}
}
