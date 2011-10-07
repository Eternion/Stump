// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ObjectModifiedMessage.xml' the '03/10/2011 12:47:08'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class ObjectModifiedMessage : Message
	{
		public const uint Id = 3029;
		public override uint MessageId
		{
			get
			{
				return 3029;
			}
		}
		
		public Types.ObjectItem @object;
		
		public ObjectModifiedMessage()
		{
		}
		
		public ObjectModifiedMessage(Types.ObjectItem @object)
		{
			this.@object = @object;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			@object.Serialize(writer);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			@object = new Types.ObjectItem();
			@object.Deserialize(reader);
		}
	}
}
