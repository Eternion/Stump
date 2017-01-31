using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Monsters
{
    [SpellCastHandler(SpellIdEnum.PARADE_DES_VIEUX_JOUETS)]
    public class OldToyParadeCastHandler : DefaultSpellCastHandler
    {
        public OldToyParadeCastHandler(SpellCastInformations cast)
            : base(cast)
        {
        }

        public override void Execute()
        {
            if (!m_initialized)
                Initialize();

            var fighter = Fight.GetFirstFighter<SummonedMonster>(TargetedCell);

            if (fighter == null)
                return;

            if (fighter.Monster.MonsterId != 494)
                return;

            //Handlers[0].Apply();

            base.Execute();
        }
    }
}
