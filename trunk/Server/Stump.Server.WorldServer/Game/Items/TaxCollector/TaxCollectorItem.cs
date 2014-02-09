using System.Collections.Generic;
using System.Linq;
using Stump.Core.Extensions;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items.Player;

namespace Stump.Server.WorldServer.Game.Items.TaxCollector
{
    public class TaxCollectorItem : Item<TaxCollectorItemRecord>
    {
        #region Constructors

        public TaxCollectorItem(TaxCollectorItemRecord record)
            : base(record)
        {
        }

        public TaxCollectorItem(TaxCollectorNpc owner, int guid, ItemTemplate template, List<EffectBase> effects, uint stack)
        {
            Record = new TaxCollectorItemRecord // create the associated record
                         {
                             Id = guid,
                             OwnerId = owner.Id,
                             Template = template,
                             Stack = stack,
                             Effects = effects,
                         };
        }

        #endregion

        #region Functions

        public bool MustStackWith(TaxCollectorItem compared)
        {
            return (compared.Template.Id == Template.Id &&
                    compared.Effects.CompareEnumerable(Effects));
        }

        public bool MustStackWith(BasePlayerItem compared)
        {
            return (compared.Template.Id == Template.Id &&
                    compared.Effects.CompareEnumerable(Effects));
        }

        public ObjectItem GetObjectItem()
        {
            return new ObjectItem(63, (short)Template.Id, 0, false, Effects.Select(x => x.GetObjectEffect()), Guid, (int)Stack);
        }

        public ObjectItemQuantity GetObjectItemQuantity()
        {
            return new ObjectItemQuantity(Guid, (int)Stack);
        }

        #endregion
    }
}
