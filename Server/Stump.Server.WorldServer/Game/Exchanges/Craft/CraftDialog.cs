using System.Collections.Generic;
using System.Linq;
using Stump.Core.Collections;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Jobs;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Exchanges.Trades;
using Stump.Server.WorldServer.Game.Exchanges.Trades.Players;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Interactives.Skills;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Jobs;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Exchanges.Craft
{
    public class CraftDialog : ITrade
    {
        public CraftDialog(Character character, InteractiveObject interactive, Skill skill)
        {
            Interactive = interactive;
            Skill = skill;
            Crafter = new Crafter(this, character);
            Job = character.Jobs[skill.SkillTemplate.ParentJobId];
            Amount = 1;
        }

        public Character Character => Crafter.Character;
        public ExchangeTypeEnum ExchangeType => ExchangeTypeEnum.CRAFT;
        public Trader FirstTrader => Crafter;
        public Trader SecondTrader => Crafter;

        public Crafter Crafter
        {
            get;
            private set;
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

        public int Amount
        {
            get;
            private set;
        }

        public DialogTypeEnum DialogType => DialogTypeEnum.DIALOG_EXCHANGE;

        public void Close()
        {
            Character.ResetDialog();
            InventoryHandler.SendExchangeLeaveMessage(Character.Client, DialogType, false);
        }

        public void Open()
        {
            InventoryHandler.SendExchangeStartedMessage(Character.Client, ExchangeType);
            InventoryHandler.SendExchangeStartOkCraftWithInformationMessage(Character.Client, Skill);

            Character.SetDialoger(Crafter);
        }

        public bool ChangeRecipe(RecipeRecord recipe)
        {
            if (recipe.JobId != Job.Id)
                return false;

            if (recipe.ResultLevel > Job.Level)
                return false;

            bool valid = true;
            for (int i = 0; valid && i < recipe.IngredientIds.Count; i++)
            {
                var item = Character.Inventory.TryGetItem(recipe.Ingredients[i]);

                valid = item != null && Crafter.MoveItem(item, (int)recipe.Quantities[i]);
            }

            if (!valid)
                return false;
            
            Amount = 1;
            InventoryHandler.SendExchangeCraftCountModifiedMessage(Character.Client, 1);
            return true;
        }

        public bool ChangeAmount(int amount)
        {
            if (amount < 0)
                return false;

            Amount = amount;
            InventoryHandler.SendExchangeCraftCountModifiedMessage(Character.Client, amount);
            return true;
        }

        public bool Craft()
        {
            var recipe = FindMatchingRecipe();

            if (recipe == null)
            {
                InventoryHandler.SendExchangeCraftResultMessage(Character.Client, ExchangeCraftResultEnum.CRAFT_FAILED);
                return false;
            }

            if (Crafter.Items.Any(x => Character.Inventory[x.Guid]?.Stack < x.Stack*Amount))
            {
                InventoryHandler.SendExchangeCraftResultMessage(Character.Client, ExchangeCraftResultEnum.CRAFT_FAILED);
                return false;
            }

            foreach (var item in Crafter.Items.ToArray())
            {
                var playerItem = Character.Inventory[item.Guid];
                var left = playerItem.Stack - Character.Inventory.RemoveItem(playerItem, (int)item.Stack*Amount, removeItemMsg:false);

                // remove the ingredient since there are no longer enough item
                if (item.Stack > left)
                {
                    Crafter.MoveItem(playerItem, 0);
                }
            }

            var createdItem = Character.Inventory.AddItem(recipe.ItemTemplate, Amount, false);
            InventoryHandler.SendExchangeCraftResultWithObjectDescMessage(Character.Client,
                ExchangeCraftResultEnum.CRAFT_SUCCESS, createdItem);
            InventoryHandler.SendExchangeCraftInformationObjectMessage(Character.Client, createdItem, Character);
            ChangeAmount(1);
            return true;
        }

        private RecipeRecord FindMatchingRecipe()
        {
            return (from recipe in Skill.SkillTemplate.Recipes
                    let valid = !(from item in Crafter.Items
                                  let index = recipe.IngredientIds.IndexOf(item.Template.Id)
                                  where index < 0 || recipe.Quantities[index] != item.Stack
                                  select item).Any()
                    where valid
                    select recipe).FirstOrDefault();
        }
    }
}