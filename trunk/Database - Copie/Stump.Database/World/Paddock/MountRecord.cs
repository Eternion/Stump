
using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Database.Types;
using Stump.Database.WorldServer.Character;

namespace Stump.Database.WorldServer.Paddock
{
    [ActiveRecord("mounts")]
    public class MountRecord : WorldBaseRecord<MountRecord>
    {
        private IList<MountRecord> m_ancestors;
        private IList<MountBehaviorRecord> m_behaviors;

        [PrimaryKey(PrimaryKeyType.Identity, "Id")]
        public uint Id { get; set; }

        [BelongsTo("OwnerId", NotNull = true)]
        public CharacterRecord Owner { get; set; }

        [Property("ModelId", NotNull = true, Default = "0")]
        public uint ModelId { get; set; }

        [HasAndBelongsToMany(typeof (MountRecord), Table = "mounts_ancestors", ColumnKey = "MountId",
            ColumnRef = "AncestorId")]
        public IList<MountRecord> Ancestors
        {
            get { return m_ancestors ?? new List<MountRecord>(); }
            set { m_ancestors = value; }
        }

        [HasMany(typeof (MountBehaviorRecord), Cascade = ManyRelationCascadeEnum.Delete)]
        public IList<MountBehaviorRecord> Behaviors
        {
            get { return m_behaviors ?? new List<MountBehaviorRecord>(); }
            set { m_behaviors = value; }
        }

        [Property("Name", NotNull = true, Default = "'SansNom'")]
        public string Name { get; set; }

        [Property("Sex", NotNull = true, Default = "0")]
        public bool Sex { get; set; }

        [Property("Experience", NotNull = true, Default = "0")]
        public long Experience { get; set; }

        [Property("IsWild", NotNull = true, Default = "0")]
        public bool IsWild { get; set; }

        [Property("Stamina", NotNull = true, Default = "0")]
        public uint Stamina { get; set; }

        [Property("Maturity", NotNull = true, Default = "0")]
        public uint Maturity { get; set; }

        [Property("Energy", NotNull = true, Default = "0")]
        public uint Energy { get; set; }

        [Property("Serenity", NotNull = true, Default = "0")]
        public uint Serenity { get; set; }

        [Property("Love", NotNull = true, Default = "0")]
        public uint Love { get; set; }

        [Property("ReproductionCount", NotNull = true, Default = "0")]
        public int ReproductionCount { get; set; }
    }
}