using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Jobs
{
    public class JobsCollection : IEnumerable<Job>
    {
        private Dictionary<int, Job> m_jobs = new Dictionary<int, Job>(); 

        public JobsCollection(Character owner)
        {
            Owner = owner;
        }

        public Character Owner
        {
            get;
            private set;
        }

        public void LoadJobs()
        {
            m_jobs = JobManager.Instance.GetCharacterJobs(Owner.Id).ToDictionary(x => x.TemplateId, x => new Job(Owner, x));

            // in order to free space we avoid storing job record with no experience in databse
            foreach(var job in JobManager.Instance.EnumerateJobTemplates().Where(x => !m_jobs.ContainsKey(x.Id)).ToArray())
                m_jobs.Add(job.Id, new Job(Owner, job));
        }

        public Job this[int templateId] => m_jobs[templateId];

        public IEnumerator<Job> GetEnumerator() => m_jobs.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}