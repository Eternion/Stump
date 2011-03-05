using System;
using Castle.ActiveRecord;

namespace Stump.Database
{
    public interface IVersionRecord
    {
        [PrimaryKey(PrimaryKeyType.Assigned, "Revision")]
        uint Revision { get; set; }

        [Property("UpdateDate")]
        DateTime UpdateDate { get; set; }

        void CreateAndFlush();
        void DeleteAndFlush();
    }
}