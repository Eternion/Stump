using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Zobal
{
    [SpellCastHandler(SpellIdEnum.REUCHE)]
    [SpellCastHandler(SpellIdEnum.REUCHE_DU_DOPEUL)]
    public class ReucheCastHandler : DefaultSpellCastHandler
    {
        public ReucheCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
        {
        }

        public override void Execute()
        {
            if (!m_initialized)
                Initialize();

            Handlers[0].AddAffectedActor(Caster);

            base.Execute();
        }
    }
}
