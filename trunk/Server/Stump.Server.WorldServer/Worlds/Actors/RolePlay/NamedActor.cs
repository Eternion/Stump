using Stump.DofusProtocol.Types;

namespace Stump.Server.WorldServer.Worlds.Actors.RolePlay
{
    public abstract class NamedActor : RolePlayActor
    {
        private string m_name;

        public virtual string Name
        {
            get { return m_name; }
            protected set
            {
                m_name = value;
                m_gameContextActorInformations.Invalidate();
            }
        }

        protected override GameContextActorInformations BuildGameContextActorInformations()
        {
            return new GameRolePlayNamedActorInformations(Id, Look, GetEntityDispositionInformations(), Name);
        }
    }
}