using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.Server.WorldServer.Entities.Actors;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.Entities.RolePlay
{
    public  abstract partial class Actor : ContextActor
    {
        protected Actor(long id, EntityLook look, ObjectPosition position)
            : base(id, look, position)
        {
        }



        public override IContext Context
        {
            get { return Map; }
        }

        public override GameContextActorInformations GetActorInformations()
        {
            return new GameRolePlayActorInformations((int)Id, Look, GetDispositionInformations());
        }
    }
}