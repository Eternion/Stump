using Stump.DofusProtocol.Types;

namespace Stump.Server.WorldServer.Worlds.Actors.RolePlay
{
    public abstract class RolePlayActor : ContextActor
    {
        protected override GameContextActorInformations BuildGameContextActorInformations()
        {
            return new GameRolePlayActorInformations(Id, Look, GetEntityDispositionInformations());
        }
    }
}