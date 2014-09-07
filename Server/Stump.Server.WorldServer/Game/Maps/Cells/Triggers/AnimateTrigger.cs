using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.World.Triggers;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Interactives;

namespace Stump.Server.WorldServer.Game.Maps.Cells.Triggers
{
    [Discriminator("Animate", typeof(CellTrigger), typeof(CellTriggerRecord))]
    public class AnimateTrigger : CellTrigger
    {
        public AnimateTrigger(CellTriggerRecord record)
            : base(record)
        {
        }

        private int? m_elementId;
        private short? m_cellId;
        private int? m_mapId;
        private string m_obstacles;
 
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

        public string Obstacles
        {
            get { return m_obstacles ?? (m_obstacles = Record.AdditionalParameters); }
            set
            {
                Record.AdditionalParameters = value;
                m_obstacles = value;
            }
        }

        public override void Apply(Character character)
        {
            var map = World.Instance.GetMap(MapId);
            InteractiveHandler.SendStatedElementUpdatedMessage(map.Clients, ElementId, CellId, 1);

            map.Area.CallDelayed(20000, Reset);
        }

        public void Reset()
        {
            var map = World.Instance.GetMap(MapId);
            InteractiveHandler.SendStatedElementUpdatedMessage(map.Clients, ElementId, CellId, 0);
        }
    }
}
