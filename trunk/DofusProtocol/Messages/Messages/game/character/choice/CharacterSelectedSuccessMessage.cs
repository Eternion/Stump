// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'CharacterSelectedSuccessMessage.xml' the '03/10/2011 12:46:55'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class CharacterSelectedSuccessMessage : Message
	{
		public const uint Id = 153;
		public override uint MessageId
		{
			get
			{
				return 153;
			}
		}
		
		public Types.CharacterBaseInformations infos;
		
		public CharacterSelectedSuccessMessage()
		{
		}
		
		public CharacterSelectedSuccessMessage(Types.CharacterBaseInformations infos)
		{
			this.infos = infos;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			infos.Serialize(writer);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			infos = new Types.CharacterBaseInformations();
			infos.Deserialize(reader);
		}
	}
}
