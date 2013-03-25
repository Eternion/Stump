
// Generated on 03/25/2013 19:24:38
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SoundUiElement")]
    [Serializable]
    public class SoundUiElement : IDataObject, IIndexedData
    {
        public uint id;
        public String name;
        public uint hookId;
        public String file;
        public uint volume;
        public String MODULE = "SoundUiElement";

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

        public uint HookId
        {
            get { return hookId; }
            set { hookId = value; }
        }

        public String File
        {
            get { return file; }
            set { file = value; }
        }

        public uint Volume
        {
            get { return volume; }
            set { volume = value; }
        }

    }
}