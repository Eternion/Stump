// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'TaxCollectorAttackedResultMessage.xml' the '03/10/2011 12:47:04'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class TaxCollectorAttackedResultMessage : Message
	{
		public const uint Id = 5635;
		public override uint MessageId
		{
			get
			{
				return 5635;
			}
		}
		
		public bool deadOrAlive;
		public Types.TaxCollectorBasicInformations basicInfos;
		
		public TaxCollectorAttackedResultMessage()
		{
		}
		
		public TaxCollectorAttackedResultMessage(bool deadOrAlive, Types.TaxCollectorBasicInformations basicInfos)
		{
			this.deadOrAlive = deadOrAlive;
			this.basicInfos = basicInfos;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteBoolean(deadOrAlive);
			basicInfos.Serialize(writer);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			deadOrAlive = reader.ReadBoolean();
			basicInfos = new Types.TaxCollectorBasicInformations();
			basicInfos.Deserialize(reader);
		}
	}
}
