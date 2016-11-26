using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Extensions;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Items.Pets;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Database.Monsters;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.Look;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Handlers.Items;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    [ItemType(ItemTypeEnum.MONTILIER)]
    [ItemType(ItemTypeEnum.FAMILIER)]
    public sealed class PetItem : BasePlayerItem
    {
        public const EffectsEnum MealCountEffect = EffectsEnum.Effect_MealCount;

        private Dictionary<int, EffectDice> m_monsterKilledEffects;

        [Variable]
        public static int MealsPerBonus = 3;


        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public PetItem(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
            PetTemplate = PetManager.Instance.GetPetTemplate(Template.Id);
            MaxPower = IsRegularPet ? GetItemMaxPower() : 0;
            MaxLifePoints = Template.Effects.OfType<EffectDice>().FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LifePoints)?.DiceNum ?? 0;
            
            InitializeEffects();

            if (IsEquiped())
                Owner.FightEnded += OnFightEnded;
        }

        private double GetItemMaxPower()
        {
            var groups = PetTemplate.Foods.GroupBy(x => x.BoostedEffect).ToArray();
            double max = 0;

            foreach(var group1 in groups)
            {
                var possibleEffect = PetTemplate.PossibleEffects.OfType<EffectDice>().FirstOrDefault(x => x.EffectId == group1.Key);

                if (possibleEffect == null)
                    continue;

                var sum = PetManager.Instance.GetEffectMaxPower(possibleEffect);
                foreach(var group2 in groups.Where(x => x != group1))
                {
                    if (group1.CompareEnumerable(group2)) // same conditions
                    {
                        possibleEffect = PetTemplate.PossibleEffects.OfType<EffectDice>().FirstOrDefault(x => x.EffectId == group1.Key);

                        if (possibleEffect == null)
                            continue;

                        sum += PetManager.Instance.GetEffectMaxPower(possibleEffect);
                    }
                }

                if (sum > max)
                    max = sum;
            }

            return max;
        }

        public int MaxLifePoints
        {
            get;
        }

        public bool IsRegularPet => PetTemplate.PossibleEffects.Count > 0;
        private void InitializeEffects()
        {
            // new item
            if (Effects.OfType<EffectInteger>().All(x => x.EffectId != MealCountEffect))
            {
                Effects.RemoveAll(x => x.EffectId == EffectsEnum.Effect_LifePoints ||
                                       x.EffectId == EffectsEnum.Effect_LastMeal ||
                                       x.EffectId == EffectsEnum.Effect_LastMealDate ||
                                       x.EffectId == EffectsEnum.Effect_Corpulence);
                Effects.Add(LifePointsEffect = new EffectInteger(EffectsEnum.Effect_LifePoints, (short)MaxLifePoints));
                m_monsterKilledEffects = new Dictionary<int, EffectDice>();
            }
            else
            {
                LifePointsEffect = Effects.OfType<EffectInteger>().First(x => x.EffectId == EffectsEnum.Effect_LifePoints);
                LastMealDateEffect = Effects.OfType<EffectDate>().FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LastMealDate);
                LastMealEffect = Effects.OfType<EffectInteger>().FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_LastMeal);
                CorpulenceEffect = Effects.OfType<EffectInteger>().FirstOrDefault(x => x.EffectId == EffectsEnum.Effect_Corpulence);

                m_monsterKilledEffects = Effects.OfType<EffectDice>().ToDictionary(x => (int)x.DiceNum);
            }
        }

        public override bool CanFeed(BasePlayerItem item)
        {
            return IsRegularPet && item.Template.Type.SuperType != ItemSuperTypeEnum.SUPERTYPE_PET;
        }

        private EffectInteger LifePointsEffect
        {
            get;
            set;
        }

        private EffectDate LastMealDateEffect
        {
            get;
            set;
        }

        private EffectInteger LastMealEffect
        {
            get;
            set;
        }

        private EffectInteger CorpulenceEffect
        {
            get;
            set;
        }

        public int LifePoints
        {
            get { return LifePointsEffect.Value; }
            set { LifePointsEffect.Value = (short)value;

                Invalidate();

                if (value <= 0)
                    Die();

            }
        }

        public DateTime? LastMealDate
        {
            get { return LastMealDateEffect?.GetDate(); }
            set
            {
                if (value == null)
                {
                    if (LastMealDateEffect == null)
                        return;

                    Effects.Remove(LastMealDateEffect);
                    LastMealDateEffect = null;
                }
                else
                {
                    if (LastMealDateEffect != null)
                        LastMealDateEffect.SetDate(value.Value);
                    else
                        Effects.Add(LastMealDateEffect = new EffectDate(EffectsEnum.Effect_LastMealDate, value.Value));
                }


                Invalidate();
            }
        }

        public int? LastMeal
        {
            get { return LastMealEffect?.Value; }
            set
            {
                if (value == null)
                {
                    if (LastMealEffect == null)
                        return;

                    Effects.Remove(LastMealEffect);
                    LastMealEffect = null;
                }
                else
                {
                    if (LastMealEffect != null)
                        LastMealEffect.Value = (short)value.Value;
                    else
                        Effects.Add(LastMealEffect = new EffectInteger(EffectsEnum.Effect_LastMeal, (short)value.Value));
                }


                Invalidate();
            }
        }

        public int? Corpulence
        {
            get { return CorpulenceEffect?.Value; }
            set
            {
                if (value == null)
                {
                    if (CorpulenceEffect == null)
                        return;

                    Effects.Remove(CorpulenceEffect);
                    CorpulenceEffect = null;
                }
                else
                {
                    if (CorpulenceEffect != null)
                        CorpulenceEffect.Value = (short)value.Value;
                    else
                        Effects.Add(CorpulenceEffect = new EffectInteger(EffectsEnum.Effect_Corpulence, (short)value.Value));
                }


                Invalidate();
            }
        }

        private int IncreaseCreatureKilledCount(MonsterTemplate monster)
        {
            EffectDice effect;

            if (!m_monsterKilledEffects.TryGetValue(monster.Id, out effect))
            {
                effect = new EffectDice((short) EffectsEnum.Effect_MonsterKilledCount, 1, (short) monster.Id, 0, new EffectBase());
                m_monsterKilledEffects.Add(monster.Id, effect);
                Effects.Add(effect);
            }
            else
                effect.Value++;

            return effect.Value;
        }

        public PetTemplate PetTemplate
        {
            get;
        }

        public double MaxPower
        {
            get;
        }

        private void Die()
        {
            ItemTemplate ghostItem; 
            if (PetTemplate.GhostItemId == null || (ghostItem = ItemManager.Instance.TryGetTemplate(PetTemplate.GhostItemId.Value)) == null)
            {
                LifePoints = 1;
                logger.Error($"Pet {PetTemplate.Id} died but has not ghost item");
                return;
            }

            var item = ItemManager.Instance.CreatePlayerItem(Owner, ghostItem, 1, Effects.Clone());
            Owner.Inventory.RemoveItem(this);
            Owner.Inventory.AddItem(item);
        }

        public override bool OnRemoveItem()
        {
            return base.OnRemoveItem();
        }

        public override bool Feed(BasePlayerItem food)
        {
            var possibleFood = PetTemplate.Foods.FirstOrDefault(x => (x.FoodType == FoodTypeEnum.ITEM && x.FoodId == food.Template.Id) ||
                                                            (x.FoodType == FoodTypeEnum.ITEMTYPE && x.FoodId == food.Template.TypeId));

            if (possibleFood == null)
                return false;

            if (Corpulence == 3)
            {

            }

            var effectMealCount = Effects.OfType<EffectInteger>().FirstOrDefault(x => x.EffectId == MealCountEffect);

            if (effectMealCount == null)
            {
                effectMealCount = new EffectInteger(MealCountEffect, 1);
                Effects.Add(effectMealCount);
            }
            else
                effectMealCount.Value++;

            if (effectMealCount.Value % MealsPerBonus == 0)
            {
                AddBonus(possibleFood);
            }

            LastMealDate = DateTime.Now;
            LastMeal = food.Template.Id;

            Invalidate();
            Owner.Inventory.RefreshItem(this);

            return true;
        }

        private bool AddBonus(PetFoodRecord food)
        {
            var possibleEffect = PetTemplate.PossibleEffects.OfType<EffectDice>().FirstOrDefault(x => x.EffectId == food.BoostedEffect);
            var effect = Effects.OfType<EffectInteger>().FirstOrDefault(x => x.EffectId == food.BoostedEffect);

            if (possibleEffect == null)
                return false;

            if (effect?.Value >= possibleEffect.Max)
                return false;

            if (PetTemplate.PossibleEffects.Count > 1 && EffectManager.Instance.GetItemPower(this) >= MaxPower)
                return false;

            if (effect == null)
            {
                Effects.Add(effect = new EffectInteger(food.BoostedEffect, (short) food.BoostAmount));
                if (IsEquiped())
                {
                    var handler = EffectManager.Instance.GetItemEffectHandler(effect, Owner, this);
                    handler.Operation = ItemEffectHandler.HandlerOperation.APPLY;
                    handler.Apply();

                    Owner.RefreshStats();
                }
            }
            else
            {
                if (IsEquiped())
                {
                    var handler = EffectManager.Instance.GetItemEffectHandler(effect, Owner, this);
                    handler.Operation = ItemEffectHandler.HandlerOperation.UNAPPLY;
                    handler.Apply();

                    effect.Value += (short) food.BoostAmount;

                    handler.Operation = ItemEffectHandler.HandlerOperation.APPLY;
                    handler.Apply();
                    Owner.RefreshStats();
                }
                else
                    effect.Value += (short) food.BoostAmount;
            }

            return true;
        }

        public override bool OnEquipItem(bool unequip)
        {
            if (unequip)
                Owner.FightEnded -= OnFightEnded;
            else
                Owner.FightEnded += OnFightEnded;

            if (unequip)
                return base.OnEquipItem(true);

            if (Owner.IsRiding)
                Owner.Dismount();

            return base.OnEquipItem(false);
        }

        public override ActorLook UpdateItemSkin(ActorLook characterLook)
        {
            if (AppearanceId <= 0)
                return characterLook;

            switch (Template.Type.ItemType)
            {
                case ItemTypeEnum.FAMILIER:
                    if (IsEquiped())
                        characterLook.SetPetSkin((short) AppearanceId);
                    else
                        characterLook.RemovePets();
                    break;
                case ItemTypeEnum.MONTILIER:
                    if (IsEquiped())
                    {
                        var mountLook = new ActorLook((short) AppearanceId);

                        //KramKram
                        if (Template.Id == (int)ItemIdEnum.KRAMKRAM_13182)
                        {
                            Color color1;
                            Color color2;

                            if (characterLook.Colors.TryGetValue(3, out color1)&&
                                characterLook.Colors.TryGetValue(4, out color2))
                            {
                                mountLook.AddColor(1, color1);
                                mountLook.AddColor(2, color2);
                            }
                        }

                        characterLook.BonesID = 2;
                        mountLook.SetRiderLook(characterLook);

                        return mountLook;
                    }
                    else
                    {
                        var look = characterLook.GetRiderLook();

                        if (look != null)
                        {
                            characterLook = look;
                            characterLook.BonesID = 1;
                        }
                        return characterLook;
                    }
            }
            
            return characterLook;
        }

        private void OnFightEnded(Character character, CharacterFighter fighter)
        {
            bool update = false;
            if (!fighter.Fight.IsDeathTemporarily && fighter.Fight.Losers == fighter.Team && IsEquiped())
            {
                LifePoints--;
                update = true;
            }

            if (fighter.Fight is FightPvM)
            {
                foreach(var monster in fighter.OpposedTeam.Fighters.OfType<MonsterFighter>().Where(x => x.IsDead()))
                {
                    var food = PetTemplate.Foods.FirstOrDefault(x => x.FoodType == FoodTypeEnum.MONSTER && x.FoodId == monster.Monster.Template.Id);

                    if (food != null)
                    {
                        if (IncreaseCreatureKilledCount(monster.Monster.Template)%food.FoodQuantity == 0)
                            AddBonus(food);

                        Invalidate();
                        update = true;
                    }
                }

            }


            if (update && LifePoints > 0)
                Owner.Inventory.RefreshItem(this);
        }
    }
}
