using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Sadida
{
    [SpellCastHandler(6718)]
    public class SacrifiedDeadSpellCastHandler : DefaultSpellCastHandler
    {
        public SacrifiedDeadSpellCastHandler(SpellCastInformations cast)
            : base(cast)
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