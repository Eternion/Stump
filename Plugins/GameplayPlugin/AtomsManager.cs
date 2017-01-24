using System.Collections.Generic;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.ORM;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Items;
using Stump.DofusProtocol.Enums;

namespace GameplayPlugin
{
    public class AtomsManager : DataManager<AtomsManager>
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Variable]
        public static short AtomItemTemplateId = 20000;

        public static ItemTemplate AtomItemTemplate;
        private static List<AtomDropper> m_records;

        [Initialization(typeof(ItemManager))]
        public override void Initialize()
        {
            AtomItemTemplate = ItemManager.Instance.TryGetTemplate(AtomItemTemplateId);

            if (AtomItemTemplate == null)
                logger.Error("Atom item template {0} doesn't exist in database !", AtomItemTemplateId);
            else
            {
                m_records = Database.Fetch<AtomDropper>("SELECT * FROM plugin_atom_droppers");
                FightManager.Instance.EntityAdded += OnFightCreated;
            }
        }

        private static void OnFightCreated(IFight fight)
        {
            if (fight is FightPvM)
            {
                fight.GeneratingResults += OnGeneratingResults;
            }
        }
        
        private static void OnGeneratingResults(IFight fight)
        {
            if (fight.Draw || fight.Winners.TeamType != TeamTypeEnum.TEAM_TYPE_PLAYER)
                return;

            var monsters = fight.GetAllFighters<MonsterFighter>(entry => entry.IsDead()).ToList();
            var players = fight.GetAllFighters<CharacterFighter>().ToList();

            var atoms = m_records.Where(x => monsters.Any(y => y.Monster.Template.Id == x.MonsterId)).Sum(x => x.Amount) / players.Count;

            if (atoms <= 0)
                return;

            foreach (var player in players)
            {
                player.Loot.AddItem(new DroppedItem(AtomItemTemplateId, (uint)atoms));
            }
            
        }

    }
}