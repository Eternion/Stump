using System.Collections.Generic;
using Castle.ActiveRecord;
using Stump.Server.WorldServer.Database.Interactives.Skills;
using Stump.Server.WorldServer.Worlds.Maps;

namespace Stump.Server.WorldServer.Database.Interactives
{
    [ActiveRecord("interactives_spawns")]
    public class InteractiveSpawn : WorldBaseRecord<InteractiveSpawn>
    {
        private Map m_map;
        private IList<SkillTemplate> m_skills;

        [PrimaryKey(PrimaryKeyType.Native)]
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Can be null
        /// </summary>
        [BelongsTo(NotNull = false)]
        public InteractiveTemplate Template
        {
            get;
            set;
        }

        [Property(NotNull = true)]
        public int ElementId
        {
            get;
            set;
        }

        [Property(NotNull = true)]
        public int MapId
        {
            get;
            set;
        }

        /// <summary>
        /// Custom skills in case of Template is null
        /// </summary>
        [HasAndBelongsToMany(Table = "interactives_custom_skills", ColumnKey = "InteractiveId", ColumnRef = "SkillId")]
        public IList<SkillTemplate> Skills
        {
            get { return m_skills ?? (m_skills = new List<SkillTemplate>()); }
            set { m_skills = value; }
        }

        public IEnumerable<SkillTemplate> GetSkills()
        {
            return Template != null ? Template.Skills : Skills;
        }

        public Map GetMap()
        {
            return m_map ?? (m_map = Worlds.World.Instance.GetMap(MapId));
        }
    }
}