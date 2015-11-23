using System.Collections;
using System.Collections.Generic;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Jobs;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Jobs
{
    public class JobManager : DataManager<JobManager>
    {
        private Dictionary<int, JobTemplate> m_jobTemplates;

        [Initialization(InitializationPass.Fifth)]
        public void Initialize()
        {
            m_jobTemplates = Database.Query<JobTemplate>(JobTemplateRelator.FetchQuery).ToDictionary(x => x.Id);
        }

        public JobTemplate GetJobTemplate(int id)
        {
            JobTemplate job;
            return m_jobTemplates.TryGetValue(id, out job) ? job : null;
        }

        public JobRecord[] GetCharacterJobs(int characterId)
        {
            return Database.Query<JobRecord>(string.Format(JobRecordRelator.FetchByOwner, characterId)).ToArray();
        }

        public JobTemplate[] GetJobTemplates() => m_jobTemplates.Values.ToArray();
        public IEnumerable<JobTemplate> EnumerateJobTemplates() => m_jobTemplates.Values;
    }
}