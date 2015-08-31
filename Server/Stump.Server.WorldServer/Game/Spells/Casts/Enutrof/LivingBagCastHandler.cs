using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Summon;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Enutrof
{
    [SpellCastHandler(SpellIdEnum.SAC_ANIMÉ)]
    public class LivingBagCastHandler : DefaultSpellCastHandler
    {
        public LivingBagCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
        {
        }

        public override void Execute()
        {
            if (!m_initialized)
                Initialize();

            var zoneEffect = Handlers[0];
            var summonEffect = Handlers[1] as Summon;

            if (zoneEffect == null || summonEffect == null)
                return;

            var affectedActors = zoneEffect.GetAffectedActors();
            summonEffect.Apply();

            var summonedBag = Caster.Summons.FirstOrDefault(x => x is SummonedMonster && ((SummonedMonster) x).Monster.MonsterId == summonEffect.Dice.DiceNum);

            if (summonedBag == null)
                return;

            foreach (var actor in affectedActors)
            {
                summonedBag.CastSpell(new Spell((int)SpellIdEnum.SACRIFICE_440, Spell.CurrentLevel), actor.Cell, true);
            }
        }
    }
}
