using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
using NLog;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Database
{
    public class InteractiveSpawnConfiguration : EntityTypeConfiguration<InteractiveSpawn>
    {
        public InteractiveSpawnConfiguration()
        {
            ToTable("interactives_spawns");
            HasOptional(x => x.Template);
            HasMany(x => x.Skills).WithMany().Map(x => x.
                MapLeftKey("InteractiveSpawnId").
                MapRightKey("SkillId").
                ToTable("interactives_spawns_skills"));
        }
    }

    public class InteractiveSpawn
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private Map m_map;
        private IList<SkillRecord> m_skills;

        public InteractiveSpawn()
        {
            CustomSkills = new HashSet<SkillRecord>();
        }

        // Primitive properties

        public int Id
        {
            get;
            set;
        }

        public InteractiveTemplate Template
        {
            get;
            set;
        }

        public int ElementId
        {
            get;
            set;
        }

        public int MapId
        {
            get;
            set;
        }

        // Navigation properties

        public virtual ICollection<SkillRecord> CustomSkills
        {
            get;
            set;
        }

        /// <summary>
        /// Custom skills if Template is null
        /// </summary>
        public HashSet<SkillRecord> Skills
        {
            get;
            set;
        }

        public IEnumerable<SkillRecord> GetSkills()
        {
            return Template != null ? Template.Skills : Skills;
        }

        public ObjectPosition GetPosition()
        {
            var map = GetMap();

            var elements = map.Record.FindMapElement(ElementId);

            if (elements.Length <= 0)
                return new ObjectPosition(map, Cell.Null);

            if (elements.Length > 1)
                logger.Debug("More than 1 elements found in interactive id = {0}", Id);

            var cell = elements[0].CellId;

            return new ObjectPosition(map, cell);
        }

        public Map GetMap()
        {
            return m_map ?? (m_map = Game.World.Instance.GetMap(MapId));
        }

    }
}