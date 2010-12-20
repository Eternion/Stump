using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class SellerBuyerDescriptor : Object
	{
		public const uint protocolId = 121;
		public List<uint> quantities;
		public List<uint> types;
		public double taxPercentage = 0;
		public uint maxItemLevel = 0;
		public uint maxItemPerAccount = 0;
		public int npcContextualId = 0;
		public uint unsoldDelay = 0;
		
		public SellerBuyerDescriptor()
		{
			this.quantities = new List<uint>();
			this.types = new List<uint>();
		}
		
		public SellerBuyerDescriptor(List<uint> arg1, List<uint> arg2, double arg3, uint arg4, uint arg5, int arg6, uint arg7)
			: this()
		{
			initSellerBuyerDescriptor(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}
		
		public virtual uint getTypeId()
		{
			return 121;
		}
		
		public SellerBuyerDescriptor initSellerBuyerDescriptor(List<uint> arg1, List<uint> arg2, double arg3 = 0, uint arg4 = 0, uint arg5 = 0, int arg6 = 0, uint arg7 = 0)
		{
			this.quantities = arg1;
			this.types = arg2;
			this.taxPercentage = arg3;
			this.maxItemLevel = arg4;
			this.maxItemPerAccount = arg5;
			this.npcContextualId = arg6;
			this.unsoldDelay = arg7;
			return this;
		}
		
		public virtual void reset()
		{
			this.quantities = new List<uint>();
			this.types = new List<uint>();
			this.taxPercentage = 0;
			this.maxItemLevel = 0;
			this.maxItemPerAccount = 0;
			this.npcContextualId = 0;
			this.unsoldDelay = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_SellerBuyerDescriptor(arg1);
		}
		
		public void serializeAs_SellerBuyerDescriptor(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.quantities.Count);
			var loc1 = 0;
			while ( loc1 < this.quantities.Count )
			{
				if ( this.quantities[loc1] < 0 )
				{
					throw new Exception("Forbidden value (" + this.quantities[loc1] + ") on element 1 (starting at 1) of quantities.");
				}
				arg1.WriteInt((int)this.quantities[loc1]);
				++loc1;
			}
			arg1.WriteShort((short)this.types.Count);
			var loc2 = 0;
			while ( loc2 < this.types.Count )
			{
				if ( this.types[loc2] < 0 )
				{
					throw new Exception("Forbidden value (" + this.types[loc2] + ") on element 2 (starting at 1) of types.");
				}
				arg1.WriteInt((int)this.types[loc2]);
				++loc2;
			}
			arg1.WriteFloat((uint)this.taxPercentage);
			if ( this.maxItemLevel < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxItemLevel + ") on element maxItemLevel.");
			}
			arg1.WriteInt((int)this.maxItemLevel);
			if ( this.maxItemPerAccount < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxItemPerAccount + ") on element maxItemPerAccount.");
			}
			arg1.WriteInt((int)this.maxItemPerAccount);
			arg1.WriteInt((int)this.npcContextualId);
			if ( this.unsoldDelay < 0 )
			{
				throw new Exception("Forbidden value (" + this.unsoldDelay + ") on element unsoldDelay.");
			}
			arg1.WriteShort((short)this.unsoldDelay);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_SellerBuyerDescriptor(arg1);
		}
		
		public void deserializeAs_SellerBuyerDescriptor(BigEndianReader arg1)
		{
			var loc5 = 0;
			var loc6 = 0;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				if ( (loc5 = arg1.ReadInt()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc5 + ") on elements of quantities.");
				}
				this.quantities.Add((uint)loc5);
				++loc2;
			}
			var loc3 = (ushort)arg1.ReadUShort();
			var loc4 = 0;
			while ( loc4 < loc3 )
			{
				if ( (loc6 = arg1.ReadInt()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc6 + ") on elements of types.");
				}
				this.types.Add((uint)loc6);
				++loc4;
			}
			this.taxPercentage = (double)arg1.ReadFloat();
			this.maxItemLevel = (uint)arg1.ReadInt();
			if ( this.maxItemLevel < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxItemLevel + ") on element of SellerBuyerDescriptor.maxItemLevel.");
			}
			this.maxItemPerAccount = (uint)arg1.ReadInt();
			if ( this.maxItemPerAccount < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxItemPerAccount + ") on element of SellerBuyerDescriptor.maxItemPerAccount.");
			}
			this.npcContextualId = (int)arg1.ReadInt();
			this.unsoldDelay = (uint)arg1.ReadShort();
			if ( this.unsoldDelay < 0 )
			{
				throw new Exception("Forbidden value (" + this.unsoldDelay + ") on element of SellerBuyerDescriptor.unsoldDelay.");
			}
		}
		
	}
}
