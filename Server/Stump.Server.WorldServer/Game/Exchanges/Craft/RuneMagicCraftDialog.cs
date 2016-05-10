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
using Stump.Server.WorldServer.Game.Items;
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

        private IEnumerable<EffectInteger> ItemEffects => ItemToImprove.Effects.OfType<EffectInteger>();

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
                var existantEffect = GetEffectToImprove(effect);
                var template = GetTemplateEffect(existantEffect);

                double criticalSuccess, neutralSuccess, criticalFailure;
                GetChances(existantEffect, template, out criticalSuccess, out neutralSuccess, out criticalFailure);

                var rand = new CryptoRandom();
                var randNumber = (int)(rand.NextDouble()*100);

                if (randNumber <= criticalSuccess)
                {
                    BoostEffect(effect);

                    yield return new Pair<CraftResultEnum, MagicPoolStatus>(CraftResultEnum.CRAFT_SUCCESS, MagicPoolStatus.UNMODIFIED);
                }
                else if (randNumber <= criticalSuccess + neutralSuccess)
                {
                    BoostEffect(effect);
                    int residual = DeBoostEffect(effect);

                    yield return new Pair<CraftResultEnum, MagicPoolStatus>(CraftResultEnum.CRAFT_SUCCESS, GetMagicPoolStatus(residual));
                }
                else
                {
                    int residual = DeBoostEffect(effect);
 
                    yield return new Pair<CraftResultEnum, MagicPoolStatus>(CraftResultEnum.CRAFT_FAILED, GetMagicPoolStatus(residual));
                }
                
            }


            Rune.Owner.Inventory.RemoveItem(Rune.PlayerItem, 1, true, false);
            Rune.Trader.MoveItem(Rune.Guid, -1);

        }
        private MagicPoolStatus GetMagicPoolStatus(int residual)
        {
            return residual == 0 ? MagicPoolStatus.UNMODIFIED : (residual > 0 ? MagicPoolStatus.INCREASED : MagicPoolStatus.DECREASED);
        }

        private void BoostEffect(EffectInteger runeEffect)
        {
            var effect = GetEffectToImprove(runeEffect);


            if (effect != null)
                effect.Value += runeEffect.Value;
            else
            {
                ItemToImprove.Effects.Add(new EffectInteger(runeEffect.EffectId, runeEffect.Value));
            }
        }

        private int DeBoostEffect(EffectInteger runeEffect)
        {
            var pwrToLose = (int)Math.Ceiling(EffectManager.Instance.GetEffectPower(runeEffect));
            short residual = 0;

            if (ItemToImprove.PlayerItem.PowerSink > 0)
            {
                residual = (short) -Math.Min(pwrToLose, ItemToImprove.PlayerItem.PowerSink);
                ItemToImprove.PlayerItem.PowerSink += residual;
                pwrToLose += residual;
            }

            if (pwrToLose == 0)
                return residual;

            while (pwrToLose > 0)
            {
                var effect = GetEffectToDown(runeEffect);
                var maxLost = (int)Math.Ceiling(EffectManager.Instance.GetEffectBasePower(runeEffect) / EffectManager.Instance.GetEffectBasePower(effect));

                var rand = new CryptoRandom();
                var lost = rand.Next(1, maxLost + 1);

                effect.Value -= (short)lost;
                pwrToLose -= (int)Math.Ceiling(lost*EffectManager.Instance.GetEffectBasePower(effect));
            }

            residual = (short)(pwrToLose < 0 ? -pwrToLose : 0);
            ItemToImprove.PlayerItem.PowerSink += residual;

            return residual;
        }

        private EffectInteger GetEffectToImprove(EffectInteger runeEffect)
        {
            return ItemEffects.FirstOrDefault(x => x.EffectId == runeEffect.EffectId);
        }

        private EffectInteger GetEffectToDown(EffectInteger runeEffect)
        {
            var effectToImprove = GetEffectToImprove(runeEffect);
            // recherche de jet exotique
            var exoticEffect = ItemEffects.Where(x => IsExotic(x) && x != effectToImprove).RandomElementOrDefault();

            if (exoticEffect != null)
                return exoticEffect;

            // recherche de jet overmax
            var overmaxEffect = ItemEffects.Where(x => IsOverMax(x) && x != effectToImprove).RandomElementOrDefault();

            if (overmaxEffect != null)
                return overmaxEffect;

            var rand = new CryptoRandom();
            foreach (var effect in ItemEffects.ShuffleLinq().Where(x => x != effectToImprove))
            {
                if (rand.NextDouble() <= EffectManager.Instance.GetEffectPower(runeEffect)/EffectManager.Instance.GetEffectBasePower(effect))
                    return effect;
            }

            return ItemEffects.FirstOrDefault(x => x != effectToImprove);
        }
        
        private bool IsExotic(EffectBase effect)
        {
            return ItemEffects.All(x => x.EffectId != effect.EffectId);
        }

        private bool IsOverMax(EffectInteger effect)
        {
            var template = GetTemplateEffect(effect);

            return effect.Value > template?.Max;
        }

        private EffectDice GetTemplateEffect(EffectBase effect)
        {
            return ItemToImprove.Template.Effects.OfType<EffectDice>().FirstOrDefault(x => x.EffectId == effect.EffectId);
        }

        private void GetChances(EffectInteger effectToImprove, EffectDice parentEffect, out double criticalSuccess, out double neutralSuccess, out double criticalFailure)
        {
            var minPwr = EffectManager.Instance.GetItemMinPower(ItemToImprove);
            var maxPwr = EffectManager.Instance.GetItemMaxPower(ItemToImprove);
            var pwr = EffectManager.Instance.GetItemPower(ItemToImprove);
            var itemStatus = (pwr - minPwr)/(maxPwr - minPwr)*100d;

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
                effectStatus = ((double)effectToImprove.Value - parentEffect.Min)/(parentEffect.Max - parentEffect.Min)*100d;
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
            criticalSuccess = Math.Max(1, 100 - Math.Ceiling(effectSuccess + itemSuccess + levelSuccess));

            if (criticalSuccess > 50)
                neutralSuccess = 100 - criticalSuccess;
            else if (criticalSuccess < 25)
                neutralSuccess = 25 + criticalSuccess;

            criticalFailure = 100 - (criticalSuccess + neutralSuccess);
        }
    }
}