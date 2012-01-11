using System;
using Castle.ActiveRecord;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Worlds.Maps;
using Stump.Server.WorldServer.Worlds.Maps.Cells;

namespace Stump.Server.WorldServer.Database.Npcs.Replies
{
    [ActiveRecord(DiscriminatorValue = "Teleport")]
    public class TeleportReply : NpcReply
    {
        private bool m_mustRefreshPosition;

        private int m_cellId;
        private DirectionsEnum m_direction;
        private int m_mapId;
        private ObjectPosition m_position;

        [Property]
        public int MapId
        {
            get
            {
                return m_mapId;
            }
            set
            {
                m_mapId = value;
                m_mustRefreshPosition = true;
            }
        }

        [Property]
        public int CellId
        {
            get
            {
                return m_cellId;
            }
            set
            {
                m_cellId = value;
                m_mustRefreshPosition = true;
            }
        }

        [Property]
        public DirectionsEnum Direction
        {
            get
            {
                return m_direction;
            }
            set
            {
                m_direction = value;
                m_mustRefreshPosition = true;
            }
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
            if (m_position == null || m_mustRefreshPosition)
                RefreshPosition();

            m_mustRefreshPosition = false;

            return m_position;
        }

        public override void Execute(Npc npc, Character character)
        {
            character.Teleport(GetPosition());
        }
    }
}