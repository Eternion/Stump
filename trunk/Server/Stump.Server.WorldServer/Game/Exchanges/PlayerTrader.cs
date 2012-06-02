using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game.Actors.RolePlay;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Dialogs;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Handlers.Basic;

namespace Stump.Server.WorldServer.Game.Exchanges
{
    public class PlayerTrader : ITrader, IDialoger
    {
        public event ItemMovedHandler ItemMoved;

        private void NotifyItemMoved(PlayerItem item, bool modified, int difference)
        {
            ItemMovedHandler handler = ItemMoved;
            if (handler != null)
                handler(this, item, modified, difference);
        }

        public event KamasChangedHandler KamasChanged;

        private void NotifyKamasChanged(uint kamasAmount)
        {
            KamasChangedHandler handler = KamasChanged;
            if (handler != null)
                handler(this, kamasAmount);
        }

        public event ReadyStatusChangedHandler ReadyStatusChanged;

        private void NotifyReadyStatusChanged(bool isready)
        {
            ReadyStatusChangedHandler handler = ReadyStatusChanged;
            if (handler != null)
                handler(this, isready);
        }


        private List<PlayerItem> m_items = new List<PlayerItem>();

        public PlayerTrader(Character character, PlayerTrade trade)
        {
            Character = character;
            Trade = trade;
        }

        public IDialog Dialog
        {
            get { return Trade; }
        }

        ITrade ITrader.Trade
        {
            get { return Trade; }
        }

        public PlayerTrade Trade
        {
            get;
            private set;
        }

        public RolePlayActor Actor
        {
            get { return Character; }
        }

        public Character Character
        {
            get;
            private set;
        }

        public ReadOnlyCollection<PlayerItem> Items
        {
            get { return m_items.AsReadOnly(); }
        }

        public uint Kamas
        {
            get;
            private set;
        }

        public bool ReadyToApply
        {
            get;
            private set;
        }

        public void ToggleReady()
        {
            ToggleReady(!ReadyToApply);
        }

        public void ToggleReady(bool status)
        {
            if (status == ReadyToApply)
                return;

            ReadyToApply = status;

            NotifyReadyStatusChanged(ReadyToApply);
        }

        public bool MoveItem(int guid, int amount)
        {
            var playerItem = Character.Inventory[guid];
            var tradeItem = Items.SingleOrDefault(entry => entry.Guid == guid);

            ToggleReady(false);

            if (playerItem == null)
                return false;

            if (playerItem.IsLinked())
            {
                BasicHandler.SendTextInformationMessage(Character.Client, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 345, playerItem.Template.Id, playerItem.Guid);
                return false;
            }

            if (tradeItem != null)
            {
                if (playerItem.Stack < tradeItem.Stack + amount || tradeItem.Stack + amount < 0)
                    return false;

                var currentStack = tradeItem.Stack;
                tradeItem.Stack += amount;

                if (tradeItem.Stack <= 0)
                    m_items.Remove(tradeItem);

                NotifyItemMoved(tradeItem, true, tradeItem.Stack - currentStack);

                return true;
            }

            if (amount > playerItem.Stack || amount <= 0)
                return false;

            var dummyRecord = new PlayerItemRecord()
            {
                Id = playerItem.Record.Id,
                Template = playerItem.Template,
                OwnerId = Character.Id,
                Stack = amount,
                Effects = playerItem.Effects,
                New = false,
                Position = playerItem.Position,
            };

            tradeItem = new PlayerItem(Character, dummyRecord);
            m_items.Add(tradeItem);

            NotifyItemMoved(tradeItem, false, amount);

            return true;
        }

        public bool SetKamas(uint amount)
        {
            ToggleReady(false);

            if (amount > Character.Inventory.Kamas)
                return false;

            Kamas = amount;

            NotifyKamasChanged(Kamas);

            return true;
        }
    }
}