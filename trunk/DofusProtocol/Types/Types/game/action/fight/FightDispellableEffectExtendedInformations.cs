// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'FightDispellableEffectExtendedInformations.xml' the '30/06/2011 11:40:22'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
	public class FightDispellableEffectExtendedInformations
	{
		public const uint Id = 208;
		public virtual short TypeId
		{
			get
			{
				return 208;
			}
		}
		
		public short actionId;
		public int sourceId;
		public Types.AbstractFightDispellableEffect effect;
		
		public FightDispellableEffectExtendedInformations()
		{
		}
		
		public FightDispellableEffectExtendedInformations(short actionId, int sourceId, Types.AbstractFightDispellableEffect effect)
		{
			this.actionId = actionId;
			this.sourceId = sourceId;
			this.effect = effect;
		}
		
		public virtual void Serialize(IDataWriter writer)
		{
			writer.WriteShort(actionId);
			writer.WriteInt(sourceId);
			writer.WriteShort(effect.TypeId);
			effect.Serialize(writer);
		}
		
		public virtual void Deserialize(IDataReader reader)
		{
			actionId = reader.ReadShort();
			if ( actionId < 0 )
			{
				throw new Exception("Forbidden value on actionId = " + actionId + ", it doesn't respect the following condition : actionId < 0");
			}
			sourceId = reader.ReadInt();
			effect = ProtocolTypeManager.GetInstance<Types.AbstractFightDispellableEffect>(reader.ReadShort());
			effect.Deserialize(reader);
		}
	}
}