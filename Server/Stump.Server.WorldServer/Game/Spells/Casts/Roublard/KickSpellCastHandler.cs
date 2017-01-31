using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Fights;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Roublard
{
    [SpellCastHandler(SpellIdEnum.BOTTE)]
    public class KickSpellCastHandler : DefaultSpellCastHandler
    {
        public KickSpellCastHandler(SpellCastInformations cast)
            : base(cast)
        {
        }

        public override bool Initialize()
        {
            CheckWhenExecute = true;

            base.Initialize();

            Handlers = Handlers.OrderByDescending(entry => entry.EffectZone.MinRadius).ToArray();

            return true;
        }
    }
}