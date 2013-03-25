
// Generated on 03/25/2013 19:24:38
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SoundUiHook")]
    [Serializable]
    public class SoundUiHook : IDataObject, IIndexedData
    {
        public uint id;
        public String name;
        public String MODULE = "SoundUiHook";

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

    }
}