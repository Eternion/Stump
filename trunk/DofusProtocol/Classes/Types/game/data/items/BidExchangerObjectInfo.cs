using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class BidExchangerObjectInfo : Object
	{
		public const uint protocolId = 122;
		public uint objectUID = 0;
		public int powerRate = 0;
		public Boolean overMax = false;
		public List<ObjectEffect> effects;
		public List<uint> prices;
		
		public BidExchangerObjectInfo()
		{
			this.effects = new List<ObjectEffect>();
			this.prices = new List<uint>();
		}
		
		public BidExchangerObjectInfo(uint arg1, int arg2, Boolean arg3, List<ObjectEffect> arg4, List<uint> arg5)
			: this()
		{
			initBidExchangerObjectInfo(arg1, arg2, arg3, arg4, arg5);
		}
		
		public virtual uint getTypeId()
		{
			return 122;
		}
		
		public BidExchangerObjectInfo initBidExchangerObjectInfo(uint arg1 = 0, int arg2 = 0, Boolean arg3 = false, List<ObjectEffect> arg4 = null, List<uint> arg5 = null)
		{
			this.@objectUID = arg1;
			this.powerRate = arg2;
			this.overMax = arg3;
			this.effects = arg4;
			this.prices = arg5;
			return this;
		}
		
		public virtual void reset()
		{
			this.@objectUID = 0;
			this.powerRate = 0;
			this.overMax = false;
			this.effects = new List<ObjectEffect>();
			this.prices = new List<uint>();
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_BidExchangerObjectInfo(arg1);
		}
		
		public void serializeAs_BidExchangerObjectInfo(BigEndianWriter arg1)
		{
			if ( this.@objectUID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectUID + ") on element objectUID.");
			}
			arg1.WriteInt((int)this.@objectUID);
			arg1.WriteShort((short)this.powerRate);
			arg1.WriteBoolean(this.overMax);
			arg1.WriteShort((short)this.effects.Count);
			var loc1 = 0;
			while ( loc1 < this.effects.Count )
			{
				arg1.WriteShort((short)this.effects[loc1].getTypeId());
				this.effects[loc1].serialize(arg1);
				++loc1;
			}
			arg1.WriteShort((short)this.prices.Count);
			var loc2 = 0;
			while ( loc2 < this.prices.Count )
			{
				if ( this.prices[loc2] < 0 )
				{
					throw new Exception("Forbidden value (" + this.prices[loc2] + ") on element 5 (starting at 1) of prices.");
				}
				arg1.WriteInt((int)this.prices[loc2]);
				++loc2;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_BidExchangerObjectInfo(arg1);
		}
		
		public void deserializeAs_BidExchangerObjectInfo(BigEndianReader arg1)
		{
			var loc5 = 0;
			object loc6 = null;
			var loc7 = 0;
			this.@objectUID = (uint)arg1.ReadInt();
			if ( this.@objectUID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectUID + ") on element of BidExchangerObjectInfo.objectUID.");
			}
			this.powerRate = (int)arg1.ReadShort();
			this.overMax = (Boolean)arg1.ReadBoolean();
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc5 = (ushort)arg1.ReadUShort();
				(( loc6 = ProtocolTypeManager.GetInstance<ObjectEffect>((uint)loc5)) as ObjectEffect).deserialize(arg1);
				this.effects.Add((ObjectEffect)loc6);
				++loc2;
			}
			var loc3 = (ushort)arg1.ReadUShort();
			var loc4 = 0;
			while ( loc4 < loc3 )
			{
				if ( (loc7 = arg1.ReadInt()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc7 + ") on elements of prices.");
				}
				this.prices.Add((uint)loc7);
				++loc4;
			}
		}
		
	}
}
