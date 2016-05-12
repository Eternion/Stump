using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Extensions;
using Stump.Core.Pool;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Game.Maps.Cells.Triggers;

namespace Stump.Server.WorldServer.Game.Interactives
{
    public class InteractiveManager : DataManager<InteractiveManager>
    {
        public const int DEFAULT_INTERACTIVE_TEMPLATE = 129;
        public const int DEFAULT_SKILL_TEMPLATE = 184;

        UniqueIdProvider m_idProviderSpawn = new UniqueIdProvider();
        UniqueIdProvider m_idProviderSkill = new UniqueIdProvider();
        Dictionary<int, InteractiveSpawn> m_interactivesSpawns;
        Dictionary<int, InteractiveTemplate> m_interactivesTemplates;
        Dictionary<int, InteractiveSkillTemplate> m_skillsTemplates;
        Dictionary<int, InteractiveCustomSkillRecord> m_interactivesCustomSkills;
        InteractiveSkillTemplate m_defaultSkillTemplate;

        public IReadOnlyDictionary<int, InteractiveTemplate> InteractivesTemplates => m_interactivesTemplates;
        public IReadOnlyDictionary<int, InteractiveSpawn> InteractivesSpawns => m_interactivesSpawns;
        public IReadOnlyDictionary<int, InteractiveSkillTemplate> SkillsTemplates => m_skillsTemplates;

        [Initialization(InitializationPass.Fourth)]
        public override void Initialize()
        {
            m_interactivesTemplates = Database.Query<InteractiveTemplate, InteractiveSkillTemplate, InteractiveTemplateSkills, InteractiveCustomSkillRecord, InteractiveTemplate>
                (new InteractiveTemplateRelator().Map, InteractiveTemplateRelator.FetchQuery).ToDictionary(entry => entry.Id);

            m_interactivesSpawns = Database.Query<InteractiveSpawn, InteractiveSpawnSkills, InteractiveCustomSkillRecord, InteractiveSpawn>
                (new InteractiveSpawnRelator().Map, InteractiveSpawnRelator.FetchQuery).ToDictionary(entry => entry.Id);
            m_skillsTemplates = m_interactivesTemplates.SelectMany(x => x.Value.TemplateSkills).DistinctBy(x => x.Id).ToDictionary(entry => entry.Id);
            m_interactivesCustomSkills =
                Database.Query<InteractiveCustomSkillRecord>(InteractiveSkillRelator.FetchQuery).ToDictionary(entry => entry.Id);

            m_idProviderSpawn = m_interactivesSpawns.Any()
                ? new UniqueIdProvider(m_interactivesSpawns.Select(x => x.Value.Id).Max())
                : new UniqueIdProvider(0);
            m_idProviderSkill = m_interactivesCustomSkills.Any()
                ? new UniqueIdProvider(m_interactivesCustomSkills.Select(x => x.Value.Id).Max())
                : new UniqueIdProvider(0);

            GenerateInteractivePlot();
        }

        void GenerateInteractivePlot()
        {
            foreach (var trigger in CellTriggerManager.Instance.GetCellTriggers())
            {
                if (trigger.Type != "Teleport")
                    continue;

                var interactive = m_interactivesSpawns.Values.FirstOrDefault(x => x.MapId == trigger.MapId && x.CellId == trigger.CellId
                                                            && x.TemplateId == null && !x.CustomSkills.Any());

                if (interactive == null)
                    continue;

                var skillId = m_idProviderSkill.Pop();
                var skill = new InteractiveCustomSkillRecord
                {
                    Id = skillId,
                    Type = "Teleport",
                    CustomTemplateId = 339,
                    Parameter0 = trigger.Parameter1, //MapId
                    Parameter1 = trigger.Parameter0, //CellId
                    Condition = trigger.Condition
                };

                interactive.CustomSkills.Add(skill);
            }
        }

        public int PopSkillId()
        {
            return m_idProviderSkill.Pop();
        }

        public int PopSpawnId()
        {
            return m_idProviderSpawn.Pop();
        }

        public void FreeSkillId(int id)
        {
            m_idProviderSkill.Push(id);
        }

        public IEnumerable<InteractiveSpawn> GetInteractiveSpawns()
        {
            return m_interactivesSpawns.Values;
        }

        public InteractiveSpawn GetOneSpawn(Predicate<InteractiveSpawn> predicate)
        {
            return m_interactivesSpawns.Values.SingleOrDefault(entry => predicate(entry));
        }

        public InteractiveTemplate GetTemplate(int id)
        {
            InteractiveTemplate template;
            return m_interactivesTemplates.TryGetValue(id, out template) ? template : template;
        }

        public InteractiveSkillTemplate GetSkillTemplate(int id)
        {
            InteractiveSkillTemplate template;
            return m_skillsTemplates.TryGetValue(id, out template) ? template : GetDefaultSkillTemplate();
        }

        public void AddInteractiveSpawn(InteractiveSpawn spawn, InteractiveCustomSkillRecord skill, InteractiveSpawnSkills spawnSkill)
        {
            Database.Insert(spawn);
            Database.Insert(skill);
            Database.Insert(spawnSkill);

            m_interactivesSpawns.Add(spawn.Id, spawn);

            spawn.GetMap().SpawnInteractive(spawn);
        }

        public void RemoveInteractiveSpawn(InteractiveSpawn spawn)
        {
            var skills = spawn.GetSkills();

            foreach (var skill in skills)
            {
                Database.Delete(skill);
                Database.Delete("interactives_spawns_skills", "SkillId", skill.Id);
            }
            var map = spawn.GetMap();
            foreach (var io in map.GetInteractiveObjects().Where(x => x.Spawn == spawn).ToArray())
            {
                map.UnSpawnInteractive(io);
            }

            Database.Delete(spawn);
            m_interactivesSpawns.Remove(spawn.Id);
        }

        public InteractiveSkillTemplate GetDefaultSkillTemplate()
            => m_defaultSkillTemplate ?? (m_defaultSkillTemplate = GetSkillTemplate(DEFAULT_SKILL_TEMPLATE));
    }
}