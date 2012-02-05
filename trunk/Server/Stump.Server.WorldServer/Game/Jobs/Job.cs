using Stump.Server.WorldServer.Database.Jobs;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Jobs
{
    public class Job
    {
        public Character Owner
        {
            get;
            private set;
        }

        internal JobRecord Record
        {
            get;
            private set;
        }

        public JobTemplate Template
        {
            get;
            private set;
        }

        public int Level
        {
            get;
            private set;
        }

        public long Experience
        {
            get { return Record.Experience; }
            private set { Record.Experience = value; }
        }


    }
}