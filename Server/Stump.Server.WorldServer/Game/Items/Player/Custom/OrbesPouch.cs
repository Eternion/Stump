using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;


namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    [ItemType(ItemTypeEnum.ORBES_POUCH)]
    public sealed class OrbesPouch : BasePlayerItem
    {
        public OrbesPouch(Character owner, PlayerItemRecord record)
            : base(owner, record)
        {
        }

        public override uint UseItem(int amount = 1, Cell targetCell = null, Character target = null)
        {
            var wonOrbes = (int)Template.Price;
            var template = ItemManager.Instance.TryGetTemplate(20000);

            Owner.Inventory.AddItem(template, wonOrbes);

            Owner.SendServerMessage(string.Format("Vous avez reçu {0} Orbes en utilisant votre {1}", wonOrbes, Template.Name));

            return 1;
        }
    }
}
