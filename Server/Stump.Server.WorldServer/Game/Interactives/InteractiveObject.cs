using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Database.World.Maps;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Interactives.Skills;
using Stump.Server.WorldServer.Game.Maps;
using Stump.Server.WorldServer.Game.Maps.Cells;
using Stump.Server.WorldServer.Handlers.Interactives;
using System;

namespace Stump.Server.WorldServer.Game.Interactives
{
    public class InteractiveObject : WorldObject
    {
        private readonly Dictionary<int, Skill> m_skills = new Dictionary<int, Skill>();

        public InteractiveObject(Map map, InteractiveSpawn spawn)
        {
            Spawn = spawn;
            Position = new ObjectPosition(map, spawn.CellId);
            Animated = spawn.Animated;

            GenerateSkills();
        }

        public InteractiveSpawn Spawn
        {
            get;
            private set;
        }

        public bool Animated
        {
            get;
            private set;
        }

        public InteractiveStateEnum State
        {
            get;
            private set;
        }

        public override int Id
        {
            get { return Spawn.Id; }
            protected set { Spawn.Id = value; }
        }

        /// <summary>
        /// Can be null
        /// </summary>
        public InteractiveTemplate Template
        {
            get { return Spawn.Template; }
        }

        public void SetInteractiveState(InteractiveStateEnum state)
        {
            State = state;

            InteractiveHandler.SendStatedElementUpdatedMessage(Map.Clients, Id, Cell.Id, (int) State);
        }

        private void GenerateSkills()
        {
            foreach (var skillRecord in Spawn.GetSkills())
            {
                try
                {
                    var id = InteractiveManager.Instance.PopSkillId();
                    var skill = skillRecord.GenerateSkill(id, this);

                    m_skills.Add(id, skill);
                }
                catch (Exception ex)
                {
                    logger.Error($"Cannot generate skills of spawn {Spawn.Id} interactive ({Spawn.Template}) : {ex.Message}");
                }
            }
        }

        public Skill GetSkill(int id)
        {
            Skill result;
            return !m_skills.TryGetValue(id, out result) ? null : result;
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
            return m_skills.Values.Where(entry => entry.IsEnabled(character) && entry.SkillTemplate.ClientDisplay).Select(entry => entry.GetInteractiveElementSkill());
        }

        public IEnumerable<InteractiveElementSkill> GetDisabledElementSkills(Character character)
        {
            return m_skills.Values.Where(entry => !entry.IsEnabled(character) && entry.SkillTemplate.ClientDisplay).Select(entry => entry.GetInteractiveElementSkill());
        }

        public InteractiveElement GetInteractiveElement(Character character)
        {
            return
                new InteractiveElement(Id, Template?.Id ?? -1, GetEnabledElementSkills(character), GetDisabledElementSkills(character));
        }

        public StatedElement GetStatedElement()
        {
            return new StatedElement(Id, Cell.Id, (int) State);
        }
    }
}