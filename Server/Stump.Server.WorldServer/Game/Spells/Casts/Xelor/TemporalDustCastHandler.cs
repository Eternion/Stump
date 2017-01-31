using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Fights;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Xelor
{
    [SpellCastHandler(SpellIdEnum.POUSSIÈRE_TEMPORELLE)]
    public class TemporalDustCastHandler : DefaultSpellCastHandler
    {
        public TemporalDustCastHandler(SpellCastInformations cast)
            : base(cast)
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
