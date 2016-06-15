using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Dialogs;
using Stump.Server.WorldServer.Game.Exchanges.Craft.Runes;
using Stump.Server.WorldServer.Game.Exchanges.Trades.Players;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Interactives.Skills;
using Stump.Server.WorldServer.Handlers.Inventory;

namespace Stump.Server.WorldServer.Game.Exchanges.Craft
{
    public class MultiCraftRequest : RequestBox
    {

        public MultiCraftRequest(Character source, Character target, InteractiveObject interactive, Skill skill)
            : base(source, target)
        {
            Interactive = interactive;
            Skill = skill;
        }
        public override bool IsExchangeRequest => true;

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

        protected override void OnOpen()
        {
            InventoryHandler.SendExchangeOkMultiCraftMessage(Source.Client, Source, Target, ExchangeTypeEnum.MULTICRAFT_CRAFTER);
            InventoryHandler.SendExchangeOkMultiCraftMessage(Target.Client, Source, Source, ExchangeTypeEnum.MULTICRAFT_CUSTOMER);
        }

        protected override void OnAccept()
        {
            if (Skill is SkillCraft)
            {
                var trade = new MultiCraftDialog(Source, Target, Interactive, Skill);
                trade.Open();
            }
            else if (Skill is SkillRuneCraft)
            {
                var trade = new MultiRuneMagicCraftDialog(Source, Target, Interactive, Skill);
                trade.Open();
            }
        }

        protected override void OnDeny()
        {
            InventoryHandler.SendExchangeLeaveMessage(Source.Client, DialogTypeEnum.DIALOG_EXCHANGE, false);
            InventoryHandler.SendExchangeLeaveMessage(Target.Client, DialogTypeEnum.DIALOG_EXCHANGE, false);
        }

        protected override void OnCancel()
        {
            Deny();
        }
    }
}