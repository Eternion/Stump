using System;
using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Conditions;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Interactives.Skills;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Database
{
    public class InteractiveSkillTeleportRecordConfiguration : EntityTypeConfiguration<InteractiveSkillTeleportRecord>
    {
        public InteractiveSkillTeleportRecordConfiguration()
        {
            Map(x => x.Requires("Discriminator").HasValue("Teleport"));

            Property(x => x.MapId).HasColumnName("Teleport_MapId");
            Property(x => x.CellId).HasColumnName("Teleport_CellId");
            Property(x => x.Direction).HasColumnName("Teleport_Direction");
            Property(x => x.Condition).HasColumnName("Teleport_Condition");
        }
    }

    public partial class InteractiveSkillTeleportRecord : InteractiveSkillRecord
    {
        // Primitive properties

        public int MapId
        {
            get;
            set;
        }
        public int CellId
        {
            get;
            set;
        }
        public int Direction
        {
            get;
            set;
        }
        public string Condition
        {
            get;
            set;
        }

        private ConditionExpression m_conditionExpression;
        private ObjectPosition m_position;
        private bool m_mustRefreshPosition;

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