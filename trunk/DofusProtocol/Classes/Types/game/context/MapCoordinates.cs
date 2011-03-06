using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class MapCoordinates : Object
	{
		public const uint protocolId = 174;
		public int worldX = 0;
		public int worldY = 0;
		
		public MapCoordinates()
		{
		}
		
		public MapCoordinates(int arg1, int arg2)
			: this()
		{
			initMapCoordinates(arg1, arg2);
		}
		
		public virtual uint getTypeId()
		{
			return 174;
		}
		
		public MapCoordinates initMapCoordinates(int arg1 = 0, int arg2 = 0)
		{
			this.worldX = arg1;
			this.worldY = arg2;
			return this;
		}
		
		public virtual void reset()
		{
			this.worldX = 0;
			this.worldY = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_MapCoordinates(arg1);
		}
		
		public void serializeAs_MapCoordinates(BigEndianWriter arg1)
		{
			if ( this.worldX < -255 || this.worldX > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldX + ") on element worldX.");
			}
			arg1.WriteShort((short)this.worldX);
			if ( this.worldY < -255 || this.worldY > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldY + ") on element worldY.");
			}
			arg1.WriteShort((short)this.worldY);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_MapCoordinates(arg1);
		}
		
		public void deserializeAs_MapCoordinates(BigEndianReader arg1)
		{
			this.worldX = (int)arg1.ReadShort();
			if ( this.worldX < -255 || this.worldX > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldX + ") on element of MapCoordinates.worldX.");
			}
			this.worldY = (int)arg1.ReadShort();
			if ( this.worldY < -255 || this.worldY > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldY + ") on element of MapCoordinates.worldY.");
			}
		}
		
	}
}
