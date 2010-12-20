using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class MapObstacleUpdateMessage : Message
	{
		public const uint protocolId = 6051;
		internal Boolean _isInitialized = false;
		public List<MapObstacle> obstacles;
		
		public MapObstacleUpdateMessage()
		{
			this.obstacles = new List<MapObstacle>();
		}
		
		public MapObstacleUpdateMessage(List<MapObstacle> arg1)
			: this()
		{
			initMapObstacleUpdateMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6051;
		}
		
		public MapObstacleUpdateMessage initMapObstacleUpdateMessage(List<MapObstacle> arg1)
		{
			this.obstacles = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.obstacles = new List<MapObstacle>();
			this._isInitialized = false;
		}
		
		public override void pack(BigEndianWriter arg1)
		{
			this.serialize(arg1);
			WritePacket(arg1, this.getMessageId());
		}
		
		public override void unpack(BigEndianReader arg1, uint arg2)
		{
			this.deserialize(arg1);
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_MapObstacleUpdateMessage(arg1);
		}
		
		public void serializeAs_MapObstacleUpdateMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.obstacles.Count);
			var loc1 = 0;
			while ( loc1 < this.obstacles.Count )
			{
				this.obstacles[loc1].serializeAs_MapObstacle(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_MapObstacleUpdateMessage(arg1);
		}
		
		public void deserializeAs_MapObstacleUpdateMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new MapObstacle()) as MapObstacle).deserialize(arg1);
				this.obstacles.Add((MapObstacle)loc3);
				++loc2;
			}
		}
		
	}
}
