using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Sadida
{
    [SpellCastHandler(6718)]
    public class SacrifiedDeadSpellCastHandler : DefaultSpellCastHandler
    {
        public SacrifiedDeadSpellCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
        {
            
        }

        public override bool Initialize()
        {
            if (base.Initialize() && CastedByEffect != null)
            {
                Handlers[0].SetAffectedActors(CastedByEffect.GetAffectedActors());
                return true;
            }

            return false;
        }
        
    }
}