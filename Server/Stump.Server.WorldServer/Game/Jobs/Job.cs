using System;
using System.Linq;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Database.Jobs;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Context.RolePlay;
using Stump.DofusProtocol.Enums;

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

        private bool IsDirty
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

        public bool IsIndexed
        {
            get;
            set;
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

            if (oldLevel != Level)
            {
                OnLevelChanged(oldLevel, Level);
            }
        }

        private void OnLevelChanged(int lastLevel, int newLevel)
        {
            ContextRoleplayHandler.SendJobLevelUpMessage(Owner.Client, this);
            var sumLevel = Owner.Jobs.Sum(x => x.Level);
            var diff = newLevel - lastLevel;

            var weightBonus = JobManager.Instance.GetWeightBonus(sumLevel - diff, sumLevel);
            Owner.Stats[PlayerFields.Weight].Base += weightBonus;
        }

        public int GetCraftXp(RecipeRecord recipe, int amount)
        {
            if (Id == (int)JobEnum.BASE)
                return 0;

            var level = Level;

            if (level == 200)
                return GetXpPerLevel(recipe, Level) * amount;

            double upperBoundXp = UpperBoundExperience;
            double currentXp = Experience;
            double xp = 0;
            while (amount > 0)
            {
                if (level - JobManager.MAX_JOB_LEVEL_GAP > recipe.ItemTemplate.Level)
                    break;

                var xpPerLevel = GetXpPerLevel(recipe, level);

                var amountBeforeLevelUp = (int)Math.Min(amount, Math.Ceiling((upperBoundXp - currentXp) /xpPerLevel));
                amount -= amountBeforeLevelUp;
                xp += xpPerLevel*amountBeforeLevelUp;
                currentXp += xpPerLevel*amountBeforeLevelUp;

                if (currentXp >= upperBoundXp)
                {
                    level++;
                    upperBoundXp = ExperienceManager.Instance.GetCharacterNextLevelExperience((byte) level);
                }
            }

            return (int)Math.Floor(xp);
        }

        private int GetXpPerLevel(RecipeRecord recipe, int level)
        {
            if (level - JobManager.MAX_JOB_LEVEL_GAP > recipe.ItemTemplate.Level)
                return 0;

            var xpPerLevel = 20d * recipe.ItemTemplate.Level / (Math.Pow((level - recipe.ItemTemplate.Level), 1.1) / 10 + 1);

            if (recipe.ItemTemplate.CraftXpRatio > -1)
                xpPerLevel *= recipe.ItemTemplate.CraftXpRatio / 100d;
            else if (recipe.ItemTemplate.Type.CraftXpRatio > -1)
                xpPerLevel *= recipe.ItemTemplate.Type.CraftXpRatio / 100d;

            xpPerLevel *= Rates.JobXpRate;
            xpPerLevel = Math.Floor(xpPerLevel);

            return (int)xpPerLevel;
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

        public JobCrafterDirectoryListEntry GetJobCrafterDirectoryListEntry()
            =>
                new JobCrafterDirectoryListEntry(
                    new JobCrafterDirectoryEntryPlayerInfo(Owner.Id, Owner.Name, (sbyte) Owner.AlignmentSide, (sbyte) Owner.Breed.Id, Owner.Sex != SexTypeEnum.SEX_FEMALE, Owner.Map.AvailableJobs.Contains(Template),
                        (short) Owner.Map.Position.X, (short) Owner.Map.Position.Y, Owner.Map.Id, (short) Owner.SubArea.Id, Owner.Status),
                    new JobCrafterDirectoryEntryJobInfo((sbyte) Template.Id, (byte) Level, WorkForFree, (byte) MinLevelCraftSetting));
    }
}