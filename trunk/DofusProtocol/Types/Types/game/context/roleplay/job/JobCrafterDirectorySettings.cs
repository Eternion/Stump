// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'JobCrafterDirectorySettings.xml' the '03/10/2011 12:47:12'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
	public class JobCrafterDirectorySettings
	{
		public const uint Id = 97;
		public virtual short TypeId
		{
			get
			{
				return 97;
			}
		}
		
		public sbyte jobId;
		public sbyte minSlot;
		public sbyte userDefinedParams;
		
		public JobCrafterDirectorySettings()
		{
		}
		
		public JobCrafterDirectorySettings(sbyte jobId, sbyte minSlot, sbyte userDefinedParams)
		{
			this.jobId = jobId;
			this.minSlot = minSlot;
			this.userDefinedParams = userDefinedParams;
		}
		
		public virtual void Serialize(IDataWriter writer)
		{
			writer.WriteSByte(jobId);
			writer.WriteSByte(minSlot);
			writer.WriteSByte(userDefinedParams);
		}
		
		public virtual void Deserialize(IDataReader reader)
		{
			jobId = reader.ReadSByte();
			if ( jobId < 0 )
			{
				throw new Exception("Forbidden value on jobId = " + jobId + ", it doesn't respect the following condition : jobId < 0");
			}
			minSlot = reader.ReadSByte();
			if ( minSlot < 0 || minSlot > 9 )
			{
				throw new Exception("Forbidden value on minSlot = " + minSlot + ", it doesn't respect the following condition : minSlot < 0 || minSlot > 9");
			}
			userDefinedParams = reader.ReadSByte();
			if ( userDefinedParams < 0 )
			{
				throw new Exception("Forbidden value on userDefinedParams = " + userDefinedParams + ", it doesn't respect the following condition : userDefinedParams < 0");
			}
		}
	}
}
