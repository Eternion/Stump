using Stump.Server.WorldServer.Database.Jobs;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Jobs
{
    public class Job
    {
        public Job(Character owner, JobRecord record)
        {
            Owner = owner;
            Record = record;
            Template = JobManager.Instance.GetJobTemplate(record.TemplateId);
        }

        public Character Owner
        {
            get;
            private set;
        }

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
            get { return Record.Experience; }
            private set
            {
                Record.Experience = value;

                if (value >= UpperBoundExperience || value < LowerBoundExperience)
                    RefreshLevel();

                IsDirty = true;
            }
        }

        private void RefreshLevel()
        {
            var level = ExperienceManager.Instance.GetJobLevel(Experience);
            UpperBoundExperience = ExperienceManager.Instance.GetJobLevelExperience(level);
            UpperBoundExperience = ExperienceManager.Instance.GetJobNextLevelExperience(level);

            Level = level;
        }

        public void Save()
        {
        }
    }
}