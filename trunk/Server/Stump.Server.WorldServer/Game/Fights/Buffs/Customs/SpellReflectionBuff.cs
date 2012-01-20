using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Buffs.Customs
{
    public class SpellReflectionBuff : Buff
    {
        public SpellReflectionBuff(int id, FightActor target, FightActor caster, EffectDice effect, Spell spell, bool critical, bool dispelable)
            : base(id, target, caster, effect, spell, critical, dispelable)
        {
            Dice = effect;
        }

        public EffectDice Dice
        {
            get;
            private set;
        }

        public int ReflectedLevel
        {
            get { return Dice.DiceFace; }
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