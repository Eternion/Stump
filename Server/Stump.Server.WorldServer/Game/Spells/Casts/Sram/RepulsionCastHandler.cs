using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move;

namespace Stump.Server.WorldServer.Game.Spells.Casts
{
    [SpellCastHandler(SpellIdEnum.RÉPULSION)]
    public class RepulsionCastHandler : DefaultSpellCastHandler
    {
        public RepulsionCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
        {
        }


        public override bool Initialize()
        {
            if (base.Initialize() && TriggerCell != null)
            {
                var push = Handlers.OfType<Push>().First();

                push.PushDirection = CastPoint.OrientationTo(TriggerCell);
                return true;
            }

            return false;
        }
    }
}