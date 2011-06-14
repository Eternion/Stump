using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.Server.WorldServer.Global;

namespace Stump.Server.WorldServer.Entities.Actors
{
    public abstract class NamedActor : Actor
    {
        protected NamedActor(long id, EntityLook look, ObjectPosition position, string name)
            : base(id, look, position)
        {
            Name = name;
        }

        public string Name
        {
            get;
            protected set;
        }

        public override GameContextActorInformations GetActorInformations()
        {
            return new GameRolePlayNamedActorInformations((int)Id, Look.EntityLook, GetDispositionInformations(), Name);
        }
    }
}