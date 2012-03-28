using System;
using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Conditions;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Interactives.Skills;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Database.Interactives.Skills
{
    [ActiveRecord(DiscriminatorValue = "Teleport")]
    public class SkillTeleportRecord : TemplateDependantSkill
    {
        private bool m_mustRefreshPosition;

        private int m_cellId;
        private DirectionsEnum m_direction;
        private int m_mapId;
        private ObjectPosition m_position;

        [Property("Teleport_MapId")]
        public int MapId
        {
            get { return m_mapId; }
            set
            {
                m_mapId = value;
                m_mustRefreshPosition = true;
            }
        }

        [Property("Teleport_CellId")]
        public int CellId
        {
            get { return m_cellId; }
            set
            {
                m_cellId = value;
                m_mustRefreshPosition = true;
            }
        }

        [Property("Teleport_Direction")]
        public DirectionsEnum Direction
        {
            get { return m_direction; }
            set
            {
                m_direction = value;
                m_mustRefreshPosition = true;
            }
        }

        [Property("Teleport_Condition")]
        public string Condition
        {
            get;
            set;
        }

        private ConditionExpression m_conditionExpression;

        public ConditionExpression ConditionExpression
        {
            get
            {
                if (string.IsNullOrEmpty(Condition) || Condition == "null")
                    return null;

                return m_conditionExpression ?? ( m_conditionExpression = ConditionExpression.Parse(Condition) );
            }
            set
            {
                m_conditionExpression = value;
                Condition = value.ToString();
            }
        }


        public override int TemplateId
        {
            get
            {
                return DEFAULT_TEMPLATE;
            }
        }

        public override Skill GenerateSkill(int id, InteractiveObject interactiveObject)
        {
            return new SkillTeleport(id, this, interactiveObject);
        }

        public bool IsConditionFilled(Character character)
        {
            return m_conditionExpression == null || m_conditionExpression.Eval(character);
        }

        private void RefreshPosition()
        {
            Map map = Game.World.Instance.GetMap(MapId);

            if (map == null)
                throw new Exception(string.Format("Cannot load SkillTeleport id={0}, map {1} isn't found", Id, MapId));

            Cell cell = map.Cells[CellId];

            m_position = new ObjectPosition(map, cell, Direction);
        }

        public ObjectPosition GetPosition()
        {
            if (m_position == null || m_mustRefreshPosition)
                RefreshPosition();

            m_mustRefreshPosition = false;

            return m_position;
        }
    }
}