using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Jobs;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Exchanges.Trades;
using Stump.Server.WorldServer.Game.Exchanges.Trades.Players;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Interactives.Skills;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Jobs;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Exchanges.Craft
{
    public abstract class CraftDialog : ITrade
    {
        protected CraftDialog(InteractiveObject interactive, Skill skill, Job job)
        {
            Interactive = interactive;
            Skill = skill;
            Job = job;
        }

        public DialogTypeEnum DialogType => DialogTypeEnum.DIALOG_EXCHANGE;
        public ExchangeTypeEnum ExchangeType => ExchangeTypeEnum.CRAFT;
        public abstract void Close();

        public abstract Trader FirstTrader
        {
            get;
        }

        public abstract Trader SecondTrader
        {
            get;
        }

        public InteractiveObject Interactive
        {
            get;
            private set;
        }

        public Skill Skill
        {
            get;
            private set;
        }

        public Job Job
        {
            get;
            private set;
        }

        public Crafter Crafter
        {
            get;
            protected set;
        }

        public CraftingActor Receiver
        {
            get;
            protected set;
        }

        public WorldClientCollection Clients
        {
            get;
            protected set;
        }

        public int Amount
        {
            get;
            private set;
        }

        public bool ChangeRecipe(CraftingActor actor, RecipeRecord recipe)
        {
            if (recipe.JobId != Job.Id)
                return false;

            if (recipe.ResultLevel > Job.Level)
                return false;

            bool valid = true;
            for (int i = 0; valid && i < recipe.IngredientIds.Count; i++)
            {
                var item = actor.Character.Inventory.TryGetItem(recipe.Ingredients[i]);

                valid = item != null && actor.MoveItemToPanel(item, (int)recipe.Quantities[i]);
            }

            if (!valid)
                return false;

            Amount = 1;
            InventoryHandler.SendExchangeCraftCountModifiedMessage(Clients, 1);
            return true;
        }

        public bool ChangeAmount(int amount)
        {
            if (amount < 0)
                return false;

            Amount = amount;
            InventoryHandler.SendExchangeCraftCountModifiedMessage(Clients, amount);
            return true;
        }

        public bool Craft()
        {
            var ingredients = GetIngredients().ToArray();
            var recipe = FindMatchingRecipe(ingredients);

            if (recipe == null)
            {
                InventoryHandler.SendExchangeCraftResultMessage(Clients, ExchangeCraftResultEnum.CRAFT_FAILED);
                return false;
            }

            if (recipe.ResultLevel > Job.Level)
            {
                InventoryHandler.SendExchangeCraftResultWithObjectIdMessage(Clients, ExchangeCraftResultEnum.CRAFT_FAILED, recipe.ItemTemplate);
                return false;
            }

            if (ingredients.Any(x => x.Owner.Inventory[x.Guid]?.Stack < x.Stack * Amount))
            {
                InventoryHandler.SendExchangeCraftResultMessage(Clients, ExchangeCraftResultEnum.CRAFT_FAILED);
                return false;
            }

            foreach (var item in ingredients)
            {
                var playerItem = item.Owner.Inventory[item.Guid];
                item.Owner.Inventory.RemoveItem(playerItem, (int)item.Stack * Amount);

                if (item.Owner == Crafter.Character)
                    Crafter.MoveItem(item.Guid, 0);
                else
                    Receiver.MoveItem(item.Guid, 0);
            }

            var xp = Job.GetCraftXp(recipe, Amount);
            Job.Experience += xp;

            if (!ItemManager.Instance.HasToBeGenerated(recipe.ItemTemplate))
            {
                var createdItem = Receiver.Character.Inventory.AddItem(recipe.ItemTemplate, Amount);
                InventoryHandler.SendExchangeCraftResultWithObjectDescMessage(Clients,
                    ExchangeCraftResultEnum.CRAFT_SUCCESS, createdItem, Amount);
                InventoryHandler.SendExchangeCraftInformationObjectMessage(Crafter.Character.Map.Clients, createdItem, Receiver.Character);
            }
            else
            {
                var dict = new Dictionary<List<EffectBase>, int>(new EffectsListComparer());
                for (int i = 0; i < Amount; i++)
                {
                    var effects = ItemManager.Instance.GenerateItemEffects(recipe.ItemTemplate);
                    if (dict.ContainsKey(effects))
                        dict[effects] += 1;
                    else
                        dict.Add(effects, 1);
                }

                foreach (var keyPair in dict)
                {
                    var createdItem = Receiver.Character.Inventory.AddItem(recipe.ItemTemplate, keyPair.Key, keyPair.Value);
                    InventoryHandler.SendExchangeCraftResultWithObjectDescMessage(Clients,
                        ExchangeCraftResultEnum.CRAFT_SUCCESS, createdItem, keyPair.Value);
                    InventoryHandler.SendExchangeCraftInformationObjectMessage(Crafter.Character.Map.Clients, createdItem, Receiver.Character);
                }
            }

            ChangeAmount(1);

            return true;
        }

        protected virtual RecipeRecord FindMatchingRecipe(PlayerTradeItem[] ingredients)
        {
            var combinedIngredients = new Dictionary<int, uint>();
            foreach (var ingredient in ingredients)
            {
                if (combinedIngredients.ContainsKey(ingredient.Template.Id))
                    combinedIngredients[ingredient.Template.Id] += ingredient.Stack;
                else
                    combinedIngredients.Add(ingredient.Template.Id, ingredient.Stack);
            }

            return (from recipe in Skill.SkillTemplate.Recipes
                    where recipe.IngredientIds.Count == combinedIngredients.Count
                    let valid = !(from item in combinedIngredients
                                  let index = recipe.IngredientIds.IndexOf(item.Key)
                                  where index < 0 || recipe.Quantities[index] != item.Value
                                  select item).Any()
                    where valid
                    select recipe).FirstOrDefault();
        }

        protected virtual IEnumerable<PlayerTradeItem> GetIngredients()
        {
            return Receiver.Items.Concat(Crafter.Items).OfType<PlayerTradeItem>();
        }
    }
}