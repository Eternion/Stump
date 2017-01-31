using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Roublard
{
    [SpellCastHandler(SpellIdEnum.ROUBLABOT)]
    public class RoublabotCastHandler : DefaultSpellCastHandler
    {
        public RoublabotCastHandler(SpellCastInformations cast)
            : base(cast)
        {
        }

        public override void Execute()
        {
            base.Execute();

            var slave = Fight.GetOneFighter<SummonedFighter>(x => x.Cell == TargetedCell && x.Controller == Caster);

            if (slave == null)
                return;

            slave.CastAutoSpell(new Spell((int)SpellIdEnum.INITIALISATION, 1), TargetedCell);
        }
    }
}