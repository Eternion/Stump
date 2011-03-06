using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class ObjectItemToSellInHumanVendorShop : Item
	{
		public const uint protocolId = 359;
		public uint objectGID = 0;
		public int powerRate = 0;
		public Boolean overMax = false;
		public List<ObjectEffect> effects;
		public uint objectUID = 0;
		public uint quantity = 0;
		public uint objectPrice = 0;
		public uint publicPrice = 0;
		
		public ObjectItemToSellInHumanVendorShop()
		{
			this.effects = new List<ObjectEffect>();
		}
		
		public ObjectItemToSellInHumanVendorShop(uint arg1, int arg2, Boolean arg3, List<ObjectEffect> arg4, uint arg5, uint arg6, uint arg7, uint arg8)
			: this()
		{
			initObjectItemToSellInHumanVendorShop(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
		}
		
		public override uint getTypeId()
		{
			return 359;
		}
		
		public ObjectItemToSellInHumanVendorShop initObjectItemToSellInHumanVendorShop(uint arg1 = 0, int arg2 = 0, Boolean arg3 = false, List<ObjectEffect> arg4 = null, uint arg5 = 0, uint arg6 = 0, uint arg7 = 0, uint arg8 = 0)
		{
			this.@objectGID = arg1;
			this.powerRate = arg2;
			this.overMax = arg3;
			this.effects = arg4;
			this.@objectUID = arg5;
			this.quantity = arg6;
			this.@objectPrice = arg7;
			this.publicPrice = arg8;
			return this;
		}
		
		public override void reset()
		{
			this.@objectGID = 0;
			this.powerRate = 0;
			this.overMax = false;
			this.effects = new List<ObjectEffect>();
			this.@objectUID = 0;
			this.quantity = 0;
			this.@objectPrice = 0;
			this.publicPrice = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ObjectItemToSellInHumanVendorShop(arg1);
		}
		
		public void serializeAs_ObjectItemToSellInHumanVendorShop(BigEndianWriter arg1)
		{
			base.serializeAs_Item(arg1);
			if ( this.@objectGID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectGID + ") on element objectGID.");
			}
			arg1.WriteShort((short)this.@objectGID);
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
			if ( this.@objectUID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectUID + ") on element objectUID.");
			}
			arg1.WriteInt((int)this.@objectUID);
			if ( this.quantity < 0 )
			{
				throw new Exception("Forbidden value (" + this.quantity + ") on element quantity.");
			}
			arg1.WriteInt((int)this.quantity);
			if ( this.@objectPrice < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectPrice + ") on element objectPrice.");
			}
			arg1.WriteInt((int)this.@objectPrice);
			if ( this.publicPrice < 0 )
			{
				throw new Exception("Forbidden value (" + this.publicPrice + ") on element publicPrice.");
			}
			arg1.WriteInt((int)this.publicPrice);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectItemToSellInHumanVendorShop(arg1);
		}
		
		public void deserializeAs_ObjectItemToSellInHumanVendorShop(BigEndianReader arg1)
		{
			var loc3 = 0;
			object loc4 = null;
			base.deserialize(arg1);
			this.@objectGID = (uint)arg1.ReadShort();
			if ( this.@objectGID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectGID + ") on element of ObjectItemToSellInHumanVendorShop.objectGID.");
			}
			this.powerRate = (int)arg1.ReadShort();
			this.overMax = (Boolean)arg1.ReadBoolean();
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc3 = (ushort)arg1.ReadUShort();
				(( loc4 = ProtocolTypeManager.GetInstance<ObjectEffect>((uint)loc3)) as ObjectEffect).deserialize(arg1);
				this.effects.Add((ObjectEffect)loc4);
				++loc2;
			}
			this.@objectUID = (uint)arg1.ReadInt();
			if ( this.@objectUID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectUID + ") on element of ObjectItemToSellInHumanVendorShop.objectUID.");
			}
			this.quantity = (uint)arg1.ReadInt();
			if ( this.quantity < 0 )
			{
				throw new Exception("Forbidden value (" + this.quantity + ") on element of ObjectItemToSellInHumanVendorShop.quantity.");
			}
			this.@objectPrice = (uint)arg1.ReadInt();
			if ( this.@objectPrice < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectPrice + ") on element of ObjectItemToSellInHumanVendorShop.objectPrice.");
			}
			this.publicPrice = (uint)arg1.ReadInt();
			if ( this.publicPrice < 0 )
			{
				throw new Exception("Forbidden value (" + this.publicPrice + ") on element of ObjectItemToSellInHumanVendorShop.publicPrice.");
			}
		}
		
	}
}
