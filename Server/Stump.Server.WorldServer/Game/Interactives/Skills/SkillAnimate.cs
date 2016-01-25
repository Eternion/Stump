using Stump.Core.IO;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Interactives;
using System.Collections.Generic;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Interactives.Skills
{
    [Discriminator("Animate", typeof(Skill), typeof(int), typeof(InteractiveCustomSkillRecord), typeof(InteractiveObject))]
    public class SkillAnimate : CustomSkill
    {
        public SkillAnimate(int id, InteractiveCustomSkillRecord skillTemplate, InteractiveObject interactiveObject)
            : base(id, skillTemplate, interactiveObject)
        {
            m_mapObstacles = ObstaclesCSV == null ? new MapObstacle[0] : ObstaclesCSV.FromCSV<short>(",").Select(x => new MapObstacle(x, (sbyte)MapObstacleStateEnum.OBSTACLE_CLOSED)).ToArray();
        }

        int? m_elementId;
        short? m_cellId;
        int? m_mapId;
        string m_obstacles;
        MapObstacle[] m_mapObstacles;

        /// <summary>
        /// Parameter 0
        /// </summary>
        public int ElementId
        {
            get
            {
                return m_elementId ?? (m_elementId = Record.GetParameter<int>(0)).Value;
            }
            set
            {
                Record.SetParameter(0, value);
                m_elementId = value;
            }
        }

        /// <summary>
        /// Parameter 1
        /// </summary>
        public int MapId
        {
            get
            {
                return m_mapId ?? (m_mapId = Record.GetParameter<int>(1)).Value;
            }
            set
            {
                Record.SetParameter(1, value);
                m_mapId = value;
            }
        }

        /// <summary>
        /// Parameter 2
        /// </summary>
        public short CellId
        {
            get
            {
                return m_cellId ?? (m_cellId = Record.GetParameter<short>(2)).Value;
            }
            set
            {
                Record.SetParameter(2, value);
                m_cellId = value;
            }
        }

        public string ObstaclesCSV
        {
            get { return m_obstacles ?? (m_obstacles = Record.AdditionalParameters); }
            set
            {
                Record.AdditionalParameters = value;
                m_obstacles = value;
            }
        }

        public List<MapObstacle> Obstacles => m_mapObstacles.ToList();

        public override int StartExecute(Character character)
        {
            if (!Record.IsConditionFilled(character))
                return -1;

            var map = World.Instance.GetMap(MapId);

            map.GetMapObstacles();

            var interactive = map.GetInteractiveObject(ElementId);
            if (interactive == null)
                return -1;

            if (interactive.State != InteractiveStateEnum.STATE_NORMAL)
                return -1;

            interactive.SetInteractiveState(InteractiveStateEnum.STATE_ACTIVATED);

            Obstacles.ForEach(x => x.state = (sbyte)MapObstacleStateEnum.OBSTACLE_OPENED);
            InteractiveHandler.SendMapObstacleUpdatedMessage(map.Clients, Obstacles);

            foreach (var element in map.GetInteractiveObjects())
                InteractiveHandler.SendInteractiveElementUpdatedMessage(map.Clients, character, element);

            return 20000;
        }

        public override void EndExecute(Character character)
        {
            var map = World.Instance.GetMap(MapId);

            var interactive = map.GetInteractiveObject(ElementId);
            if (interactive == null)
                return;

            interactive.SetInteractiveState(InteractiveStateEnum.STATE_NORMAL);

            Obstacles.ForEach(x => x.state = (sbyte)MapObstacleStateEnum.OBSTACLE_CLOSED);
            InteractiveHandler.SendMapObstacleUpdatedMessage(map.Clients, Obstacles);

            foreach (var element in map.GetInteractiveObjects())
                InteractiveHandler.SendInteractiveElementUpdatedMessage(map.Clients, character, element);
        }
    }
}
