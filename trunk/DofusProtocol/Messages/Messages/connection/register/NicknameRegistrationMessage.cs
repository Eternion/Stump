// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'NicknameRegistrationMessage.xml' the '03/10/2011 12:46:52'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class NicknameRegistrationMessage : Message
	{
		public const uint Id = 5640;
		public override uint MessageId
		{
			get
			{
				return 5640;
			}
		}
		
		
		public override void Serialize(IDataWriter writer)
		{
		}
		
		public override void Deserialize(IDataReader reader)
		{
		}
	}
}
