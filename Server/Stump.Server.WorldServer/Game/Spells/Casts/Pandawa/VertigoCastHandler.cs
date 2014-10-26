using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Pandawa
{
    [SpellCastHandler(SpellIdEnum.VERTIGE)]
    public class VertigoCastHandler : DefaultSpellCastHandler
    {
        public VertigoCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
        {
        }

        public override void Execute()
        {
            if (!m_initialized)
                Initialize();

            if (!Caster.IsCarrying())
                return;

            var carriedActor = Caster.GetCarriedActor();
            if (carriedActor == null)
                return;

            Handlers[0].AddAffectedActor(carriedActor);
            Handlers[0].Apply();
        }
    }
}
