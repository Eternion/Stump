using System;
using Castle.ActiveRecord;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Database.Interfaces;

namespace Stump.Server.AuthServer.Database
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