﻿using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move
{
    [EffectHandler(EffectsEnum.Effect_SwitchPosition)]
    [EffectHandler(EffectsEnum.Effect_SwitchPosition_1023)]
    public class SwitchPosition : SpellEffectHandler
    {
        public SwitchPosition(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical) : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            var target = GetAffectedActors().FirstOrDefault();

            if (target == null)
                return false;

            if (!target.CanSwitchPos())
                return false;

            if (target.IsCarrying())
                return false;

            if ((target is SummonedTurret) && !(Caster is SummonedTurret))
                return false;

            Caster.ExchangePositions(target);

            target.OnActorMoved(Caster, false);
            Caster.OnActorMoved(Caster, false);

            return true;
        }
    }
}