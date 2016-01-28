using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Xelor
{
    [SpellCastHandler(SpellIdEnum.POUSSIÈRE_TEMPORELLE)]
    public class TemporalDustCastHandler : DefaultSpellCastHandler
    {
        public TemporalDustCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
        {
        }

        public override void Execute()
        {
            Handlers[5].SetAffectedActors(Handlers[5].GetAffectedActors(x => !x.NeedTelefragState));
            Handlers[6].SetAffectedActors(Handlers[6].GetAffectedActors(x => !x.NeedTelefragState));

            base.Execute();
        }
    }
}
