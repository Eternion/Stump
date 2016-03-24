using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Others;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Sadida
{
    [SpellCastHandler(SpellIdEnum.FEUILLAGE)]
    public class FeuillageCastHandler : DefaultSpellCastHandler
    {
        public FeuillageCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
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