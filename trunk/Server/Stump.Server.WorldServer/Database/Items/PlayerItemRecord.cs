using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.Items;

namespace Stump.Server.WorldServer.Database
{
    public class PlayerItemRecordConfiguration : EntityTypeConfiguration<PlayerItemRecord>
    {
        public PlayerItemRecordConfiguration()
        {
            Map(x => x.Requires("Discriminator").HasValue("Player"));
        }
    }
    
    public class PlayerItemRecord : ItemRecord<PlayerItemRecord>
    {
        public int OwnerId
        {
            get;
            set;
        }

        public int PositionInt
        {
            get;
            set;
        }

        public CharacterInventoryPositionEnum Position
        {
            get { return (CharacterInventoryPositionEnum) PositionInt; }
            set { PositionInt = (int) value; }
        }
    }
}