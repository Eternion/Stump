using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Others
{
    [EffectHandler(EffectsEnum.Effect_CastSpell_1160)]
    public class CastSpell : SpellEffectHandler
    {
        public CastSpell(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var affectedActor in GetAffectedActors())
            {
                affectedActor.CastSpell(new Spell(Dice.DiceNum, (byte)Dice.DiceFace), affectedActor.Cell, true, true);
            }

            return true;
        }
    }
}
