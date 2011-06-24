
using System.Collections.Generic;

namespace Stump.Server.DataProvider.Data.Job
{
    public class JobTemplate
    {
        public int JobId { get; set; }

        public List<int> JobToolIds { get; set; }

        public int SpecialisationOfId { get; set; }

        public List<JobSkill> Skills { get; set; }
    }
}