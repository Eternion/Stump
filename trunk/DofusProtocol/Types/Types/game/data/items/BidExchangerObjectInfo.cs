// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'BidExchangerObjectInfo.xml' the '03/10/2011 12:47:12'
using System;
using Stump.Core.IO;
using System.Collections.Generic;
using System.Linq;

namespace Stump.DofusProtocol.Types
{
	public class BidExchangerObjectInfo
	{
		public const uint Id = 122;
		public virtual short TypeId
		{
			get
			{
				return 122;
			}
		}
		
		public int objectUID;
		public short powerRate;
		public bool overMax;
		public IEnumerable<Types.ObjectEffect> effects;
		public IEnumerable<int> prices;
		
		public BidExchangerObjectInfo()
		{
		}
		
		public BidExchangerObjectInfo(int objectUID, short powerRate, bool overMax, IEnumerable<Types.ObjectEffect> effects, IEnumerable<int> prices)
		{
			this.objectUID = objectUID;
			this.powerRate = powerRate;
			this.overMax = overMax;
			this.effects = effects;
			this.prices = prices;
		}
		
		public virtual void Serialize(IDataWriter writer)
		{
			writer.WriteInt(objectUID);
			writer.WriteShort(powerRate);
			writer.WriteBoolean(overMax);
			writer.WriteUShort((ushort)effects.Count());
			foreach (var entry in effects)
			{
				writer.WriteShort(entry.TypeId);
				entry.Serialize(writer);
			}
			writer.WriteUShort((ushort)prices.Count());
			foreach (var entry in prices)
			{
				writer.WriteInt(entry);
			}
		}
		
		public virtual void Deserialize(IDataReader reader)
		{
			objectUID = reader.ReadInt();
			if ( objectUID < 0 )
			{
				throw new Exception("Forbidden value on objectUID = " + objectUID + ", it doesn't respect the following condition : objectUID < 0");
			}
			powerRate = reader.ReadShort();
			overMax = reader.ReadBoolean();
			int limit = reader.ReadUShort();
			effects = new Types.ObjectEffect[limit];
			for (int i = 0; i < limit; i++)
			{
				(effects as Types.ObjectEffect[])[i] = ProtocolTypeManager.GetInstance<Types.ObjectEffect>(reader.ReadShort());
				(effects as Types.ObjectEffect[])[i].Deserialize(reader);
			}
			limit = reader.ReadUShort();
			prices = new int[limit];
			for (int i = 0; i < limit; i++)
			{
				(prices as int[])[i] = reader.ReadInt();
			}
		}
	}
}
