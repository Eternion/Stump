using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Pool;
using Stump.Core.Reflection;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Database.World.Triggers;
using Stump.Server.WorldServer.Worlds.Interactives;

namespace Stump.Server.WorldServer.Worlds.Maps.Cells.Triggers
{
    public class CellTriggerManager : Singleton<CellTriggerManager>
    {
        private Dictionary<int, CellTriggerRecord> m_cellTriggers;

        [Initialization(InitializationPass.Fourth)]
        public void Initialize()
        {
            m_cellTriggers = CellTriggerRecord.FindAll().ToDictionary(entry => entry.Id);
        }

        public IEnumerable<CellTriggerRecord> GetCellTriggers()
        {
            return m_cellTriggers.Values;
        }

        public CellTriggerRecord GetOneCellTrigger(Predicate<CellTriggerRecord> predicate)
        {
            return m_cellTriggers.Values.Where(entry => predicate(entry)).FirstOrDefault();
        }

        public CellTriggerRecord GetCellTrigger(int id)
        {
            CellTriggerRecord cellTrigger;
            if (m_cellTriggers.TryGetValue(id, out cellTrigger))
                return cellTrigger;

            return cellTrigger;
        }

        public void AddCellTrigger(CellTriggerRecord cellTrigger)
        {
            cellTrigger.Save();

            m_cellTriggers.Add(cellTrigger.Id, cellTrigger);
        }
    }
}