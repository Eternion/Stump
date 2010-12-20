using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeBidHouseInListAddedMessage : Message
	{
		public const uint protocolId = 5949;
		internal Boolean _isInitialized = false;
		public int itemUID = 0;
		public int objGenericId = 0;
		public int powerRate = 0;
		public Boolean overMax = false;
		public List<ObjectEffect> effects;
		public List<uint> prices;
		
		public ExchangeBidHouseInListAddedMessage()
		{
			this.effects = new List<ObjectEffect>();
			this.prices = new List<uint>();
		}
		
		public ExchangeBidHouseInListAddedMessage(int arg1, int arg2, int arg3, Boolean arg4, List<ObjectEffect> arg5, List<uint> arg6)
			: this()
		{
			initExchangeBidHouseInListAddedMessage(arg1, arg2, arg3, arg4, arg5, arg6);
		}
		
		public override uint getMessageId()
		{
			return 5949;
		}
		
		public ExchangeBidHouseInListAddedMessage initExchangeBidHouseInListAddedMessage(int arg1 = 0, int arg2 = 0, int arg3 = 0, Boolean arg4 = false, List<ObjectEffect> arg5 = null, List<uint> arg6 = null)
		{
			this.itemUID = arg1;
			this.objGenericId = arg2;
			this.powerRate = arg3;
			this.overMax = arg4;
			this.effects = arg5;
			this.prices = arg6;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.itemUID = 0;
			this.objGenericId = 0;
			this.powerRate = 0;
			this.overMax = false;
			this.effects = new List<ObjectEffect>();
			this.prices = new List<uint>();
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
			this.serializeAs_ExchangeBidHouseInListAddedMessage(arg1);
		}
		
		public void serializeAs_ExchangeBidHouseInListAddedMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.itemUID);
			arg1.WriteInt((int)this.objGenericId);
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
					throw new Exception("Forbidden value (" + this.prices[loc2] + ") on element 6 (starting at 1) of prices.");
				}
				arg1.WriteInt((int)this.prices[loc2]);
				++loc2;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeBidHouseInListAddedMessage(arg1);
		}
		
		public void deserializeAs_ExchangeBidHouseInListAddedMessage(BigEndianReader arg1)
		{
			var loc5 = 0;
			object loc6 = null;
			var loc7 = 0;
			this.itemUID = (int)arg1.ReadInt();
			this.objGenericId = (int)arg1.ReadInt();
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
