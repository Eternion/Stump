
// Generated on 03/02/2013 21:17:47
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Dungeons")]
    [Serializable]
    public class Dungeon : IDataObject, IIndexedData
    {
        private const String MODULE = "Dungeons";
        public int id;
        public uint nameId;
        public int optimalPlayerLevel;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public int OptimalPlayerLevel
        {
            get { return optimalPlayerLevel; }
            set { optimalPlayerLevel = value; }
        }

    }
}