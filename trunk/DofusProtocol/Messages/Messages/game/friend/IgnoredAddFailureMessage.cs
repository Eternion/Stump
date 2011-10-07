// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'IgnoredAddFailureMessage.xml' the '03/10/2011 12:47:03'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class IgnoredAddFailureMessage : Message
	{
		public const uint Id = 5679;
		public override uint MessageId
		{
			get
			{
				return 5679;
			}
		}
		
		public sbyte reason;
		
		public IgnoredAddFailureMessage()
		{
		}
		
		public IgnoredAddFailureMessage(sbyte reason)
		{
			this.reason = reason;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteSByte(reason);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			reason = reader.ReadSByte();
			if ( reason < 0 )
			{
				throw new Exception("Forbidden value on reason = " + reason + ", it doesn't respect the following condition : reason < 0");
			}
		}
	}
}
