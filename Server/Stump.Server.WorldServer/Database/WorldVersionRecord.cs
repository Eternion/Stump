using System;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using Stump.Server.BaseServer.Database.Interfaces;

namespace Stump.Server.WorldServer.Database
{
    [TableName("version")]
    public class VersionRecord : IVersionRecord
    {
        public string DofusVersion
        {
            get;
            set;
        }

        #region IVersionRecord Members

        [PrimaryKey("Revision")]
        public uint Revision
        {
            get;
            set;
        }
        public DateTime UpdateDate
        {
            get;
            set;
        }

        #endregion
    }
}