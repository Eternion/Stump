// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'IdentificationAccountForceWithServerIdMessage.xml' the '03/10/2011 12:46:52'
using System;
using Stump.Core.IO;
using System.Collections.Generic;

namespace Stump.DofusProtocol.Messages
{
	public class IdentificationAccountForceWithServerIdMessage : IdentificationAccountForceMessage
	{
		public const uint Id = 6133;
		public override uint MessageId
		{
			get
			{
				return 6133;
			}
		}
		
		public short serverId;
		
		public IdentificationAccountForceWithServerIdMessage()
		{
		}
		
		public IdentificationAccountForceWithServerIdMessage(Types.Version version, string login, string password, IEnumerable<Types.TrustCertificate> certificate, bool autoconnect, string forcedAccountLogin, short serverId)
			 : base(version, login, password, certificate, autoconnect, forcedAccountLogin)
		{
			this.serverId = serverId;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			writer.WriteShort(serverId);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			serverId = reader.ReadShort();
		}
	}
}
