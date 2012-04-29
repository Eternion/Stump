using System.Collections.Generic;
using Castle.ActiveRecord;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items.Shops;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Dialogs.Npcs;
using Stump.Server.WorldServer.Game.Items;

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

        [Property("Shop_Token")]
        public int TokenId
        {
            get;
            set;
        }

        private ItemTemplate m_token;

        public ItemTemplate Token
        {
            get
            {
                return TokenId > 0 ? m_token ?? ( m_token = ItemManager.Instance.TryGetTemplate(TokenId) ) : null;
            }
            set
            {
                m_token = value;
                TokenId = value == null ? 0 : m_token.Id;
            }
        }

        [Property("Shop_CanSell", Default = "1")]
        public bool CanSell
        {
            get;
            set;
        }

        [Property("Shop_MaxStats", Default = "0")]
        public bool MaxStats
        {
            get;
            set;
        }

        public override NpcActionTypeEnum ActionType
        {
            get { return NpcActionTypeEnum.ACTION_BUY_SELL; }
        }

        public override void Execute(Npc npc, Character character)
        {
            var dialog = new NpcShopDialog(character, npc, Items, Token)
            {
                CanSell = CanSell,
                MaxStats = MaxStats
            };
            dialog.Open();
        }
    }
}