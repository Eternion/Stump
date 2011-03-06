using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class MapObstacle : Object
	{
		public const uint protocolId = 200;
		public uint obstacleCellId = 0;
		public uint state = 0;
		
		public MapObstacle()
		{
		}
		
		public MapObstacle(uint arg1, uint arg2)
			: this()
		{
			initMapObstacle(arg1, arg2);
		}
		
		public virtual uint getTypeId()
		{
			return 200;
		}
		
		public MapObstacle initMapObstacle(uint arg1 = 0, uint arg2 = 0)
		{
			this.obstacleCellId = arg1;
			this.state = arg2;
			return this;
		}
		
		public virtual void reset()
		{
			this.obstacleCellId = 0;
			this.state = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_MapObstacle(arg1);
		}
		
		public void serializeAs_MapObstacle(BigEndianWriter arg1)
		{
			if ( this.obstacleCellId < 0 || this.obstacleCellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.obstacleCellId + ") on element obstacleCellId.");
			}
			arg1.WriteShort((short)this.obstacleCellId);
			arg1.WriteByte((byte)this.state);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_MapObstacle(arg1);
		}
		
		public void deserializeAs_MapObstacle(BigEndianReader arg1)
		{
			this.obstacleCellId = (uint)arg1.ReadShort();
			if ( this.obstacleCellId < 0 || this.obstacleCellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.obstacleCellId + ") on element of MapObstacle.obstacleCellId.");
			}
			this.state = (uint)arg1.ReadByte();
			if ( this.state < 0 )
			{
				throw new Exception("Forbidden value (" + this.state + ") on element of MapObstacle.state.");
			}
		}
		
	}
}
