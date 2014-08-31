using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move;

namespace Stump.Server.WorldServer.Game.Spells.Casts
{
    [SpellCastHandler(SpellIdEnum.BOTTE)]
    public class KickSpellCastHandler : DefaultSpellCastHandler
    {
        public KickSpellCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical) : base(caster, spell, targetedCell, critical)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            foreach (var handler in Handlers)
            {
                if (handler is Push)
                    (handler as Push).DamagesDisabled = true;
            }
        }
    }
}