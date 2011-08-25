using Stump.DofusProtocol.Types;

namespace Stump.Server.WorldServer.Worlds.Actors.RolePlay
{
    public abstract class NamedActor : RolePlayActor
    {
        #region Network

        public virtual string Name
        {
            get;
            protected set;
        }

        public override GameContextActorInformations GetGameContextActorInformations()
        {
            return new GameRolePlayNamedActorInformations(Id, Look, GetEntityDispositionInformations(), Name);
        }
        #endregion

        #region Actions

        #region Chat

        // todo

        #endregion
        
        #endregion
    }
}