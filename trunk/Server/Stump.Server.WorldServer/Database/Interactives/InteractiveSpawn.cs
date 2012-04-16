using System.Collections.Generic;
using Castle.ActiveRecord;
using NLog;
using Stump.Server.WorldServer.Database.Interactives.Skills;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Database.Interactives
{
    [ActiveRecord("interactives_spawns")]
    public class InteractiveSpawn : WorldBaseRecord<InteractiveSpawn>
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private Map m_map;
        private IList<SkillRecord> m_skills;

        [PrimaryKey(PrimaryKeyType.Native)]
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Can be null
        /// </summary>
        [Property(NotNull = false)]
        public int TemplateId
        {
            get;
            set;
        }

        private InteractiveTemplate m_template;
        public InteractiveTemplate Template
        {
            get
            {
                if (TemplateId < 0)
                    return null;

                return m_template ?? ( m_template = InteractiveManager.Instance.GetTemplate(TemplateId) );
            }
            set
            {
                m_template = value;
                TemplateId = value.Id;
            }
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
        public IList<SkillRecord> Skills
        {
            get { return m_skills ?? (m_skills = new List<SkillRecord>()); }
            set { m_skills = value; }
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