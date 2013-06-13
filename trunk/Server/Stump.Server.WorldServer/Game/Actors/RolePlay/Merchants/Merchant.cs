using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay
{
    public class Merchant : RolePlayActor
    {
        public Merchant(string name, int sType)
        {
            Name = name;
            sellType = sType;
        }

        #region Network

        public virtual string Name
        {
            get;
            protected set;
        }

        public virtual int sellType
        {
            get;
            protected set;
        }

        public override GameContextActorInformations GetGameContextActorInformations()
        {
            return new GameRolePlayMerchantInformations(Id, Look, GetEntityDispositionInformations(), Name, sellType);
        }

        #endregion
    }
}