using System;
using Castle.ActiveRecord;
using Stump.Server.AuthServer.Database.World;

namespace Stump.Server.AuthServer.Database.Account
{
    [ActiveRecord("connections")]
    public class ConnectionLog : AuthBaseRecord<ConnectionLog>
    {
        [PrimaryKey(PrimaryKeyType.Native, "Id")]
        public uint Id
        {
            get;
            set;
        }

        [BelongsTo("Account")]
        public Account Account
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

        [Property("Ip", NotNull = false, Length = 28)]
        public string Ip
        {
            get;
            set;
        }

        [BelongsTo("World")]
        public WorldServer World
        {
            get;
            set;
        }

        [Version]
        public int RecordVersion
        {
            get;
            set;
        }
    }
}