using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    [ItemId(ItemIdEnum.TokenScroll)]
    public sealed class TokenScroll : BasePlayerItem
    {
        [Variable]
        private const uint WonTokens = 100;

        public TokenScroll(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {
            Owner.Area.AddMessage(() =>
            {
                var tokens = Owner.Inventory.Tokens;

                if (tokens != null)
                {
                    tokens.Stack += WonTokens;
                    Owner.Inventory.RefreshItem(tokens); 
                }
                else
                    Owner.Inventory.AddItem(Inventory.TokenTemplate, (int)WonTokens);


                Owner.Inventory.Save();
                Owner.SendServerMessage(string.Format("Vous avez reçu {0} Jetons en utilisant votre {1}", WonTokens, Template.Name));
            });

            return 1;
        }
    }
}
