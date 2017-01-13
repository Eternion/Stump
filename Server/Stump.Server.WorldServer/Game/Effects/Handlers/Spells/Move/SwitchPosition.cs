using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells.Casts;
using Stump.Server.WorldServer.Game.Fights.Buffs;

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

            Caster.ExchangePositions(target);

            //Caster.TriggerBuffs(Caster, BuffTriggerType.OnMoved);
            target.TriggerBuffs(Caster, BuffTriggerType.OnMoved);

            return true;
        }
    }
}