using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Items.Player;
using Stump.Server.WorldServer.Game.Items.Player.Custom;

namespace ArkalysPlugin.Items
{
    [ItemType(ItemTypeEnum.TOKEN_SCROLL)]
    public sealed class TokenScroll : BasePlayerItem
    {
        public TokenScroll(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {
            var tokens = Owner.Inventory.Tokens;
            var wonTokens = (uint)Template.Price;

            if (tokens != null)
            {
                tokens.Stack += wonTokens;
                Owner.Inventory.RefreshItem(tokens);
            }
            else
            {
                Owner.Inventory.CreateTokenItem(wonTokens);
            }

            WorldServer.Instance.IOTaskPool.AddMessage(() => Owner.Inventory.Save());
            Owner.SendServerMessage($"Vous avez reçu {wonTokens} Jetons en utilisant votre {Template.Name}");

            return 1;
        }
    }
}
