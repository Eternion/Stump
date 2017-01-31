using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Fights;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Sadida
{
    [SpellCastHandler(SpellIdEnum.FEUILLAGE)]
    public class FeuillageCastHandler : DefaultSpellCastHandler
    {
        public FeuillageCastHandler(SpellCastInformations cast)
            : base(cast)
        {
        }

        public override bool Initialize()
        {
            if(base.Initialize())
            {
                if (Spell.CurrentLevel == 1 && Caster.SummoningEffect.CastHandler.CastedByEffect != null) // summoned after death
                    Handlers[0].Delay = 2;

                return true;
            }

            return false;
        }
    }
}