using System;
using Castle.ActiveRecord;

namespace Stump.Server.BaseServer.Database
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

        void CreateAndFlush();
        void DeleteAndFlush();
    }
}