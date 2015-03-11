using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Monsters
{
    [SpellCastHandler(SpellIdEnum.BEUVERIE)]
    public class BeuverieCastHandler : DefaultSpellCastHandler
    {
        public BeuverieCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
        {
        }

        public override void Execute()
        {
            if (!m_initialized)
                Initialize();

            var fighters = Handlers[1].GetAffectedActors(x => x.HasState((int) SpellStatesEnum.Drunk));
            Handlers[1].SetAffectedActors(fighters);

            base.Execute();
        }
    }
}
