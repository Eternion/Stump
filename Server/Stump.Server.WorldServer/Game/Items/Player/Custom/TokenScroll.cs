using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
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
            Owner.SendServerMessage(string.Format("Vous avez reçu {0} Jetons en utilisant votre {1}", wonTokens, Template.Name));

            return 1;
        }
    }
}
