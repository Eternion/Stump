using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Idols;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using System.Collections.Generic;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Idols
{
    public class IdolManager : DataManager<IdolManager>
    {
        private Dictionary<int, IdolTemplate> m_idolTemplates;

        [Initialization(InitializationPass.Fourth)]
        public override void Initialize()
        {
            m_idolTemplates = Database.Query<IdolTemplate>(IdolTemplateRelator.FetchQuery).ToDictionary(entry => entry.Id);
        }

        public IdolTemplate[] GetTemplates()
        {
            return m_idolTemplates.Values.ToArray();
        }

        public IdolTemplate GetTemplate(int id)
        {
            return !m_idolTemplates.TryGetValue(id, out var result) ? null : result;
        }

        public IdolTemplate GetTemplateByItemId(int itemId)
        {
            return m_idolTemplates.FirstOrDefault(x => x.Value.IdolItemId == itemId).Value;
        }

        public PlayerIdol CreatePlayerIdol(Character owner, int idolId)
        {
            var template = GetTemplate(idolId);

            if (template == null)
                return null;

            return new PlayerIdol(owner, template);
        }
    }
}
