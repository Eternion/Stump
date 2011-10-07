// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'EntityMovementInformations.xml' the '03/10/2011 12:47:10'
using System;
using Stump.Core.IO;
using System.Collections.Generic;
using System.Linq;

namespace Stump.DofusProtocol.Types
{
	public class EntityMovementInformations
	{
		public const uint Id = 63;
		public virtual short TypeId
		{
			get
			{
				return 63;
			}
		}
		
		public int id;
		public IEnumerable<sbyte> steps;
		
		public EntityMovementInformations()
		{
		}
		
		public EntityMovementInformations(int id, IEnumerable<sbyte> steps)
		{
			this.id = id;
			this.steps = steps;
		}
		
		public virtual void Serialize(IDataWriter writer)
		{
			writer.WriteInt(id);
			writer.WriteUShort((ushort)steps.Count());
			foreach (var entry in steps)
			{
				writer.WriteSByte(entry);
			}
		}
		
		public virtual void Deserialize(IDataReader reader)
		{
			id = reader.ReadInt();
			int limit = reader.ReadUShort();
			steps = new sbyte[limit];
			for (int i = 0; i < limit; i++)
			{
				(steps as sbyte[])[i] = reader.ReadSByte();
			}
		}
	}
}
