using Castle.ActiveRecord;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Monsters;

namespace Stump.Server.WorldServer.Database.Monsters
{
    [ActiveRecord("monsters_drops")]
    public class DroppableItem : WorldBaseRecord<DroppableItem>
    {
        [PrimaryKey(PrimaryKeyType.Native)]
        public int Id
        {
            get;
            set;
        }

        [Property(NotNull = true)]
        public int MonsterOwnerId
        {
            get;
            set;
        }

        private MonsterTemplate m_template;
        public MonsterTemplate MonsterOwner
        {
            get
            {
                return m_template ?? ( m_template = MonsterManager.Instance.GetTemplate(MonsterOwnerId) );
            }
            set
            {
                m_template = value;
                MonsterOwnerId = value.Id;
            }
        }

        /// <summary>
        /// The id of the item to drop
        /// </summary>
        [Property(NotNull = true)]
        public short ItemId
        {
            get;
            set;
        }

        /// <summary>
        /// A monster cannot drop this item more times than the drop limit. 0 to disable this limit
        /// </summary>
        [Property(Default = "0", NotNull = true)]
        public int DropLimit
        {
            get;
            set;
        }

        /// <summary>
        /// Define the probability that the item drop. Between 0.00% and 100.00%
        /// </summary>
        [Property(NotNull = true)]
        public double DropRate
        {
            get;
            set;
        }


        /// <summary>
        /// How many times the rolls are threw
        /// </summary>
        [Property(Default = "1", NotNull = true)]
        public int RollsCounter
        {
            get;
            set;
        }

        /// <summary>
        /// Requiered team prospection to have a chance to drop the item
        /// </summary>
        [Property(NotNull = true, Default = "100")]
        public int ProspectingLock
        {
            get;
            set;
        }
    }
}