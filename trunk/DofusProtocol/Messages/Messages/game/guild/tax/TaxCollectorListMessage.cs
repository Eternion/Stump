using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class TaxCollectorListMessage : Message
	{
		public const uint protocolId = 5930;
		internal Boolean _isInitialized = false;
		public uint nbcollectorMax = 0;
		public uint taxCollectorHireCost = 0;
		public List<TaxCollectorInformations> informations;
		public List<TaxCollectorFightersInformation> fightersInformations;
		
		public TaxCollectorListMessage()
		{
			this.informations = new List<TaxCollectorInformations>();
			this.fightersInformations = new List<TaxCollectorFightersInformation>();
		}
		
		public TaxCollectorListMessage(uint arg1, uint arg2, List<TaxCollectorInformations> arg3, List<TaxCollectorFightersInformation> arg4)
			: this()
		{
			initTaxCollectorListMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 5930;
		}
		
		public TaxCollectorListMessage initTaxCollectorListMessage(uint arg1 = 0, uint arg2 = 0, List<TaxCollectorInformations> arg3 = null, List<TaxCollectorFightersInformation> arg4 = null)
		{
			this.nbcollectorMax = arg1;
			this.taxCollectorHireCost = arg2;
			this.informations = arg3;
			this.fightersInformations = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.nbcollectorMax = 0;
			this.taxCollectorHireCost = 0;
			this.informations = new List<TaxCollectorInformations>();
			this.fightersInformations = new List<TaxCollectorFightersInformation>();
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
			this.serializeAs_TaxCollectorListMessage(arg1);
		}
		
		public void serializeAs_TaxCollectorListMessage(BigEndianWriter arg1)
		{
			if ( this.nbcollectorMax < 0 )
			{
				throw new Exception("Forbidden value (" + this.nbcollectorMax + ") on element nbcollectorMax.");
			}
			arg1.WriteByte((byte)this.nbcollectorMax);
			if ( this.taxCollectorHireCost < 0 )
			{
				throw new Exception("Forbidden value (" + this.taxCollectorHireCost + ") on element taxCollectorHireCost.");
			}
			arg1.WriteShort((short)this.taxCollectorHireCost);
			arg1.WriteShort((short)this.informations.Count);
			var loc1 = 0;
			while ( loc1 < this.informations.Count )
			{
				arg1.WriteShort((short)this.informations[loc1].getTypeId());
				this.informations[loc1].serialize(arg1);
				++loc1;
			}
			arg1.WriteShort((short)this.fightersInformations.Count);
			var loc2 = 0;
			while ( loc2 < this.fightersInformations.Count )
			{
				this.fightersInformations[loc2].serializeAs_TaxCollectorFightersInformation(arg1);
				++loc2;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_TaxCollectorListMessage(arg1);
		}
		
		public void deserializeAs_TaxCollectorListMessage(BigEndianReader arg1)
		{
			var loc5 = 0;
			object loc6 = null;
			object loc7 = null;
			this.nbcollectorMax = (uint)arg1.ReadByte();
			if ( this.nbcollectorMax < 0 )
			{
				throw new Exception("Forbidden value (" + this.nbcollectorMax + ") on element of TaxCollectorListMessage.nbcollectorMax.");
			}
			this.taxCollectorHireCost = (uint)arg1.ReadShort();
			if ( this.taxCollectorHireCost < 0 )
			{
				throw new Exception("Forbidden value (" + this.taxCollectorHireCost + ") on element of TaxCollectorListMessage.taxCollectorHireCost.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc5 = (ushort)arg1.ReadUShort();
				(( loc6 = ProtocolTypeManager.GetInstance<TaxCollectorInformations>((uint)loc5)) as TaxCollectorInformations).deserialize(arg1);
				this.informations.Add((TaxCollectorInformations)loc6);
				++loc2;
			}
			var loc3 = (ushort)arg1.ReadUShort();
			var loc4 = 0;
			while ( loc4 < loc3 )
			{
				((loc7 = new TaxCollectorFightersInformation()) as TaxCollectorFightersInformation).deserialize(arg1);
				this.fightersInformations.Add((TaxCollectorFightersInformation)loc7);
				++loc4;
			}
		}
		
	}
}
