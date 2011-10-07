// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'PaddockInformations.xml' the '03/10/2011 12:47:13'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
	public class PaddockInformations
	{
		public const uint Id = 132;
		public virtual short TypeId
		{
			get
			{
				return 132;
			}
		}
		
		public short maxOutdoorMount;
		public short maxItems;
		
		public PaddockInformations()
		{
		}
		
		public PaddockInformations(short maxOutdoorMount, short maxItems)
		{
			this.maxOutdoorMount = maxOutdoorMount;
			this.maxItems = maxItems;
		}
		
		public virtual void Serialize(IDataWriter writer)
		{
			writer.WriteShort(maxOutdoorMount);
			writer.WriteShort(maxItems);
		}
		
		public virtual void Deserialize(IDataReader reader)
		{
			maxOutdoorMount = reader.ReadShort();
			if ( maxOutdoorMount < 0 )
			{
				throw new Exception("Forbidden value on maxOutdoorMount = " + maxOutdoorMount + ", it doesn't respect the following condition : maxOutdoorMount < 0");
			}
			maxItems = reader.ReadShort();
			if ( maxItems < 0 )
			{
				throw new Exception("Forbidden value on maxItems = " + maxItems + ", it doesn't respect the following condition : maxItems < 0");
			}
		}
	}
}
