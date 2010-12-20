using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class MapComplementaryInformationsDataMessage : Message
	{
		public const uint protocolId = 226;
		internal Boolean _isInitialized = false;
		public uint subareaId = 0;
		public uint mapId = 0;
		public int subareaAlignmentSide = 0;
		public List<HouseInformations> houses;
		public List<GameRolePlayActorInformations> actors;
		public List<InteractiveElement> interactiveElements;
		public List<StatedElement> statedElements;
		public List<MapObstacle> obstacles;
		public List<FightCommonInformations> fights;
		
		public MapComplementaryInformationsDataMessage()
		{
			this.houses = new List<HouseInformations>();
			this.actors = new List<GameRolePlayActorInformations>();
			this.interactiveElements = new List<InteractiveElement>();
			this.statedElements = new List<StatedElement>();
			this.obstacles = new List<MapObstacle>();
			this.fights = new List<FightCommonInformations>();
		}
		
		public MapComplementaryInformationsDataMessage(uint arg1, uint arg2, int arg3, List<HouseInformations> arg4, List<GameRolePlayActorInformations> arg5, List<InteractiveElement> arg6, List<StatedElement> arg7, List<MapObstacle> arg8, List<FightCommonInformations> arg9)
			: this()
		{
			initMapComplementaryInformationsDataMessage(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
		}
		
		public override uint getMessageId()
		{
			return 226;
		}
		
		public MapComplementaryInformationsDataMessage initMapComplementaryInformationsDataMessage(uint arg1 = 0, uint arg2 = 0, int arg3 = 0, List<HouseInformations> arg4 = null, List<GameRolePlayActorInformations> arg5 = null, List<InteractiveElement> arg6 = null, List<StatedElement> arg7 = null, List<MapObstacle> arg8 = null, List<FightCommonInformations> arg9 = null)
		{
			this.subareaId = arg1;
			this.mapId = arg2;
			this.subareaAlignmentSide = arg3;
			this.houses = arg4;
			this.actors = arg5;
			this.interactiveElements = arg6;
			this.statedElements = arg7;
			this.obstacles = arg8;
			this.fights = arg9;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.subareaId = 0;
			this.mapId = 0;
			this.subareaAlignmentSide = 0;
			this.houses = new List<HouseInformations>();
			this.actors = new List<GameRolePlayActorInformations>();
			this.interactiveElements = new List<InteractiveElement>();
			this.statedElements = new List<StatedElement>();
			this.obstacles = new List<MapObstacle>();
			this.fights = new List<FightCommonInformations>();
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
			this.serializeAs_MapComplementaryInformationsDataMessage(arg1);
		}
		
		public void serializeAs_MapComplementaryInformationsDataMessage(BigEndianWriter arg1)
		{
			if ( this.subareaId < 0 )
			{
				throw new Exception("Forbidden value (" + this.subareaId + ") on element subareaId.");
			}
			arg1.WriteShort((short)this.subareaId);
			if ( this.mapId < 0 )
			{
				throw new Exception("Forbidden value (" + this.mapId + ") on element mapId.");
			}
			arg1.WriteInt((int)this.mapId);
			arg1.WriteByte((byte)this.subareaAlignmentSide);
			arg1.WriteShort((short)this.houses.Count);
			var loc1 = 0;
			while ( loc1 < this.houses.Count )
			{
				arg1.WriteShort((short)this.houses[loc1].getTypeId());
				this.houses[loc1].serialize(arg1);
				++loc1;
			}
			arg1.WriteShort((short)this.actors.Count);
			var loc2 = 0;
			while ( loc2 < this.actors.Count )
			{
				arg1.WriteShort((short)this.actors[loc2].getTypeId());
				this.actors[loc2].serialize(arg1);
				++loc2;
			}
			arg1.WriteShort((short)this.interactiveElements.Count);
			var loc3 = 0;
			while ( loc3 < this.interactiveElements.Count )
			{
				this.interactiveElements[loc3].serializeAs_InteractiveElement(arg1);
				++loc3;
			}
			arg1.WriteShort((short)this.statedElements.Count);
			var loc4 = 0;
			while ( loc4 < this.statedElements.Count )
			{
				this.statedElements[loc4].serializeAs_StatedElement(arg1);
				++loc4;
			}
			arg1.WriteShort((short)this.obstacles.Count);
			var loc5 = 0;
			while ( loc5 < this.obstacles.Count )
			{
				this.obstacles[loc5].serializeAs_MapObstacle(arg1);
				++loc5;
			}
			arg1.WriteShort((short)this.fights.Count);
			var loc6 = 0;
			while ( loc6 < this.fights.Count )
			{
				this.fights[loc6].serializeAs_FightCommonInformations(arg1);
				++loc6;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_MapComplementaryInformationsDataMessage(arg1);
		}
		
		public void deserializeAs_MapComplementaryInformationsDataMessage(BigEndianReader arg1)
		{
			var loc13 = 0;
			object loc14 = null;
			var loc15 = 0;
			object loc16 = null;
			object loc17 = null;
			object loc18 = null;
			object loc19 = null;
			object loc20 = null;
			this.subareaId = (uint)arg1.ReadShort();
			if ( this.subareaId < 0 )
			{
				throw new Exception("Forbidden value (" + this.subareaId + ") on element of MapComplementaryInformationsDataMessage.subareaId.");
			}
			this.mapId = (uint)arg1.ReadInt();
			if ( this.mapId < 0 )
			{
				throw new Exception("Forbidden value (" + this.mapId + ") on element of MapComplementaryInformationsDataMessage.mapId.");
			}
			this.subareaAlignmentSide = (int)arg1.ReadByte();
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc13 = (ushort)arg1.ReadUShort();
				(( loc14 = ProtocolTypeManager.GetInstance<HouseInformations>((uint)loc13)) as HouseInformations).deserialize(arg1);
				this.houses.Add((HouseInformations)loc14);
				++loc2;
			}
			var loc3 = (ushort)arg1.ReadUShort();
			var loc4 = 0;
			while ( loc4 < loc3 )
			{
				loc15 = (ushort)arg1.ReadUShort();
				(( loc16 = ProtocolTypeManager.GetInstance<GameRolePlayActorInformations>((uint)loc15)) as GameRolePlayActorInformations).deserialize(arg1);
				this.actors.Add((GameRolePlayActorInformations)loc16);
				++loc4;
			}
			var loc5 = (ushort)arg1.ReadUShort();
			var loc6 = 0;
			while ( loc6 < loc5 )
			{
				((loc17 = new InteractiveElement()) as InteractiveElement).deserialize(arg1);
				this.interactiveElements.Add((InteractiveElement)loc17);
				++loc6;
			}
			var loc7 = (ushort)arg1.ReadUShort();
			var loc8 = 0;
			while ( loc8 < loc7 )
			{
				((loc18 = new StatedElement()) as StatedElement).deserialize(arg1);
				this.statedElements.Add((StatedElement)loc18);
				++loc8;
			}
			var loc9 = (ushort)arg1.ReadUShort();
			var loc10 = 0;
			while ( loc10 < loc9 )
			{
				((loc19 = new MapObstacle()) as MapObstacle).deserialize(arg1);
				this.obstacles.Add((MapObstacle)loc19);
				++loc10;
			}
			var loc11 = (ushort)arg1.ReadUShort();
			var loc12 = 0;
			while ( loc12 < loc11 )
			{
				((loc20 = new FightCommonInformations()) as FightCommonInformations).deserialize(arg1);
				this.fights.Add((FightCommonInformations)loc20);
				++loc12;
			}
		}
		
	}
}
