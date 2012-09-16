using System.Data.Entity.ModelConfiguration;
using Castle.ActiveRecord;

namespace Stump.Server.WorldServer.Database
{
    public class JobRecordConfiguration : EntityTypeConfiguration<JobRecord>
    {
        public JobRecordConfiguration()
        {
            ToTable("characters_jobs");
        }
    }
    public class JobRecord : AssignedWorldRecord<JobRecord>
    {
        public int JobId
        {
            get;
            set;
        }

        public int OwnerId
        {
            get;
            set;
        }

        public long Experience
        {
            get;
            set;
        }
    }
}