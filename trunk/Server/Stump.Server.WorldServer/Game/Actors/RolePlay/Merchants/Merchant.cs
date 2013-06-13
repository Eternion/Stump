using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay
{
    public class Merchant : ContextActor
    {
        #region Network

        public virtual string Name
        {
            get;
            protected set;
        }

        public override GameContextActorInformations GetGameContextActorInformations()
        {
            return new GameRolePlayMerchantInformations(Id, Look, GetEntityDispositionInformations(), "", 0);
        }

        #endregion
    }
}