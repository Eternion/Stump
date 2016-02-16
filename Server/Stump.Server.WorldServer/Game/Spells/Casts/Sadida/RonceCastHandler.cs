using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Sadida
{
    [SpellCastHandler(SpellIdEnum.RONCE)]
    public class RonceCastHandler : DefaultSpellCastHandler
    {
        public RonceCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
        {
        }
        
        public override void Execute()
        {
            base.Execute();

            var target = Fight.GetOneFighter(TargetedCell);
            var tree = target as SummonedMonster;

            if (tree == null || tree.Monster.Template.Id != (int)MonsterIdEnum.ARBRE_282 || tree.Summoner != Caster)
                return;

            var bloqueuse = Caster.Summons.OfType<SummonedMonster>().FirstOrDefault(x => x.Monster.Template.Id == (int)MonsterIdEnum.LA_BLOQUEUSE_115);

            bloqueuse?.CastSpell(new Spell((int)SpellIdEnum.SUBSTITUTION_POUPESQUE, 1), tree.Cell, true, true);

        }
    }
}