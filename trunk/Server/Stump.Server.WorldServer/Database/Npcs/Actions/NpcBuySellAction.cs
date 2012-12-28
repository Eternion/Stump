using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.ModelConfiguration;
using NLog;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Dialogs.Npcs;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Database
{
    public class NpcBuySellActionConfiguration : EntityTypeConfiguration<NpcBuySellAction>
    {
        public NpcBuySellActionConfiguration()
        {
            Map(x => x.Requires("Discriminator").HasValue("Shop"));

            Property(x => x.TokenId).HasColumnName("Shop_TokenId");
            Property(x => x.CanSell).HasColumnName("Shop_CanSell");
            Property(x => x.MaxStats).HasColumnName("Shop_MaxStats");

        }
    }
    public class NpcBuySellAction : Npcs.NpcAction
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

        [DefaultValue(1)]
        public bool CanSell
        {
            get;
            set;
        }

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