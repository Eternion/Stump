using System.Collections.Generic;
using System.Linq;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Effects.Instances;

namespace Stump.Server.WorldServer.Game.Items
{
    public class MerchantItem : Item<PlayerMerchantItemRecord>
    {
        #region Fields

        public uint Price
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public MerchantItem(PlayerMerchantItemRecord record)
            : base(record)
        {
            Price = Record.Price;
        }

        public MerchantItem(Character owner, int guid, ItemTemplate template, List<EffectBase> effects, uint stack, uint price)
        {
            Price = price;

            Record = new PlayerMerchantItemRecord // create the associated record
                         {
                             Id = guid,
                             OwnerId = owner.Id,
                             Template = template,
                             Stack = stack,
                             Price = price,
                             Effects = effects,
                         };
        }

        #endregion

        #region Functions
        public bool MustStackWith(MerchantItem compared)
        {
            return ( compared.Template.Id == Template.Id &&
                    compared.Effects.CompareEnumerable(Effects) );
        }

        public bool MustStackWith(PlayerItem compared)
        {
            return ( compared.Template.Id == Template.Id &&
                    compared.Effects.CompareEnumerable(Effects) );
        }

        public ObjectItemToSell GetObjectItemToSell()
        {
            return new ObjectItemToSell((short) Template.Id, 0, false,
                                 Effects.Select(x => x.GetObjectEffect()),
                                 Guid, (int) Stack, (int) Price);
        }

        #endregion


    }
}
