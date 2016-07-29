using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Inventory;
using MapPaddock = Stump.Server.WorldServer.Game.Maps.Paddocks.Paddock;

namespace Stump.Server.WorldServer.Game.Exchanges.Paddock
{
    public class PaddockExchange : IExchange
    {
        private readonly PaddockExchanger m_paddockExchanger;

        public PaddockExchange(Character character, MapPaddock paddock)
        {
            Character = character;
            Paddock = paddock;
            m_paddockExchanger = new PaddockExchanger(character, paddock, this);
        }

        public Character Character
        {
            get;
            private set;
        }

        public MapPaddock Paddock
        {
            get;
            private set;
        }

        public ExchangeTypeEnum ExchangeType => ExchangeTypeEnum.MOUNT_STABLE;

        public DialogTypeEnum DialogType => DialogTypeEnum.DIALOG_EXCHANGE;

        #region IDialog Members

        public void Open()
        {
            Character.SetDialoger(m_paddockExchanger);

            var stabledMounts = Character.StabledMounts.Where(x => x.Paddock == Paddock).ToList();
            var paddockedMounts = Paddock.IsPublicPaddock() ? Character.PublicPaddockedMounts.Where(x => x.Paddock == Paddock) : Paddock.PaddockedMounts;

            InventoryHandler.SendExchangeStartOkMountMessage(Character.Client, stabledMounts, paddockedMounts);
        }
        public void Close()
        {
            InventoryHandler.SendExchangeLeaveMessage(Character.Client, DialogType, false);
            Character.CloseDialog(this);
        }

        #endregion
    }
}
