using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Effects.Instances;

namespace Stump.Server.WorldServer.Game.Exchanges.Items
{
    public abstract class TradeItem
    {
        public abstract int Guid
        {
            get;
        }

        public abstract ItemTemplate Template
        {
            get;
        }

        public abstract uint Stack
        {
            get;
            set;
        }

        public abstract List<EffectBase> Effects
        {
            get;
        }

        public abstract CharacterInventoryPositionEnum Position
        {
            get;
        }

        public ObjectItem GetObjectItem()
        {
            return new ObjectItem(
                (byte)Position,
                (short)Template.Id,
                0,
                false,
                Effects.Where(entry => !entry.Hidden).Select(entry => entry.GetObjectEffect()),
                Guid,
                (int)Stack);
        }
    }
}