using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Ecaflip
{
    [SpellCastHandler(SpellIdEnum.DESTIN_D_ECAFLIP)]
    [SpellCastHandler(SpellIdEnum.DESTIN_D_ECAFLIP_DU_DOPEUL)]
    public class FateOfEcaflipCastHandler : DefaultSpellCastHandler
    {
        public FateOfEcaflipCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
        {
        }

        public override void Execute()
        {
            if (!m_initialized)
                Initialize();

            var pullHandler = Handlers[1];

            if (pullHandler == null)
                return;

            var affectedActors = pullHandler.GetAffectedActors().FirstOrDefault();

            if (affectedActors == null)
                return;

            pullHandler.Apply();

            TargetedCell = affectedActors.Cell;
            TargetedPoint = affectedActors.Position.Point;

            Initialize();

            foreach (var handler in Handlers)
            {
                handler.AddAffectedActor(affectedActors);
            }

            base.Execute();
        }
    }
}
