using System.Collections.Generic;
using System.Drawing;
using Stump.BaseCore.Framework.Utils;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.World.Zones;

namespace Stump.Server.WorldServer.World
{
    public partial class World : Singleton<World>
    {

        private readonly Dictionary<int, SuperArea> m_superAreas = new Dictionary<int, SuperArea>();

        private readonly Dictionary<int, Area> m_areas = new Dictionary<int, Area>();

        private readonly Dictionary<int, SubArea> m_subAreas = new Dictionary<int, SubArea>();

        private readonly Dictionary<int, Map> m_maps = new Dictionary<int, Map>();

        private readonly Dictionary<Point, Map> m_indoorMapsByCoordinates = new Dictionary<Point, Map>();

        private readonly Dictionary<Point, Map> m_outdoorMapsByCoordinates = new Dictionary<Point, Map>();


        public SuperArea GetSuperArea(int id)
        {
            if (m_superAreas.ContainsKey(id))
                return m_superAreas[id];
            logger.Warn(string.Format("Try to access to unexistant SuperArea {{0}}", id));
            return null;
        }

        public Area GetArea(int id)
        {
            if (m_areas.ContainsKey(id))
                return m_areas[id];
            logger.Warn(string.Format("Try to access to unexistant Area {{0}}", id));
            return null;
        }

        public SubArea GetSubArea(int id)
        {
            if (m_subAreas.ContainsKey(id))
                return m_subAreas[id];
            logger.Warn(string.Format("Try to access to unexistant SubArea {{0}}", id));
            return null;
        }

        public Map GetMap(int id)
        {
            if (m_maps.ContainsKey(id))
                return m_maps[id];
            logger.Warn(string.Format("Try to access to unexistant Map {{0}}", id));
            return null;
        }

        public Map GetMap(int x, int y, MapTypeEnum type)
        {
            var p = new Point(x, y);
            if (type == MapTypeEnum.OUTDOOR)
            {
                if (m_outdoorMapsByCoordinates.ContainsKey(p))
                    return m_outdoorMapsByCoordinates[p];
            }
            else
            {
                if (m_indoorMapsByCoordinates.ContainsKey(p))
                    return m_indoorMapsByCoordinates[p];
            }
            logger.Warn(string.Format("Try to access to unexistant Map {{0}}", p.ToString()));
            return null;
        }
    }
}