// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'FightTemporarySpellBoostEffect.xml' the '03/10/2011 12:47:10'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
	public class FightTemporarySpellBoostEffect : FightTemporaryBoostEffect
	{
		public const uint Id = 207;
		public override short TypeId
		{
			get
			{
				return 207;
			}
		}
		
		public short boostedSpellId;
		
		public FightTemporarySpellBoostEffect()
		{
		}
		
		public FightTemporarySpellBoostEffect(int uid, int targetId, short turnDuration, sbyte dispelable, short spellId, int parentBoostUid, short delta, short boostedSpellId)
			 : base(uid, targetId, turnDuration, dispelable, spellId, parentBoostUid, delta)
		{
			this.boostedSpellId = boostedSpellId;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			writer.WriteShort(boostedSpellId);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			boostedSpellId = reader.ReadShort();
			if ( boostedSpellId < 0 )
			{
				throw new Exception("Forbidden value on boostedSpellId = " + boostedSpellId + ", it doesn't respect the following condition : boostedSpellId < 0");
			}
		}
	}
}
