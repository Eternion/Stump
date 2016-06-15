using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Mathematics;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Exchanges.Trades;
using Stump.Server.WorldServer.Game.Exchanges.Trades.Players;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Exchanges.Craft.Runes
{
    public class RuneTrade : ITrade
    {
        public RuneTrade(Character character)
        {
            Trader = new RuneTrader(character, this);
        }

        public DialogTypeEnum DialogType => DialogTypeEnum.DIALOG_EXCHANGE;
        public ExchangeTypeEnum ExchangeType => ExchangeTypeEnum.RUNES_TRADE;

        private bool m_decrafting;

        public void Open()
        {
            InventoryHandler.SendExchangeStartOkRunesTradeMessage(Trader.Character.Client);
            Trader.Character.SetDialoger(Trader);

            Trader.ReadyStatusChanged += OnReadyStatusChanged;
            Trader.ItemMoved += OnItemMoved;
        }

        private void OnItemMoved(Trader trader, TradeItem item, bool modified, int difference)
        {
            if (m_decrafting)
                return;

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

                trader.ToggleReady(false);
            }
        }

        public void Decraft()
        {
            m_decrafting = true;
            var results = new Dictionary<PlayerTradeItem, DecraftResult>();

            foreach(var item in Trader.Items.OfType<PlayerTradeItem>())
            {
                var result = new DecraftResult(item);
                results.Add(item, result);
                for (int i = 0; i < item.Stack; i++)
                {
                    RuneManager.Instance.RegisterDecraft(item.Template);
                    var coeff = RuneManager.Instance.GetDecraftCoefficient(item.Template);

                    foreach (var effect in item.Effects.OfType<EffectInteger>())
                    {
                        var runes = RuneManager.Instance.GetEffectRunes(effect.EffectId);

                        if (runes.Length <= 0)
                            continue;

                        var prop = coeff * effect.Value * Math.Min(2 / 3d, 1.5 * item.Template.Level * item.Template.Level / Math.Pow(EffectManager.Instance.GetEffectBasePower(effect), 5 / 4d));

                        var random = new CryptoRandom();
                        prop *= random.NextDouble() * 0.2 + 0.9;

                        var amount = (int)Math.Floor(prop);
                        if (random.NextDouble() < prop - Math.Floor(prop))
                            amount++;

                        var rune = runes.OrderBy(x => x.Amount).First();

                        var runeAmount = (int)Math.Floor((double)amount / rune.Amount);

                        if (result.Runes.ContainsKey(rune.Item))
                            result.Runes[rune.Item] += runeAmount;
                        else
                            result.Runes.Add(rune.Item, runeAmount);
                    }

                    if (!result.MinCoeff.HasValue || coeff < result.MinCoeff)
                        result.MinCoeff = coeff;
                    if (!result.MaxCoeff.HasValue || coeff > result.MaxCoeff)
                        result.MaxCoeff = coeff;
                }
            }
            InventoryHandler.SendDecraftResultMessage(Character.Client,
                results.Select(x => new DecraftedItemStackInfo(x.Key.Guid, (float)(x.Value.MinCoeff ?? 0.5), (float)(x.Value.MaxCoeff ?? 0.5), x.Value.Runes.Select(y => (short)y.Key.Id), x.Value.Runes.Select(y => y.Value))));

            foreach (PlayerTradeItem item in results.Keys)
            {
                Character.Inventory.RemoveItem(item.PlayerItem, (int)item.Stack);
                Trader.MoveItem(item.Guid, 0);
            }

            foreach(var group in results.Values.SelectMany(x => x.Runes).GroupBy(x => x.Key))
            {
                var rune = group.Key;
                var amount = group.Sum(x => x.Value);

                Character.Inventory.AddItem(rune, amount);
            }
            m_decrafting = false;
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