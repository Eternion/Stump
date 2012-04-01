using Castle.ActiveRecord;

namespace Stump.Server.WorldServer.Database.Jobs
{
    [ActiveRecord("characters_jobs")]
    public class JobRecord : AssignedWorldRecord<JobRecord>
    {
        [Property("JobId")]
        public int JobId
        {
            get;
            set;
        }

        [Property("Owner")]
        public int OwnerId
        {
            get;
            set;
        }

        [Property("Experience")]
        public long Experience
        {
            get;
            set;
        }
    }
}