using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Extensions;
using Stump.Core.Mathematics;
using Stump.Core.Timers;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Exchanges.Trades;
using Stump.Server.WorldServer.Game.Exchanges.Trades.Players;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Interactives.Skills;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Jobs;

namespace Stump.Server.WorldServer.Game.Exchanges.Craft.Runes
{
    public abstract class RuneMagicCraftDialog : BaseCraftDialog
    {
        public const int MAX_STAT_POWER = 100;
        public const int AUTOCRAFT_INTERVAL = 1000;

        private TimedTimerEntry m_autoCraftTimer;

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

        public override void Close()
        {
            StopAutoCraft();
        }

        public void StopAutoCraft(ExchangeReplayStopReasonEnum reason = ExchangeReplayStopReasonEnum.STOPPED_REASON_USER)
        {
            if (m_autoCraftTimer != null)
            {
                m_autoCraftTimer.Stop();
                m_autoCraftTimer = null;

                OnAutoCraftStopped(reason);
                ChangeAmount(1);
            }
        }

        protected virtual void OnAutoCraftStopped(ExchangeReplayStopReasonEnum reason)
        {

        }

        protected virtual void OnItemMoved(Trader trader, TradeItem item, bool modified, int difference)
        {
            var playerItem = item as PlayerTradeItem;

            if (playerItem == null)
                return;

            if (item.Template.Type.ItemType == ItemTypeEnum.RUNE_DE_FORGEMAGIE && (playerItem != Rune || playerItem.Stack == 0))
            {
                Rune = playerItem.Stack > 0 ? playerItem : null;
            }
            else if (Skill.SkillTemplate.ModifiableItemTypes.Contains((int) item.Template.TypeId) && (playerItem != ItemToImprove || playerItem.Stack == 0))
            {
                ItemToImprove = playerItem.Stack > 0 ? playerItem : null;
            }
        }

        public override bool CanMoveItem(BasePlayerItem item)
        {
            return item.Template.TypeId == (int)ItemTypeEnum.RUNE_DE_FORGEMAGIE || Skill.SkillTemplate.ModifiableItemTypes.Contains((int)item.Template.TypeId);
        }

        protected virtual void OnRuneApplied(CraftResultEnum result, MagicPoolStatus poolStatus)
        {
        }

        public void ApplyAllRunes()
        {
            if (m_autoCraftTimer != null)
                StopAutoCraft();

            if (Amount == 1 || Amount == 0)
                ApplyRune();
            else
                AutoCraft();
        }

        private void AutoCraft()
        {
            ApplyRune();
            if (ItemToImprove != null && Rune != null && Amount == -1)
                m_autoCraftTimer = Crafter.Character.Area.CallDelayed(AUTOCRAFT_INTERVAL, AutoCraft);
            else
                StopAutoCraft(ExchangeReplayStopReasonEnum.STOPPED_REASON_OK);
        }

        public void ApplyRune()
        {
            if (ItemToImprove == null || Rune == null)
                return;

            foreach (var effect in Rune.Effects.OfType<EffectInteger>())
            {
                var existantEffect = GetEffectToImprove(effect);

                double criticalSuccess, neutralSuccess, criticalFailure;
                GetChances(existantEffect, effect, out criticalSuccess, out neutralSuccess, out criticalFailure);

                var rand = new CryptoRandom();
                var randNumber = (int)(rand.NextDouble()*100);

                if (randNumber <= criticalSuccess)
                {
                    BoostEffect(effect);

                    OnRuneApplied(CraftResultEnum.CRAFT_SUCCESS, MagicPoolStatus.UNMODIFIED);
                }
                else if (randNumber <= criticalSuccess + neutralSuccess)
                {
                    BoostEffect(effect);
                    int residual = DeBoostEffect(effect);

                    OnRuneApplied(CraftResultEnum.CRAFT_SUCCESS, GetMagicPoolStatus(residual));
                }
                else
                {
                    int residual = DeBoostEffect(effect);

                    OnRuneApplied(CraftResultEnum.CRAFT_FAILED, GetMagicPoolStatus(residual));
                }
                
            }

            var rune = Rune;
            Crafter.MoveItem(rune.Guid, -1);
            rune.Owner.Inventory.RemoveItem(rune.PlayerItem, 1, true, false);
            ItemToImprove.PlayerItem.Invalidate();

        }
        private MagicPoolStatus GetMagicPoolStatus(int residual)
        {
            return residual == 0 ? MagicPoolStatus.UNMODIFIED : (residual > 0 ? MagicPoolStatus.INCREASED : MagicPoolStatus.DECREASED);
        }

        private void BoostEffect(EffectInteger runeEffect)
        {
            var effect = GetEffectToImprove(runeEffect);


            if (effect != null)
            {
                effect.Value += (short) ((effect.Template.BonusType == -1 ? -1 : 1)*runeEffect.Value);

                if (effect.Value == 0)
                    ItemToImprove.Effects.Remove(effect);
                else if (effect.Value > 0 && effect.Value <= runeEffect.Value && effect.Template.OppositeId > 0) // from negativ to positiv
                {
                    ItemToImprove.Effects.Remove(effect);
                    ItemToImprove.Effects.Add(new EffectInteger((EffectsEnum)effect.Template.OppositeId, effect.Value));
                }
            }
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

                if (effect == null)
                    break;

                var maxLost = (int)Math.Ceiling(EffectManager.Instance.GetEffectBasePower(runeEffect) / Math.Abs(EffectManager.Instance.GetEffectBasePower(effect)));

                var rand = new CryptoRandom();
                var lost = rand.Next(1, maxLost + 1);

                var oldValue = effect.Value;

                effect.Value -= (short)((effect.Template.BonusType == -1 ? -1 : 1) * lost);
                pwrToLose -= (int)Math.Ceiling(lost*Math.Abs(EffectManager.Instance.GetEffectBasePower(effect)));

                if (effect.Value == 0 || (effect.Value < 0 && oldValue > 0))
                    ItemToImprove.Effects.Remove(effect);
                else if (effect.Value < 0 && effect.Value >= -lost && effect.Template.OppositeId > 0) // from positiv to negativ stat
                {
                    ItemToImprove.Effects.Remove(effect);
                    ItemToImprove.Effects.Add(new EffectInteger((EffectsEnum) effect.Template.OppositeId, (short) -effect.Value));
                }

            }

