// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'StorageInventoryContentMessage.xml' the '03/10/2011 12:47:09'
using System;
using Stump.Core.IO;
using System.Collections.Generic;

namespace Stump.DofusProtocol.Messages
{
	public class StorageInventoryContentMessage : InventoryContentMessage
	{
		public const uint Id = 5646;
		public override uint MessageId
		{
			get
			{
				return 5646;
			}
		}
		
		
		public StorageInventoryContentMessage()
		{
		}
		
		public StorageInventoryContentMessage(IEnumerable<Types.ObjectItem> objects, int kamas)
			 : base(objects, kamas)
		{
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
		}
	}
}
