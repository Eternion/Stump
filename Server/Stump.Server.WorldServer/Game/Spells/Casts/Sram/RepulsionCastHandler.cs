using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move;
using Stump.Server.WorldServer.Game.Fights;

namespace Stump.Server.WorldServer.Game.Spells.Casts
{
    [SpellCastHandler(SpellIdEnum.RÉPULSION)]
    public class RepulsionCastHandler : DefaultSpellCastHandler
    {
        public RepulsionCastHandler(SpellCastInformations cast)
            : base(cast)
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