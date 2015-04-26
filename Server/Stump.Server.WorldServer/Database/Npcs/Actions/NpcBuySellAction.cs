using System.Collections.Generic;
using System.ComponentModel;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.Items.Shops;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Dialogs.Npcs;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Database.Npcs.Actions
{
    [Discriminator(Discriminator, typeof(NpcActionDatabase), typeof(NpcActionRecord))]
    public class NpcBuySellAction : NpcActionDatabase
    {
        public const string Discriminator = "Shop";
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private List<NpcItem> m_items;
        private ItemTemplate m_token;

        public NpcBuySellAction(NpcActionRecord record)
            : base(record)
        {
        }

        /// <summary>
        /// Read-only
        /// </summary>
        public List<NpcItem> Items
        {
            get { return m_items ?? (m_items = ItemManager.Instance.GetNpcShopItems(Record.Id)); }
        }

        /// <summary>
        /// Parameter 0
        /// </summary>
        public int TokenId
        {
            get { return Record.GetParameter<int>(0, true); }
            set { Record.SetParameter(0, value); }
        }

        public ItemTemplate Token
        {
            get { return TokenId > 0 ? m_token ?? (m_token = ItemManager.Instance.TryGetTemplate(TokenId)) : null; }
            set
            {
                m_token = value;
                TokenId = value == null ? 0 : m_token.Id;
            }
        }

        /// <summary>
        /// Parameter 1
        /// </summary>
        [DefaultValue(1)]
        public bool CanSell
        {
            get { return Record.GetParameter<int>(1, true) == 1; }
            set { Record.SetParameter(1, value); }
        }

        /// <summary>
        /// Parameter 2
        /// </summary>
        public bool MaxStats
        {
            get { return Record.GetParameter<int>(2, true) == 1; }
            set { Record.SetParameter(2, value); }
        }

        public override NpcActionTypeEnum[] ActionType
        {
            get { return new[] { NpcActionTypeEnum.ACTION_BUY_SELL, NpcActionTypeEnum.ACTION_SELL, NpcActionTypeEnum.ACTION_BUY }; }
        }

        public override void Execute(Npc npc, Character character)
        {
            var dialog = new NpcShopDialogLogger(character, npc, Items, Token)
                {
                    CanSell = CanSell,
                    MaxStats = MaxStats
                };
            dialog.Open();
        }
    }
}