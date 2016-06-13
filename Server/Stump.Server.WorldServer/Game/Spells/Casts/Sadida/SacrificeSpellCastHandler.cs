using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Sadida
{
    [SpellCastHandler(SpellIdEnum.SACRIFICE_440)]
    public class SacrificeSpellCastHandler : DefaultSpellCastHandler
    {
        public SacrificeSpellCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
        {
        }

        public override bool Initialize()
        {
            if (base.Initialize())
            {
                Handlers[1].Priority = 0;
                return true;
            }

            return false;
        }
    }
}