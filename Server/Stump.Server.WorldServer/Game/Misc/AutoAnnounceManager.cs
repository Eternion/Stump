using Stump.Core.Attributes;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Misc;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Misc
{
    public class AutoAnnounceManager : DataManager<AutoAnnounceManager>
    {
        [Variable]
        public static int AnnouncesDelaySeconds = 300;

        private Dictionary<int, AutoAnnounceMessage> m_announces;
        private int m_lastId;

        [Initialization(InitializationPass.Any)]
        public override void Initialize()
        {
            m_announces = Database.Query<AutoAnnounceMessage>(AutoAnnounceMessageRelator.FecthQuery).ToDictionary(x => x.Id);
            WorldServer.Instance.IOTaskPool.CallPeriodically(AnnouncesDelaySeconds * 1000, PromptNextAnnounce);
        }

        public int AddAnnounce(string message, Color? color = null)
        {
            var announce = new AutoAnnounceMessage { Color = color?.ToArgb(), Message = message };
            announce.AssignIdentifier();

            m_announces.Add(announce.Id, announce);

            WorldServer.Instance.IOTaskPool.ExecuteInContext(() => Database.Insert(announce));

            return announce.Id;
        }

        public bool RemoveAnnounce(int id)
        {
            if (m_announces.Remove(id))
            {
                WorldServer.Instance.IOTaskPool.ExecuteInContext(() => Database.Delete<AutoAnnounceMessage>(id));
                return true;
            }
            return false;
        }

        public void PromptNextAnnounce()
        {
            AutoAnnounceMessage announce = null;

            if (m_lastId >= m_announces.Keys.Max())
                announce = m_announces.Values.OrderBy(x => x.Id).FirstOrDefault();
            else
                announce = m_announces.Values.FirstOrDefault(x => x.Id > m_lastId);

            if (announce == null)
                return;

            SendAnnounce(announce);

            m_lastId = announce.Id;
        }

        private void SendAnnounce(AutoAnnounceMessage announce)
        {
            var color = announce.Color != null ? (Color?)Color.FromArgb(announce.Color.Value) : null;

            WorldServer.Instance.IOTaskPool.AddMessage(() =>
                World.Instance.ForEachCharacter(character =>
                {
                    var msg = character.IsGameMaster() ? $"(ANNOUNCE)[ID = {announce.Id}] {announce.Message}" : $"(ANNOUNCE) {announce.Message}";

                    if (color != null)
                        character.SendServerMessage(msg, color.Value);
                    else
                        character.SendServerMessage(msg);
                }));
        }
    }
}