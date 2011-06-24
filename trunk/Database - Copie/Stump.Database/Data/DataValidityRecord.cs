using System;
using Castle.ActiveRecord;
using Stump.Database.Types;

namespace Stump.Database.Data
{
    public class DataValidityRecord : DataBaseRecord<DataValidityRecord>
    {
        [PrimaryKey(PrimaryKeyType.Assigned, "File")]
        public string File
        {
            get;
            set;
        }

        [Property("Hash")]
        public byte[] Hash
        {
            get;
            set;
        }
    }
}