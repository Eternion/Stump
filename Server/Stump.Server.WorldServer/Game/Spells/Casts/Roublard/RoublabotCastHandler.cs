using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Roublard
{
    [SpellCastHandler(SpellIdEnum.ROUBLABOT)]
    public class RoublabotCastHandler : DefaultSpellCastHandler
    {
        public RoublabotCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
        {
        }

        public override void Execute()
        {
            base.Execute();

            var slave = Fight.GetOneFighter<SummonedFighter>(x => x.Cell == TargetedCell && x.Controller == Caster);

            if (slave == null)
                return;

            slave.CastSpell(new Spell((int)SpellIdEnum.INITIALISATION, 1), TargetedCell, true, true, true, ignored: new[] { SpellCastResult.OK });
        }
    }
}