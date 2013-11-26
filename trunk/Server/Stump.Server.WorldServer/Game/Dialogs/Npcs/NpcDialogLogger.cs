using System.Collections.Generic;
using Stump.Server.WorldServer.Database.Items.Shops;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Items.Player;

namespace Stump.Server.WorldServer.Game.Dialogs.Npcs
{
    public class NpcDialogLogger : NpcShopDialog
    {
        protected NpcDialogLogger(Character character, Npc npc, IEnumerable<NpcItem> items) : base(character, npc, items)
        {
            Character = character;
            Npc = npc;
            Items = items;
            CanSell = true;
        }

        protected NpcDialogLogger(Character character, Npc npc, IEnumerable<NpcItem> items, ItemTemplate token) : base(character, npc, items, token)
        {
            Character = character;
            Npc = npc;
            Items = items;
            Token = token;
            CanSell = true;
        }

        public override void ItemBoughtEvent(BasePlayerItem item, uint amount, uint finalPrice, bool isToken)
        {
            base.ItemBoughtEvent(item, amount, finalPrice, isToken);
        }
    }
}
