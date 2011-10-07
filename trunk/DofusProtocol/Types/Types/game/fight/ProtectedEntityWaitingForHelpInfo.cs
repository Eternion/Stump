// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'ProtectedEntityWaitingForHelpInfo.xml' the '03/10/2011 12:47:12'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
	public class ProtectedEntityWaitingForHelpInfo
	{
		public const uint Id = 186;
		public virtual short TypeId
		{
			get
			{
				return 186;
			}
		}
		
		public int timeLeftBeforeFight;
		public int waitTimeForPlacement;
		public sbyte nbPositionForDefensors;
		
		public ProtectedEntityWaitingForHelpInfo()
		{
		}
		
		public ProtectedEntityWaitingForHelpInfo(int timeLeftBeforeFight, int waitTimeForPlacement, sbyte nbPositionForDefensors)
		{
			this.timeLeftBeforeFight = timeLeftBeforeFight;
			this.waitTimeForPlacement = waitTimeForPlacement;
			this.nbPositionForDefensors = nbPositionForDefensors;
		}
		
		public virtual void Serialize(IDataWriter writer)
		{
			writer.WriteInt(timeLeftBeforeFight);
			writer.WriteInt(waitTimeForPlacement);
			writer.WriteSByte(nbPositionForDefensors);
		}
		
		public virtual void Deserialize(IDataReader reader)
		{
			timeLeftBeforeFight = reader.ReadInt();
			waitTimeForPlacement = reader.ReadInt();
			nbPositionForDefensors = reader.ReadSByte();
			if ( nbPositionForDefensors < 0 )
			{
				throw new Exception("Forbidden value on nbPositionForDefensors = " + nbPositionForDefensors + ", it doesn't respect the following condition : nbPositionForDefensors < 0");
			}
		}
	}
}
