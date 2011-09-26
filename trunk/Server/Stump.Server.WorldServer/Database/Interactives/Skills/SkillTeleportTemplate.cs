using System;
using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Interactives;
using Stump.Server.WorldServer.Worlds.Interactives.Skills;
using Stump.Server.WorldServer.Worlds.Maps;
using Stump.Server.WorldServer.Worlds.Maps.Cells;

namespace Stump.Server.WorldServer.Database.Interactives.Skills
{
    [ActiveRecord(DiscriminatorValue = "Teleport")]
    public class SkillTeleportTemplate : SkillTemplate
    {
        private int m_cellId;
        private DirectionsEnum m_direction;
        private int m_mapId;
        private ObjectPosition m_position;

        [Property(NotNull = true)]
        public int MapId
        {
            get { return m_mapId; }
            set
            {
                m_mapId = value;
                RefreshPosition();
            }
        }

        [Property(NotNull = true)]
        public int CellId
        {
            get { return m_cellId; }
            set
            {
                m_cellId = value;
                RefreshPosition();
            }
        }

        [Property(NotNull = true)]
        public DirectionsEnum Direction
        {
            get { return m_direction; }
            set
            {
                m_direction = value;
                RefreshPosition();
            }
        }

        [Property("`Condition`")]
        public string Condition
        {
            get;
            set;
        }

        public override int SkillId
        {
            get { return 184; }
        }

        public override Skill GenerateSkill(int id, InteractiveObject interactiveObject)
        {
            return new SkillTeleport(id, this, interactiveObject);
        }

        public bool IsConditionFilled(Character character)
        {
            return true;
        }

        private void RefreshPosition()
        {
            Map map = Worlds.World.Instance.GetMap(MapId);

            if (map == null)
                throw new Exception(string.Format("Cannot load SkillTeleport id={0}, map {1} isn't found", Id, MapId));

            Cell cell = map.Cells[CellId];

            m_position = new ObjectPosition(map, cell, Direction);
        }

        public ObjectPosition GetPosition()
        {
            if (m_position  == null)
                RefreshPosition();

            return m_position;
        }
    }
}