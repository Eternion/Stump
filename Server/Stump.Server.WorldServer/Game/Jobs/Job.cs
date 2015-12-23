using System;
using System.Linq;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Database.Jobs;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;

namespace Stump.Server.WorldServer.Game.Jobs
{
    public class Job
    {
        /// <summary>
        /// Instantiate a job without record, experience equals zero
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="template"></param>
        public Job(Character owner, JobTemplate template)
        {
            Owner = owner;
            Record = null;
            Template = template;

            var level = ExperienceManager.Instance.GetJobLevel(Experience);
            LowerBoundExperience = ExperienceManager.Instance.GetJobLevelExperience(level);
            UpperBoundExperience = ExperienceManager.Instance.GetJobNextLevelExperience(level);
            Level = level;
        }

        public Job(Character owner, JobRecord record)
        {
            Owner = owner;
            Record = record;
            Template = JobManager.Instance.GetJobTemplate(record.TemplateId);

            var level = ExperienceManager.Instance.GetJobLevel(Experience);
            LowerBoundExperience = ExperienceManager.Instance.GetJobLevelExperience(level);
            UpperBoundExperience = ExperienceManager.Instance.GetJobNextLevelExperience(level);
            Level = level;
        }

        public int Id => Template.Id;

        public Character Owner
        {
            get;
            private set;
        }

        // may be null
        private JobRecord Record
        {
            get;
            set;
        }

        public JobTemplate Template
        {
            get;
            private set;
        }

        public bool IsDirty
        {
            get;
            set;
        }

        private bool IsNew
        {
            get;
            set;
        }

        public int Level
        {
            get;
            private set;
        }

        public long UpperBoundExperience
        {
            get;
            private set;
        }

        public long LowerBoundExperience
        {
            get;
            private set;
        }

        public long Experience
        {
            get { return Record?.Experience ?? 0; }
            set
            {
                if (value < 0)
                    throw new ArgumentException();

                CheckRecordExists();
                Record.Experience = value;

                if (value >= UpperBoundExperience || value < LowerBoundExperience)
                    RefreshLevel();

                IsDirty = true;
                ContextRoleplayHandler.SendJobExperienceUpdateMessage(Owner.Client, this);
            }
        }

        public bool WorkForFree
        {
            get { return Record?.WorkForFree ?? false; }
            set
            {
                CheckRecordExists();
                Record.WorkForFree = value;
            }
        }

        public int MinLevelCraftSetting
        {
            get { return Record?.MinLevelCraftSetting ?? 1; }
            set
            {
                if (value < 1 || value > 200)
                    throw new ArgumentException();

                CheckRecordExists();

                Record.MinLevelCraftSetting = value;

            }
        }

        private void RefreshLevel()
        {
            var level = ExperienceManager.Instance.GetJobLevel(Experience);
            LowerBoundExperience = ExperienceManager.Instance.GetJobLevelExperience(level);
            UpperBoundExperience = ExperienceManager.Instance.GetJobNextLevelExperience(level);

            var oldLevel = Level;
            Level = level;

            if (oldLevel < Level)
            {
                ContextRoleplayHandler.SendJobLevelUpMessage(Owner.Client, this);
            }
        }

        public void Save(ORM.Database database)
        {
            if (IsNew)
                database.Insert(Record);
            else if (IsDirty)
                database.Update(Record);

            IsNew = IsDirty = false;
        }

        private bool CheckRecordExists()
        {
            if (Record == null)
            {
                Record = new JobRecord() {OwnerId = Owner.Id, TemplateId = Template.Id};
                IsNew = true;
            }

            return IsNew;
        }
        
        
        private SkillActionDescription GetSkillActionDescription(InteractiveSkillTemplate skill)
        {
            if (skill.GatheredRessourceItem > 0)
            {
                var minMax = JobManager.Instance.GetHarvestItemMinMax(Template, Level, skill);
                return new SkillActionDescriptionCollect((short) skill.Id, 0, (short) minMax.First, (short) minMax.Second);
            }
            else if (skill.CraftableItemIds.Length > 0)
                return new SkillActionDescriptionCraft((short) skill.Id, 0);

            return new SkillActionDescription((short) skill.Id);
        }

        public JobExperience GetJobExperience()
            => new JobExperience((sbyte)Template.Id, (byte)Level, Experience, LowerBoundExperience, UpperBoundExperience);

        public JobDescription GetJobDescription()
            => new JobDescription((sbyte) Template.Id, Template.Skills.Where(x => x.LevelMin <= Level).Select(x => GetSkillActionDescription(x)));

        public JobCrafterDirectorySettings GetJobCrafterDirectorySettings()
            => new JobCrafterDirectorySettings((sbyte) Template.Id, (byte)MinLevelCraftSetting, WorkForFree);
    }
}