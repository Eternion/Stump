using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Damage
{
    [EffectHandler(EffectsEnum.Effect_Kill)]
    public class Kill : SpellEffectHandler
    {
        public Kill(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override int Priority => Spell.Id == (int)SpellIdEnum.FIN_DES_TEMPS ? 0 : int.MaxValue;

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                actor.Stats.Health.DamageTaken = actor.LifePoints;
                actor.CheckDead(Caster);
            }

            return true;
        }
    }
}