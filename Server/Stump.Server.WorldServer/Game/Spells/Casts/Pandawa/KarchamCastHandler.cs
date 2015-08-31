using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Pandawa
{
    [SpellCastHandler(SpellIdEnum.KARCHAM)]
    public class KarchamCastHandler : DefaultSpellCastHandler
    {
        public KarchamCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
        {
        }

        public override bool Initialize()
        {
            base.Initialize();

            var targetActor = Fight.GetOneFighter(TargetedCell);

            if (targetActor != null)
                TargetedActor = targetActor;

            TargetedCell = Caster.Cell;

            return true;
        }

        public override void Execute()
        {
            if (!m_initialized)
                Initialize();

            if (Caster.IsCarrying())
                return;

            Handlers[0].Apply();

            var carriedActor = Caster.GetCarriedActor();
            if (carriedActor == null)
                return;

            if (carriedActor.IsFriendlyWith(Caster))
                Handlers[2].AddAffectedActor(carriedActor);

            Handlers[2].Apply();
        }
    }
}
