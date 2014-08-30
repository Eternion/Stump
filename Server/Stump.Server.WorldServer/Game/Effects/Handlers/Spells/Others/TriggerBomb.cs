using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Others
{
    [EffectHandler(EffectsEnum.Effect_TriggerBomb)]
    public class TriggerBomb : SpellEffectHandler
    {
        public TriggerBomb(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical) : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var bomb in GetAffectedActors(x => x is SummonedBomb && (x as SummonedBomb).Summoner == Caster))
            {
                if (bomb.IsAlive())
                {
                    (bomb as SummonedBomb).Explode();
                }
            }

            return true;
        }
    }
}