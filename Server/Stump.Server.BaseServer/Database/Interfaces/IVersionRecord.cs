using System;

namespace Stump.Server.BaseServer.Database.Interfaces
{
    public interface IVersionRecord
    {
        uint Revision
        {
            get;
            set;
        }

        DateTime UpdateDate
        {
            get;
            set;
        }
    }
}