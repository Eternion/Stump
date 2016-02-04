using Stump.Server.WorldServer.Game.Spells;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stump.Server.WorldServer.Database.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Buffs
{
    public class EmptyBuff : Buff
    {
        public EmptyBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, bool dispelable)
            : base(id, target, caster, effect, spell, false, dispelable)
        {
        }

        public EmptyBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, bool dispelable, short customActionId)
            : base(id, target, caster, effect, spell, false, dispelable, customActionId)
        {
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            var values = Effect.GetValues();

            return new FightTriggeredEffect(Id, Target.Id, Duration, (sbyte)(Dispellable ? 0 : 1), (short)Spell.Id, Effect.Id, 0, (short)values[0], (short)values[1], (short)values[2], 0);
        }
    }
}