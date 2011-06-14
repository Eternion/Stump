
using System;
using Castle.ActiveRecord;
using Stump.Database.Types;

namespace Stump.Database.AuthServer
{
    [ActiveRecord("version")]
    public class AuthVersionRecord : AuthBaseRecord<AuthVersionRecord>, IVersionRecord
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