
using System;
using System.Collections.Generic;
using Stump.Database.Data.World;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global.Maps;
using Stump.Server.WorldServer.Global.WorldSpaces;

namespace Stump.Server.WorldServer.Global
{
    /// <summary>
    /// SubArea
    /// </summary>
    public class Zone
    {
        public Zone(SubAreaRecord record, Region parent)
        {
            Record = record;
            Parent = parent;
            Parent.Childrens.Add(this);
        }

        public Region Parent
        {
            get;
            private set;
        }

        public List<Map> Childrens
        {
            get;
            internal set;
        }

        #region Properties

        public SubAreaRecord Record
        {
            get;
            set;
        }
        #endregion
    }
}