            residual = (short)(pwrToLose < 0 ? -pwrToLose : 0);
            ItemToImprove.PlayerItem.PowerSink += residual;

            return residual;
        }

        private EffectInteger GetEffectToImprove(EffectInteger runeEffect)
        {
            return ItemEffects.FirstOrDefault(x => x.EffectId == runeEffect.EffectId || (x.Template.OppositeId != 0 && x.Template.OppositeId == runeEffect.Id));
        }

        private EffectInteger GetEffectToDown(EffectInteger runeEffect)
        {
            var effectToImprove = GetEffectToImprove(runeEffect);
            // recherche de jet exotique
            var exoticEffect = ItemEffects.Where(x => IsExotic(x) && x != effectToImprove).RandomElementOrDefault();

            if (exoticEffect != null)
                return exoticEffect;

            // recherche de jet overmax
            var overmaxEffect = ItemEffects.Where(x => IsOverMax(x, runeEffect) && x != effectToImprove).RandomElementOrDefault();

            if (overmaxEffect != null)
                return overmaxEffect;

            var rand = new CryptoRandom();
            foreach (var effect in ItemEffects.ShuffleLinq().Where(x => x != effectToImprove))
            {
                if (EffectManager.Instance.GetEffectPower(effect) - EffectManager.Instance.GetEffectPower(runeEffect) < MAX_STAT_POWER)
                    continue;

                if (rand.NextDouble() <= EffectManager.Instance.GetEffectPower(runeEffect)/Math.Abs(EffectManager.Instance.GetEffectBasePower(effect)))
                    return effect;
            }

            return ItemEffects.FirstOrDefault(x => x != effectToImprove);
        }
        
        private bool IsExotic(EffectBase effect)
        {
            return ItemToImprove.Template.Effects.All(x => x.EffectId != effect.EffectId);
        } 

        private double GetExoticPower()
        {
            return ItemToImprove.Effects.Where(IsExotic).OfType<EffectInteger>().Sum(x => EffectManager.Instance.GetEffectPower(x));
        }

        private bool IsOverMax(EffectInteger effect)
        {
            var template = GetTemplateEffect(effect);

            return effect.Template.BonusType > -1 && effect.Value > template?.Max;
        }

        private bool IsOverMax(EffectInteger effect, EffectInteger runeEffect)
        {
            var template = GetTemplateEffect(effect);

            return effect.Template.BonusType > -1 && effect.Value + runeEffect.Value > template?.Max;
        }

        private EffectDice GetTemplateEffect(EffectBase effect)
        {
            return ItemToImprove.Template.Effects.OfType<EffectDice>().FirstOrDefault(x => x.EffectId == effect.EffectId || (x.Template.OppositeId > 0 && x.Template.OppositeId == (int) effect.EffectId));
        }

        private void GetChances(EffectInteger effectToImprove, EffectInteger runeEffect, out double criticalSuccess, out double neutralSuccess, out double criticalFailure)
        {
            var minPwr = EffectManager.Instance.GetItemMinPower(ItemToImprove);
            var maxPwr = EffectManager.Instance.GetItemMaxPower(ItemToImprove);
            var pwr = EffectManager.Instance.GetItemPower(ItemToImprove);

            double itemStatus = Math.Max(0, GetProgress(pwr, maxPwr, minPwr)*100);
            var parentEffect = GetTemplateEffect(runeEffect);

            if (effectToImprove != null && 
                (EffectManager.Instance.GetEffectPower(effectToImprove) + EffectManager.Instance.GetEffectPower(runeEffect) > MAX_STAT_POWER ||
                GetExoticPower() > MAX_STAT_POWER))

            {
                neutralSuccess = 0;
                criticalSuccess = 0;
                criticalFailure = 100;
                return;
            }

            double effectStatus;
            double diceFactor;
            double itemFactor;
            double levelSuccess;
            double effectSuccess;
            double itemSuccess;
            if (parentEffect == null) // exo
            {
                effectStatus = 100;
                itemStatus = 89 + Math.Sqrt(EffectManager.Instance.GetEffectPower(runeEffect)) + Math.Sqrt(itemStatus);
                diceFactor = 40;
                itemFactor = 54;
                levelSuccess = 5;
            }
            else
            {
                effectStatus = Math.Max(0, GetProgress(effectToImprove?.Value ?? 0, parentEffect.Max, parentEffect.Min) * 100);

                if (effectToImprove != null && IsOverMax(effectToImprove, runeEffect))
                {

                    itemStatus = Math.Max(itemStatus, effectStatus/2);
                    effectStatus += EffectManager.Instance.GetEffectPower(runeEffect);
                }

                diceFactor = 30;
                itemFactor = 50;
                levelSuccess = 5;
            }

            effectStatus = Math.Min(100, effectStatus);
            itemStatus = Math.Min(99, itemStatus);

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

        private double GetProgress(double value, double max, double min)
        {
            if (min < 0 || max < 0)
            {
                var x = max;
                max = -min;
                min = -x;
            }

            if (max == min && max != 0) return value / max;
            else if (max == 0) return 1d;
            else return (value - min) / (max - min);
        }
    }
}