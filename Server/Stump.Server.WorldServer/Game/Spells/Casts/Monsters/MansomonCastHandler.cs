using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Fights;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Monsters
{
    [SpellCastHandler(SpellIdEnum.MANSOMON)]
    public class MansomonCastHandler : DefaultSpellCastHandler
    {
        public MansomonCastHandler(SpellCastInformations cast)
            : base(cast)
        {
        }

        public override bool Initialize()
        {
            var init = base.Initialize();

            if (init)
                foreach(var handler in Handlers)
                    handler.DefaultDispellableStatus = FightDispellableEnum.REALLY_NOT_DISPELLABLE;

            return init;
        }
    }
}