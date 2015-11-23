using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Jobs;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

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
            RefreshLevel();
        }

        public Job(Character owner, JobRecord record)
        {
            Owner = owner;
            Record = record;
            Template = JobManager.Instance.GetJobTemplate(record.TemplateId);
            RefreshLevel();
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
            private set
            {
                CheckRecordExists();
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

        public void Save(ORM.Database database)
        {
            if (IsNew)
                database.Insert(Record);
            else if (IsDirty)
                database.Update(Record);
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

        public JobExperience GetJobExperience()
            => new JobExperience((sbyte)Template.Id, (byte)Level, Experience, LowerBoundExperience, UpperBoundExperience);
    }
}