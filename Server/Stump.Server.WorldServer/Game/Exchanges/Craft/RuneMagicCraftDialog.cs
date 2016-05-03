using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Stump.Core.Collections;
using Stump.Core.Extensions;
using Stump.Core.Mathematics;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Exchanges.Trades;
using Stump.Server.WorldServer.Game.Exchanges.Trades.Players;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Interactives.Skills;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Jobs;

namespace Stump.Server.WorldServer.Game.Exchanges.Craft
{
    public abstract class RuneMagicCraftDialog : BaseCraftDialog
    {
        public RuneMagicCraftDialog(InteractiveObject interactive, Skill skill, Job job)
            : base(interactive, skill, job)
        {
        }

        public PlayerTradeItem ItemToImprove
        {
            get;
            private set;
        }

        public PlayerTradeItem Rune
        {
            get;
            private set;
        }

        public PlayerTradeItem SignatureRune
        {
            get;
            private set;
        } 

        public virtual void Open()
        {
            FirstTrader.ItemMoved += OnItemMoved;
            SecondTrader.ItemMoved += OnItemMoved;
        }

        private void OnItemMoved(Trader trader, TradeItem item, bool modified, int difference)
        {
            var playerItem = item as PlayerTradeItem;

            if (playerItem == null)
                return;

            if (item.Template.Type.ItemType == ItemTypeEnum.RUNE_DE_FORGEMAGIE)
                Rune = playerItem.Stack > 0 ? playerItem : null;
            else if (Skill.SkillTemplate.ModifiableItemTypes.Contains((int) item.Template.TypeId))
                ItemToImprove = playerItem.Stack > 0 ? playerItem : null;
        }

        public override bool CanMoveItem(BasePlayerItem item)
        {
            return item.Template.TypeId == (int)ItemTypeEnum.RUNE_DE_FORGEMAGIE || Skill.SkillTemplate.ModifiableItemTypes.Contains((int)item.Template.TypeId);
        }

        public IEnumerable<Pair<CraftResultEnum, MagicPoolStatus>> ApplyRune()
        {
            foreach (var effect in Rune.Effects.OfType<EffectInteger>())
            {
                var existantEffect = ItemToImprove.Effects.OfType<EffectInteger>().FirstOrDefault(x => x.EffectId == effect.EffectId);
                var template = ItemToImprove.Template.Effects.OfType<EffectDice>().FirstOrDefault(x => x.EffectId == effect.EffectId);

                double criticalSuccess, neutralSuccess, criticalFailure;
                GetChances(existantEffect, template, out criticalSuccess, out neutralSuccess, out criticalFailure);

                var rand = new CryptoRandom();
                var randNumber = rand.NextDouble();

                if (randNumber <= criticalSuccess)
                {
                    if (existantEffect != null)
                        existantEffect.Value += effect.Value;

                    else
                    {
                        ItemToImprove.Effects.Add(new EffectInteger(effect.EffectId, effect.Value));
                    }


                    yield return new Pair<CraftResultEnum, MagicPoolStatus>(CraftResultEnum.CRAFT_SUCCESS, MagicPoolStatus.UNMODIFIED);
                }
                else if (randNumber <= criticalSuccess + neutralSuccess)
                {
                    var effectToDown = GetEffectToDown(existantEffect);

                    if (existantEffect != null)
                        existantEffect.Value += effect.Value;

                    else
                    {
                        ItemToImprove.Effects.Add(new EffectInteger(effect.EffectId, effect.Value));
                    }

                    effectToDown.Value -= (short)(effect.Value*Math.Ceiling(EffectManager.Instance.GetEffectPower((EffectBase) effect)/EffectManager.Instance.GetEffectPower((EffectBase) effectToDown)));

                    yield return new Pair<CraftResultEnum, MagicPoolStatus>(CraftResultEnum.CRAFT_SUCCESS, MagicPoolStatus.UNMODIFIED);
                }
                else
                {
                    var effectToDown = GetEffectToDown(existantEffect);
                    

                    effectToDown.Value -= (short)(effect.Value * Math.Ceiling(EffectManager.Instance.GetEffectPower((EffectBase)effect) / EffectManager.Instance.GetEffectPower((EffectBase)effectToDown)));

                    yield return new Pair<CraftResultEnum, MagicPoolStatus>(CraftResultEnum.CRAFT_FAILED, MagicPoolStatus.UNMODIFIED);
                }
                
            }


            Rune.Owner.Inventory.RemoveItem(Rune.PlayerItem, 1, true, false);
            Rune.Trader.MoveItem(Rune.Guid, -1);

        }

        private EffectInteger GetEffectToDown(EffectInteger effectToImprove)
        {
            return ItemToImprove.Effects.OfType<EffectInteger>().Where(x => x != effectToImprove && EffectManager.Instance.GetEffectPower(x) != 0).RandomElementOrDefault();
        }

        private void GetChances(EffectInteger effectToImprove, EffectDice parentEffect, out double criticalSuccess, out double neutralSuccess, out double criticalFailure)
        {
            var minPwr = EffectManager.Instance.GetItemMinPower(ItemToImprove);
            var maxPwr = EffectManager.Instance.GetItemMaxPower(ItemToImprove);
            var pwr = EffectManager.Instance.GetItemPower(ItemToImprove);
            var itemStatus = pwr - minPwr/(maxPwr - minPwr)*100d;

            double effectStatus;
            double diceFactor;
            double itemFactor;
            double levelSuccess;
            double effectSuccess;
            double itemSuccess;

            if (parentEffect == null)
            {
                effectStatus = 100;
                diceFactor = 40;
                itemFactor = 54;
                levelSuccess = 5;
            }
            else
            {
                diceFactor = 30;
                itemFactor = 50;
                levelSuccess = 5;
                effectStatus = effectToImprove.Value - parentEffect.Min/(parentEffect.Max - parentEffect.Min)*100;
            }

            if (effectStatus >= 80)
                effectSuccess = diceFactor*effectStatus/100;
            else
                effectSuccess = effectStatus/4;

            if (itemStatus >= 50)
                itemSuccess = itemFactor*itemStatus/100;
            else
                itemSuccess = itemStatus;

            neutralSuccess = 50d;
            criticalSuccess = 100 - Math.Ceiling(effectSuccess + itemSuccess + levelSuccess);

            if (criticalSuccess > 50)
                neutralSuccess = 100 - criticalSuccess;
            else if (criticalSuccess < 25)
                neutralSuccess = 25 + criticalSuccess;

            criticalFailure = 100 - (criticalSuccess + neutralSuccess);
        }
    }
}