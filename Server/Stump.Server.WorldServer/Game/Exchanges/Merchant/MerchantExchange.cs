using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Exchanges.Merchant
{
    public class MerchantExchange : IExchange
    {
        private readonly CharacterMerchant m_merchant;

        public MerchantExchange(Character character)
        {
            Character = character;
            m_merchant = new CharacterMerchant(character, this);
        }

        public Character Character
        {
            get;
        }

        public ExchangeTypeEnum ExchangeType
        {
            get { return ExchangeTypeEnum.DISCONNECTED_VENDOR; }
        }

        public DialogTypeEnum DialogType => DialogTypeEnum.DIALOG_EXCHANGE;

        #region IDialog Members

        public void Open()
        {
            Character.SetDialoger(m_merchant);

            InventoryHandler.SendExchangeStartedMessage(Character.Client, ExchangeType);
            InventoryHandler.SendExchangeShopStockStartedMessage(Character.Client, Character.MerchantBag);
        }

        public void Close()
        {
            InventoryHandler.SendExchangeLeaveMessage(Character.Client, DialogType, false);
            Character.CloseDialog(this);
        }

        #endregion
    }
}
