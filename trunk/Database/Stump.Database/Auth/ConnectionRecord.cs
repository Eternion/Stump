
using System;
using Castle.ActiveRecord;
using NHibernate.Criterion;
using Stump.Database.AuthServer.World;
using Stump.Database.Types;

namespace Stump.Database.AuthServer
{
    [ActiveRecord("connections")]
    public class ConnectionRecord : AuthBaseRecord<ConnectionRecord>
    {
        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public uint Id
        {
            get;
            set;
        }

        [BelongsTo("Account")]
        public AccountRecord Account
        {
            get;
            set;
        }

        [Property("Date")]
        public DateTime Date
        {
            get;
            set;
        }

        [Property("Ip", NotNull = false, Length = 15)]
        public string Ip
        {
            get;
            set;
        }

        [BelongsTo("World")]
        public WorldRecord World
        {
            get;
            set;
        }

    }
}