using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Npcs;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;

namespace Stump.Server.WorldServer.Database.Npcs.Replies
{
    [Discriminator("Teleport", typeof(NpcReply), typeof(NpcReplyRecord))]
    public class TeleportReply : NpcReply
    {
        private int m_cellId;
        private DirectionsEnum m_direction;
        private int m_mapId;
        private bool m_mustRefreshPosition;
        private ObjectPosition m_position;

        public TeleportReply(NpcReplyRecord record)
            : base(record)
        {
        }

        /// <summary>
        /// Parameter 0
        /// </summary>
        public int MapId
        {
            get { return GetParameter<int>(0); }
            set
            {
                SetParameter(0, value);
                m_mustRefreshPosition = true;
            }
        }

        /// <summary>
        /// Parameter 1
        /// </summary>
        public int CellId
        {
            get { return GetParameter<int>(1); }
            set
            {
                SetParameter(1, value);
                m_mustRefreshPosition = true;
            }
        }

        /// <summary>
        /// Parameter 2
        /// </summary>
        public DirectionsEnum Direction
        {
            get { return (DirectionsEnum) GetParameter<int>(2); }
            set
            {
                SetParameter(2, (int) value);
                m_mustRefreshPosition = true;
            }
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

        public override bool Execute(Npc npc, Character character)
        {
            if (!base.Execute(npc, character))
                return false;

            return character.Teleport(GetPosition());
        }
    }
}