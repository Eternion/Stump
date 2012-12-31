using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Game.Interactives.Skills
{
    [Discriminator("Teleport", typeof(Skill), typeof(int), typeof(InteractiveSkillRecord), typeof(InteractiveObject))]
    public class SkillTeleport : Skill
    {
        private bool m_mustRefreshPosition;
        private ObjectPosition m_position;

        public SkillTeleport(int id, InteractiveSkillRecord record, InteractiveObject interactiveObject)
            : base (id, record, interactiveObject)
        {
        }

        public override bool IsEnabled(Character character)
        {
            return Record.IsConditionFilled(character);
        }

        public override void Execute(Character character)
        {
            character.Teleport(GetPosition());
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