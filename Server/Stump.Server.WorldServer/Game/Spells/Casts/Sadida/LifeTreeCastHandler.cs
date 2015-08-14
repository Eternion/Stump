using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Summon;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Sadida
{
    [SpellCastHandler(SpellIdEnum.ARBRE_DE_VIE)]
    public class LifeTreeCastHandler : DefaultSpellCastHandler
    {
        public LifeTreeCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
        {
        }

        public override void Execute()
        {
            if (!m_initialized)
                Initialize();

            var summonEffect = Handlers[0] as Summon;

            if (summonEffect == null)
                return;

            summonEffect.Apply();

            var summonedTree = Caster.Summons.FirstOrDefault(x => x is SummonedMonster && ((SummonedMonster)x).Monster.MonsterId == summonEffect.Dice.DiceNum);

            if (summonedTree == null)
                return;

            summonedTree.CastSpell(new Spell((int)SpellIdEnum.SOIN_SYLVESTRE, Spell.CurrentLevel), summonedTree.Cell, true);
        }
    }
}
