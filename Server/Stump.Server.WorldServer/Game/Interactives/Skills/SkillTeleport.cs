using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Game.Interactives.Skills
{
    [Discriminator("Teleport", typeof(Skill), typeof(int), typeof(InteractiveCustomSkillRecord), typeof(InteractiveObject))]
    public class SkillTeleport : CustomSkill
    {
        private bool m_mustRefreshPosition;
        private ObjectPosition m_position;

        public SkillTeleport(int id, InteractiveCustomSkillRecord record, InteractiveObject interactiveObject)
            : base (id, record, interactiveObject)
        {
        }

        public override bool IsEnabled(Character character)
        {
            return Record.IsConditionFilled(character);
        }

        public override int StartExecute(Character character)
        {
            character.Teleport(GetPosition());
            return 0;
        }

        private void RefreshPosition()
        {
            var map = World.Instance.GetMap(MapId);

            if (map == null)
                throw new Exception(string.Format("Cannot load SkillTeleport id={0}, map {1} isn't found", Id, MapId));

            var cell = map.Cells[CellId];

            m_position = new ObjectPosition(map, cell, Direction);
        }

        public ObjectPosition GetPosition()
        {
            if (m_position == null || m_mustRefreshPosition)
                RefreshPosition();

            m_mustRefreshPosition = false;

            return m_position;
        }

        public int MapId
        {
            get { return Record.GetParameter<int>(0); }
        }

        public int CellId
        {
            get { return Record.GetParameter<int>(1); }
        }

        public DirectionsEnum Direction
        {
            get { return (DirectionsEnum)Record.GetParameter<int>(2, true); }
        }
    }
}