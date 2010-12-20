using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class TeleportDestinationsListMessage : Message
	{
		public const uint protocolId = 5960;
		internal Boolean _isInitialized = false;
		public uint teleporterType = 0;
		public List<uint> mapIds;
		public List<uint> subareaIds;
		public List<uint> costs;
		
		public TeleportDestinationsListMessage()
		{
			this.mapIds = new List<uint>();
			this.subareaIds = new List<uint>();
			this.costs = new List<uint>();
		}
		
		public TeleportDestinationsListMessage(uint arg1, List<uint> arg2, List<uint> arg3, List<uint> arg4)
			: this()
		{
			initTeleportDestinationsListMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 5960;
		}
		
		public TeleportDestinationsListMessage initTeleportDestinationsListMessage(uint arg1 = 0, List<uint> arg2 = null, List<uint> arg3 = null, List<uint> arg4 = null)
		{
			this.teleporterType = arg1;
			this.mapIds = arg2;
			this.subareaIds = arg3;
			this.costs = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.teleporterType = 0;
			this.mapIds = new List<uint>();
			this.subareaIds = new List<uint>();
			this.costs = new List<uint>();
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
			this.serializeAs_TeleportDestinationsListMessage(arg1);
		}
		
		public void serializeAs_TeleportDestinationsListMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.teleporterType);
			arg1.WriteShort((short)this.mapIds.Count);
			var loc1 = 0;
			while ( loc1 < this.mapIds.Count )
			{
				if ( this.mapIds[loc1] < 0 )
				{
					throw new Exception("Forbidden value (" + this.mapIds[loc1] + ") on element 2 (starting at 1) of mapIds.");
				}
				arg1.WriteInt((int)this.mapIds[loc1]);
				++loc1;
			}
			arg1.WriteShort((short)this.subareaIds.Count);
			var loc2 = 0;
			while ( loc2 < this.subareaIds.Count )
			{
				if ( this.subareaIds[loc2] < 0 )
				{
					throw new Exception("Forbidden value (" + this.subareaIds[loc2] + ") on element 3 (starting at 1) of subareaIds.");
				}
				arg1.WriteShort((short)this.subareaIds[loc2]);
				++loc2;
			}
			arg1.WriteShort((short)this.costs.Count);
			var loc3 = 0;
			while ( loc3 < this.costs.Count )
			{
				if ( this.costs[loc3] < 0 )
				{
					throw new Exception("Forbidden value (" + this.costs[loc3] + ") on element 4 (starting at 1) of costs.");
				}
				arg1.WriteShort((short)this.costs[loc3]);
				++loc3;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_TeleportDestinationsListMessage(arg1);
		}
		
		public void deserializeAs_TeleportDestinationsListMessage(BigEndianReader arg1)
		{
			var loc7 = 0;
			var loc8 = 0;
			var loc9 = 0;
			this.teleporterType = (uint)arg1.ReadByte();
			if ( this.teleporterType < 0 )
			{
				throw new Exception("Forbidden value (" + this.teleporterType + ") on element of TeleportDestinationsListMessage.teleporterType.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				if ( (loc7 = arg1.ReadInt()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc7 + ") on elements of mapIds.");
				}
				this.mapIds.Add((uint)loc7);
				++loc2;
			}
			var loc3 = (ushort)arg1.ReadUShort();
			var loc4 = 0;
			while ( loc4 < loc3 )
			{
				if ( (loc8 = arg1.ReadShort()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc8 + ") on elements of subareaIds.");
				}
				this.subareaIds.Add((uint)loc8);
				++loc4;
			}
			var loc5 = (ushort)arg1.ReadUShort();
			var loc6 = 0;
			while ( loc6 < loc5 )
			{
				if ( (loc9 = arg1.ReadShort()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc9 + ") on elements of costs.");
				}
				this.costs.Add((uint)loc9);
				++loc6;
			}
		}
		
	}
}
