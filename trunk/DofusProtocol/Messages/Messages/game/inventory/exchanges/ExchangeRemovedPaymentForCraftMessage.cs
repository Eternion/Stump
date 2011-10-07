// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ExchangeRemovedPaymentForCraftMessage.xml' the '03/10/2011 12:47:06'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ExchangeRemovedPaymentForCraftMessage : Message
	{
		public const uint Id = 6031;
		public override uint MessageId
		{
			get
			{
				return 6031;
			}
		}
		
		public bool onlySuccess;
		public int objectUID;
		
		public ExchangeRemovedPaymentForCraftMessage()
		{
		}
		
		public ExchangeRemovedPaymentForCraftMessage(bool onlySuccess, int objectUID)
		{
			this.onlySuccess = onlySuccess;
			this.objectUID = objectUID;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteBoolean(onlySuccess);
			writer.WriteInt(objectUID);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			onlySuccess = reader.ReadBoolean();
			objectUID = reader.ReadInt();
			if ( objectUID < 0 )
			{
				throw new Exception("Forbidden value on objectUID = " + objectUID + ", it doesn't respect the following condition : objectUID < 0");
			}
		}
	}
}
