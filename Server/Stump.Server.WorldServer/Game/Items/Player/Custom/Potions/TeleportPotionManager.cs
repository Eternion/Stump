using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Items.Usables;

namespace Stump.Server.WorldServer.Game.Items.Player.Custom
{
    public class TeleportPotionManager : DataManager<TeleportPotionManager>
    {
        private Dictionary<int, TeleportPotionRecord> m_teleportRecords;

        [Initialization(typeof(ItemManager))]
        public override void Initialize()
        {
            m_teleportRecords = Database.Query<TeleportPotionRecord>(TeleportPotionRelator.FetchQuery).ToDictionary(x => x.ItemId);

            foreach(var record in m_teleportRecords.Values)
            {
                ItemManager.Instance.AddItemIdConstructor(typeof (TeleportPotion), (ItemIdEnum) record.ItemId);
            }
        }

        public TeleportPotionRecord GetRecord(int id)
        {
            TeleportPotionRecord record;
            return m_teleportRecords.TryGetValue(id, out record) ? record : null;
        }
    }
}