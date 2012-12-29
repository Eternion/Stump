using System;
using Castle.ActiveRecord;
using Stump.Server.BaseServer.Database.Interfaces;

namespace Stump.Server.WorldServer.Database
{
    [ActiveRecord("version")]
    public class WorldVersionRecord : WorldBaseRecord<WorldVersionRecord>, IVersionRecord
    {
        [Property]
        public string DofusVersion
        {
            get;
            set;
        }

        #region IVersionRecord Members

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

        #endregion
    }
}