using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Monsters
{
    [SpellCastHandler(SpellIdEnum.GLOURSOMPTUEUX)]
    public class GloursomptueuxCastHandler : DefaultSpellCastHandler
    {
        public GloursomptueuxCastHandler(SpellCastInformations cast)
            : base(cast)
        {
        }

        public override void Execute()
        {
            if (!m_initialized)
                Initialize();

            var fighter = Fight.GetFirstFighter<FightActor>(TargetedCell);

            if (fighter == null)
                return;

            Handlers[0].SetAffectedActors(new[] {fighter});

            base.Execute();
        }
    }
}
