// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GameContextKickMessage.xml' the '03/10/2011 12:46:56'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class GameContextKickMessage : Message
	{
		public const uint Id = 6081;
		public override uint MessageId
		{
			get
			{
				return 6081;
			}
		}
		
		public int targetId;
		
		public GameContextKickMessage()
		{
		}
		
		public GameContextKickMessage(int targetId)
		{
			this.targetId = targetId;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteInt(targetId);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			targetId = reader.ReadInt();
		}
	}
}
