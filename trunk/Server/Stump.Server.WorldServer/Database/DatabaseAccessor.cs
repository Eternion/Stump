using System.Data.Entity;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.Items;
using Stump.Server.WorldServer.Database.Maps;
using Stump.Server.WorldServer.Database.Triggers;
using Stump.Server.WorldServer.Database.World;

namespace Stump.Server.WorldServer.Database
{
    public class DatabaseAccessor : BaseContext
    {
        public DbSet<WorldAccount> Accounts
        {
            get;
            set;
        }

        public DbSet<AreaRecord> Areas
        {
            get;
            set;
        }

        public DbSet<Breed> Breeds
        {
            get;
            set;
        }

        public DbSet<BreedItem> BreedItems
        {
            get;
            set;
        }

        public DbSet<BreedSpell> BreedSpells
        {
            get;
            set;
        }

        public DbSet<CharacterRecord> Characters
        {
            get;
            set;
        }

        public DbSet<JobRecord> CharacterJobs
        {
            get;
            set;
        }

        public DbSet<CharacterSpell> CharacterSpells
        {
            get;
            set;
        }

        public DbSet<EffectTemplate> Effects
        {
            get;
            set;
        }

        public DbSet<ExperienceTableEntry> ExperiencesEntries
        {
            get;
            set;
        }

        public DbSet<HintRecord> Hints
        {
            get;
            set;
        }

        public DbSet<InteractiveTemplate> Interactives
        {
            get;
            set;
        }

        public DbSet<InteractiveSkillRecord> InteractivesSkills
        {
            get;
            set;
        }

        public DbSet<InteractiveSkillTemplate> InteractivesSkillsTemplates
        {
            get;
            set;
        }

        public DbSet<InteractiveSpawn> InteractivesSpawns
        {
            get;
            set;
        }

        public DbSet<ItemRecord> Items
        {
            get;
            set;
        }

        public DbSet<ItemToSell> ItemsToSell
        {
            get;
            set;
        }

        public DbSet<ItemSetTemplate> ItemsSets
        {
            get;
            set;
        }

        public DbSet<ItemTemplate> ItemsTemplates
        {
            get;
            set;
        }

        public DbSet<ItemTypeRecord> ItemsTypes
        {
            get;
            set;
        }

        public DbSet<JobTemplate> Jobs
        {
            get;
            set;
        }

        public DbSet<MapRecord> Maps
        {
            get;
            set;
        }

        public DbSet<CellTriggerRecord> MapsCellsTriggers
        {
            get;
            set;
        }

        public DbSet<MapPositionRecord> MapsPositions
        {
            get;
            set;
        }

        public DbSet<MapReferenceRecord> MapsReferences
        {
            get;
            set;
        }

        public DbSet<MonsterTemplate> Monsters
        {
            get;
            set;
        }

        public DbSet<DroppableItem> MonstersDrops
        {
            get;
            set;
        }

        public DbSet<MonsterGrade> MonstersGrades
        {
            get;
            set;
        }

        public DbSet<MonsterRace> MonstersRaces
        {
            get;
            set;
        }

        public DbSet<MonsterSpawn> MonstersSpawns
        {
            get;
            set;
        }

        public DbSet<MonsterDungeonSpawn> MonstersDungeonsSpawns
        {
            get;
            set;
        }

        public DbSet<MonsterSpell> MonstersSpells
        {
            get;
            set;
        }

        public DbSet<MonsterSuperRace> MonstersSuperRaces
        {
            get;
            set;
        }

        public DbSet<NpcTemplate> Npcs
        {
            get;
            set;
        }

        public DbSet<NpcAction> NpcsActions
        {
            get;
            set;
        }

        public DbSet<NpcMessage> NpcsMessages
        {
            get;
            set;
        }

        public DbSet<NpcReply> NpcsReplies
        {
            get;
            set;
        }

        public DbSet<NpcSpawn> NpcSpawns
        {
            get;
            set;
        }

        public DbSet<Shortcut> Shortcuts
        {
            get;
            set;
        }

        public DbSet<InteractiveSkillTemplate> InteractiveSkillsTemplates
        {
            get;
            set;
        }

        public DbSet<SpellTemplate> Spells
        {
            get;
            set;
        }

        public DbSet<SpellBombTemplate> SpellsBomb
        {
            get;
            set;
        }

        public DbSet<SpellLevelTemplate> SpellsLevels
        {
            get;
            set;
        }

        public DbSet<SpellState> SpellsStates
        {
            get;
            set;
        }

        public DbSet<SpellType> SpellsTypes
        {
            get;
            set;
        }

        public DbSet<StartupActionRecord> StartupActions
        {
            get;
            set;
        }

        public DbSet<StartupActionItemRecord> StartupActionsObjects
        {
            get;
            set;
        }

        public DbSet<SubAreaRecord> SubAreas
        {
            get;
            set;
        }

        public DbSet<SuperAreaRecord> SuperAreas
        {
            get;
            set;
        }

        public DbSet<LangText> Texts
        {
            get;
            set;
        }

        public DbSet<LangTextUi> TextsUI
        {
            get;
            set;
        }

        public DbSet<WorldMapRecord> WorldMaps
        {
            get;
            set;
        }
    }
}