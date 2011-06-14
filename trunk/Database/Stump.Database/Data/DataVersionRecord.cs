
using System;
using Castle.ActiveRecord;
using Stump.Database.AuthServer;
using Stump.Database.Types;

namespace Stump.Database.Data
{
    [ActiveRecord("version")]
    public class DataVersionRecord : DataBaseRecord<DataVersionRecord>, IVersionRecord
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