using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.DofusProtocol.Enums;

namespace Stump.Server.WorldServer.Database.Items
{
    [ActiveRecord(DiscriminatorValue = "Player")]
    public class PlayerItemRecord : ItemRecord<PlayerItemRecord>
    {
        [Property("Owner")]
        public int OwnerId
        {
            get;
            set;
        }

        [Property("Position", NotNull = true, Default = "63")]
        public CharacterInventoryPositionEnum Position
        {
            get;
            set;
        }

        public static PlayerItemRecord[] FindAllByOwner(int ownerId)
        {
            return FindAll(Restrictions.Eq("OwnerId", ownerId));
        }
    }
}