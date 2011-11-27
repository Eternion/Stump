using System.Collections.Generic;
using Castle.ActiveRecord;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items.Shops;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Worlds.Dialogs.Npcs;

namespace Stump.Server.WorldServer.Database.Npcs.Actions
{
    [ActiveRecord(DiscriminatorValue = "Shop")]
    public class NpcBuySellAction : NpcAction
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private IList<NpcItem> m_items;

        [HasMany(typeof (NpcItem))]
        public IList<NpcItem> Items
        {
            get { return m_items ?? (m_items = new List<NpcItem>()); }
            set { m_items = value; }
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