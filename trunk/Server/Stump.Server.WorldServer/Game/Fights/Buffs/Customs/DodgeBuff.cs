using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Buffs.Customs
{
    public class DodgeBuff : Buff
    {
        public DodgeBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, bool critical, bool dispelable, int dodgePercent, int backCellsCount) : base(id, target, caster, effect, spell, critical, dispelable)
        {
            DodgePercent = dodgePercent;
            BackCellsCount = backCellsCount;
        }

        public DodgeBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, bool critical, bool dispelable, short customActionId, int dodgePercent, int backCellsCount) : base(id, target, caster, effect, spell, critical, dispelable, customActionId)
        {
            DodgePercent = dodgePercent;
            BackCellsCount = backCellsCount;
        }

        public int DodgePercent
        {
            get;
            set;
        }

        public int BackCellsCount
        {
            get;
            set;
        }

        public override void Apply()
        {

        }

        public override void Dispell()
        {

        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            var values = Effect.GetValues();

            return new FightTriggeredEffect(Id, Target.Id, Duration, (sbyte)( Dispelable ? 0 : 1 ), (short)Spell.Id, 0, (short)values[0], (short)values[1], (short)values[2], 0);
        }
    }
}