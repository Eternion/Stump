using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.Global
{
    class PathElement
    {
        private MapPoint m_step;
        public MapPoint Step
        {
            get { return m_step; }
            set { m_step = value; }
        }

        private DirectionsEnum m_orientation;
        public DirectionsEnum Orientation
        {
            get { return m_orientation; }
            set { m_orientation = value; }
        }
    }
}
