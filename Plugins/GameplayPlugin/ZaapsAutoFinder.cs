using System.Linq;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Maps;

namespace GameplayPlugin
{
    public class ZaapsAutoFinder
    {
        private static Map[] m_zaaps;
        private const int ZAAP_TEMPLATE = 16;
        static ZaapsAutoFinder()
        {
            CharacterManager.Instance.CreatingCharacter += OnCreatingCharacter;
        }

        [Initialization(typeof(World))]
        public static void Initialize()
        {
            m_zaaps = InteractiveManager.Instance.GetInteractiveSpawns().Where(x => x.TemplateId != null && x.TemplateId == ZAAP_TEMPLATE).
                Select(x => World.Instance.GetMap(x.MapId)).Where(x => x != null).ToArray();
        }

        private static void OnCreatingCharacter(CharacterRecord record)
        {
            record.KnownZaaps = m_zaaps.Where(x => x != null).ToList();
            WorldServer.Instance.IOTaskPool.AddMessage(() => CharacterManager.Instance.Database.Update(record));
        }
    }
}