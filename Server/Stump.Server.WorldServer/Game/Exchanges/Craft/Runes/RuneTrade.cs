using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Mathematics;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Exchanges.Trades;
using Stump.Server.WorldServer.Game.Exchanges.Trades.Players;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Interactives.Skills;
using Stump.Server.WorldServer.Game.Jobs;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Exchanges.Craft
{
    public class RuneTrade : ITrade
    {
        public RuneTrade(Character character)
        {
            Trader = new RuneTrader(character, this);
        }

        public DialogTypeEnum DialogType => DialogTypeEnum.DIALOG_EXCHANGE;
        public ExchangeTypeEnum ExchangeType => ExchangeTypeEnum.RUNES_TRADE;

        public void Open()
        {
            InventoryHandler.SendExchangeStartOkRunesTradeMessage(Trader.Character.Client);
            Trader.Character.SetDialoger(Trader);

            Trader.ReadyStatusChanged += OnReadyStatusChanged;
            Trader.ItemMoved += OnItemMoved;
        }

        private void OnItemMoved(Trader trader, TradeItem item, bool modified, int difference)
        {
            if (!modified && item.Stack > 0)
                InventoryHandler.SendExchangeObjectAddedMessage(Character.Client, false, item);

            else if (item.Stack <= 0)
                InventoryHandler.SendExchangeObjectRemovedMessage(Character.Client, false, item.Guid);

            else
                InventoryHandler.SendExchangeObjectModifiedMessage(Character.Client, false, item);
        }

        private void OnReadyStatusChanged(Trader trader, bool isready)
        {
            if (isready)
            {
                Decraft();
            }
        }

        public void Decraft()
        {
            var results = new Dictionary<PlayerTradeItem, Dictionary<ItemTemplate, int>>();

            foreach(var item in Trader.Items.OfType<PlayerTradeItem>())
            {
                results.Add(item, new Dictionary<ItemTemplate, int>());

                foreach(var effect in item.Effects.OfType<EffectInteger>())
                {
                    var runes = RuneManager.Instance.GetEffectRunes(effect.EffectId);

                    if (runes.Length <= 0)
                        continue;
                    
                    var prop = effect.Value * Math.Min(2/3d, 1.5*item.Template.Level*item.Template.Level/Math.Pow(EffectManager.Instance.GetEffectBasePower(effect), 5/4d));

                    var random = new CryptoRandom();
                    prop *= random.NextDouble() * 0.2 + 0.9;

                    var amount = (int) Math.Floor(prop);
                    if (random.NextDouble() < prop - Math.Floor(prop))
                        amount++;

                    while(amount > 0)
                    {
                        var rune = runes.Last(x => x.Amount <= amount);

                        if (rune == null)
                            break;

                        var runeAmount = (int)Math.Floor((double)amount/rune.Amount);

                        if (results[item].ContainsKey(rune.Item))
                            results[item][rune.Item] += runeAmount;
                        else
                            results[item].Add(rune.Item, runeAmount);

                        amount -= runeAmount *rune.Amount;
                    }
                }
            }

            foreach(PlayerTradeItem item in results.Keys)
            {
                Character.Inventory.RemoveItem(item.PlayerItem);
                Trader.MoveItem(item.Guid, 0);
            }

            foreach(var group in results.Values.SelectMany(x => x).GroupBy(x => x.Key))
            {
                var rune = group.Key;
                var amount = group.Sum(x => x.Value);

                Character.Inventory.AddItem(rune, amount);
            }

            InventoryHandler.SendDecraftResultMessage(Character.Client,
                results.Select(x => new DecraftedItemStackInfo(x.Key.Guid, 0.5f, 0.5f, x.Value.Select(y => (short)y.Key.Id), x.Value.Select(y => y.Value))));
        }


        public void Close()
        {
            Character.ResetDialog();
            InventoryHandler.SendExchangeLeaveMessage(Character.Client, DialogType, false);
        }

        public Character Character => Trader.Character;

        public RuneTrader Trader
        {
            get;
        }

        public Trader FirstTrader => Trader;

        public Trader SecondTrader => Trader;


    }
}