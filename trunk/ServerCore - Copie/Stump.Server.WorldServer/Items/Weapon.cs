
using Stump.Database;
using Stump.Database.WorldServer;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Items
{
    public class Weapon : Item
    {
        public Weapon(LivingEntity owner, WeaponTemplate template, long guid)
            : base(template, guid)
        {
        }

        public Weapon(LivingEntity owner, WeaponTemplate template, long guid, CharacterInventoryPositionEnum position)
            : base(template, guid, position)
        {
        }

        public Weapon(LivingEntity owner, WeaponTemplate template, long guid, CharacterInventoryPositionEnum position,
                      uint stack)
            : base(template, guid, position, stack)
        {
        }

        public Weapon(LivingEntity owner, ItemRecord record)
            : base(record)
        {
        }

        public new WeaponTemplate Template
        {
            get { return base.Template as WeaponTemplate; }
        }
    }
}