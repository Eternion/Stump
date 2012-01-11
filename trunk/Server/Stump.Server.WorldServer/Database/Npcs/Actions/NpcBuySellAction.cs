using System.Collections.Generic;
using Castle.ActiveRecord;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items.Shops;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Worlds.Dialogs.Npcs;
using Stump.Server.WorldServer.Worlds.Items;

namespace Stump.Server.WorldServer.Database.Npcs.Actions
{
    [ActiveRecord(DiscriminatorValue = "Shop")]
    public class NpcBuySellAction : NpcAction
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private List<NpcItem> m_items;
        public List<NpcItem> Items
        {
            get
            {
                return m_items ?? ( m_items = ItemManager.Instance.GetNpcShopItems(Id) );
            }
        }

        public override NpcActionTypeEnum ActionType
        {
            get { return NpcActionTypeEnum.ACTION_BUY_SELL; }
        }

        public override void Execute(Npc npc, Character character)
        {
            var dialog = new NpcShopDialog(character, npc, Items);
            dialog.Open();
        }
    }
}