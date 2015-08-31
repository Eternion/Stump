using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Debuffs
{
    [EffectHandler(EffectsEnum.Effect_SubVitalityPercent)]
    [EffectHandler(EffectsEnum.Effect_SubVitalityPercent_1048)]
    public class SubVitalityPercent : SpellEffectHandler
    {
        public SubVitalityPercent(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var integerEffect = GenerateEffect();

                if (integerEffect == null)
                    return false;

                var bonus = (int)(actor.Stats.Health.TotalSafe * (integerEffect.Value / 100d));

                AddStatBuff(actor, (short)-bonus, PlayerFields.Health, true,
                    Effect.Id);

                if (Effect.EffectId == EffectsEnum.Effect_SubVitalityPercent)
                    ActionsHandler.SendGameActionFightLifePointsLostMessage(Fight.Clients, actor, actor, (short)bonus, 0);
            }

            return true;
        }
    }
}
