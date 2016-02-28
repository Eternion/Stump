using System.Linq;
using Stump.Core.IO;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Database.Jobs;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Dialogs.Jobs;
using Stump.Server.WorldServer.Game.Jobs;

namespace Stump.Server.WorldServer.Game.Interactives.Skills
{
    [Discriminator("JobBook", typeof(Skill), typeof(int), typeof(InteractiveCustomSkillRecord), typeof(InteractiveObject))]
    public class SkillOpenJobBook : CustomSkill
    {
        public SkillOpenJobBook(int id, InteractiveCustomSkillRecord record, InteractiveObject interactiveObject)
            : base(id, record, interactiveObject)
        {
        }

        private string m_jobsCSV;
        private JobTemplate[] m_jobs;
        /// <summary>
        /// Parameter 0
        /// </summary>
        private string JobsCSV
        {
            get
            {
                return m_jobsCSV ?? (m_jobsCSV = Record.GetParameter<string>(0));
            }
            set
            {
                Record.SetParameter(0, value);
                m_jobsCSV = value;
                m_jobs = value.FromCSV<int>(",").Select(x => JobManager.Instance.GetJobTemplate(x)).ToArray();
            }
        }

        public JobTemplate[] Jobs
        {
            get
            {
                return m_jobs ?? (m_jobs = JobsCSV.FromCSV<int>(",").Select(x => JobManager.Instance.GetJobTemplate(x)).ToArray());
            }
            set
            {
                m_jobs = value;
                m_jobsCSV = value.ToCSV(",");
            }
        }

        public override int StartExecute(Character character)
        {
            var dialog = new JobIndexDialog(character, Jobs);
            dialog.Open();

            return base.StartExecute(character);
        }
    }
}