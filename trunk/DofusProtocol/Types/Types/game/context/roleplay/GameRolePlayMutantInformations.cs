// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GameRolePlayMutantInformations.xml' the '03/10/2011 12:47:11'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
	public class GameRolePlayMutantInformations : GameRolePlayHumanoidInformations
	{
		public const uint Id = 3;
		public override short TypeId
		{
			get
			{
				return 3;
			}
		}
		
		public int monsterId;
		public sbyte powerLevel;
		
		public GameRolePlayMutantInformations()
		{
		}
		
		public GameRolePlayMutantInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, string name, Types.HumanInformations humanoidInfo, int monsterId, sbyte powerLevel)
			 : base(contextualId, look, disposition, name, humanoidInfo)
		{
			this.monsterId = monsterId;
			this.powerLevel = powerLevel;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			writer.WriteInt(monsterId);
			writer.WriteSByte(powerLevel);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			monsterId = reader.ReadInt();
			powerLevel = reader.ReadSByte();
		}
	}
}
