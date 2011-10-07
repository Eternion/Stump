// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GoldAddedMessage.xml' the '03/10/2011 12:47:07'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class GoldAddedMessage : Message
	{
		public const uint Id = 6030;
		public override uint MessageId
		{
			get
			{
				return 6030;
			}
		}
		
		public Types.GoldItem gold;
		
		public GoldAddedMessage()
		{
		}
		
		public GoldAddedMessage(Types.GoldItem gold)
		{
			this.gold = gold;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			gold.Serialize(writer);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			gold = new Types.GoldItem();
			gold.Deserialize(reader);
		}
	}
}
