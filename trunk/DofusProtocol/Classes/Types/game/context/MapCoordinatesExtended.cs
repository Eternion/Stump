using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class MapCoordinatesExtended : MapCoordinates
	{
		public const uint protocolId = 176;
		public uint mapId = 0;
		
		public MapCoordinatesExtended()
		{
		}
		
		public MapCoordinatesExtended(int arg1, int arg2, uint arg3)
			: this()
		{
			initMapCoordinatesExtended(arg1, arg2, arg3);
		}
		
		public override uint getTypeId()
		{
			return 176;
		}
		
		public MapCoordinatesExtended initMapCoordinatesExtended(int arg1 = 0, int arg2 = 0, uint arg3 = 0)
		{
			base.initMapCoordinates(arg1, arg2);
			this.mapId = arg3;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.mapId = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_MapCoordinatesExtended(arg1);
		}
		
		public void serializeAs_MapCoordinatesExtended(BigEndianWriter arg1)
		{
			base.serializeAs_MapCoordinates(arg1);
			if ( this.mapId < 0 )
			{
				throw new Exception("Forbidden value (" + this.mapId + ") on element mapId.");
			}
			arg1.WriteInt((int)this.mapId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_MapCoordinatesExtended(arg1);
		}
		
		public void deserializeAs_MapCoordinatesExtended(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.mapId = (uint)arg1.ReadInt();
			if ( this.mapId < 0 )
			{
				throw new Exception("Forbidden value (" + this.mapId + ") on element of MapCoordinatesExtended.mapId.");
			}
		}
		
	}
}
