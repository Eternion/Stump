using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Database.Interactives.Skills;
using Stump.Server.WorldServer.Worlds.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Worlds.Interactives.Skills;

namespace Stump.Server.WorldServer.Worlds.Interactives
{
    public class InteractiveObject
    {
        private readonly Dictionary<int, Skill> m_skills = new Dictionary<int, Skill>();

        public InteractiveObject(InteractiveSpawn spawn)
        {
            Spawn = spawn;

            GenerateSkills();
        }

        public InteractiveSpawn Spawn
        {
            get;
            private set;
        }

        public bool CanSelectSkill
        {
            get { return Template != null; }
        }

        public int Id
        {
            get { return Spawn.ElementId; }
        }

        /// <summary>
        /// Can be null
        /// </summary>
        public InteractiveTemplate Template
        {
            get { return Spawn.Template; }
        }

        private void GenerateSkills()
        {
            foreach (SkillTemplate skillTemplate in Spawn.GetSkills())
            {
                int id = InteractiveManager.Instance.PopSkillId();

                m_skills.Add(id, skillTemplate.GenerateSkill(id, this));
            }
        }

        public Skill GetSkill(int id)
        {
            Skill result;
            if (!m_skills.TryGetValue(id, out result))
                return null;

            return result;
        }

        public IEnumerable<Skill> GetSkills()
        {
            return m_skills.Values;
        }

        public IEnumerable<Skill> GetEnabledSkills(Character character)
        {
            return m_skills.Values.Where(entry => entry.IsEnabled(character));
        }

        public IEnumerable<Skill> GetDisabledSkills(Character character)
        {
            return m_skills.Values.Where(entry => !entry.IsEnabled(character));
        }

        public IEnumerable<InteractiveElementSkill> GetEnabledElementSkills(Character character)
        {
            return m_skills.Values.Where(entry => entry.IsEnabled(character)).Select(entry => entry.GetInteractiveElementSkill());
        }

        public IEnumerable<InteractiveElementSkill> GetDisabledElementSkills(Character character)
        {
            return m_skills.Values.Where(entry => !entry.IsEnabled(character)).Select(entry => entry.GetInteractiveElementSkill());
        }

        public InteractiveElement GetInteractiveElement(Character character)
        {
            return
                new InteractiveElement(Id, Template != null ? Template.Id : -1, GetEnabledElementSkills(character), GetDisabledElementSkills(character));
        }
    }
}