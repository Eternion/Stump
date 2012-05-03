using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Buffs.Customs
{
    public class DodgeBuff : Buff
    {
        public DodgeBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, bool critical, bool dispelable) : base(id, target, caster, effect, spell, critical, dispelable)
        {
        }

        public DodgeBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, bool critical, bool dispelable, short customActionId) : base(id, target, caster, effect, spell, critical, dispelable, customActionId)
        {
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
            throw new System.NotImplementedException();
        }

        public override void Dispell()
        {
            throw new System.NotImplementedException();
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            throw new System.NotImplementedException();
        }
    }
}