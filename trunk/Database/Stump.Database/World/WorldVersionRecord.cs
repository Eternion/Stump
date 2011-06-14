using System;
using Castle.ActiveRecord;
using Stump.Database.Types;

namespace Stump.Database.WorldServer
{
    public class WorldVersionRecord : WorldBaseRecord<WorldVersionRecord>, IVersionRecord
    {
        [PrimaryKey(PrimaryKeyType.Assigned, "Revision")]
        public uint Revision
        {
            get;
            set;
        }

        [Property("UpdateDate")]
        public DateTime UpdateDate
        {
            get;
            set;
        }
    }
}