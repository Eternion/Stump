

// Generated on 12/12/2013 16:57:43
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SoundUiElement", "com.ankamagames.dofus.datacenter.sounds")]
    [Serializable]
    public class SoundUiElement : IDataObject, IIndexedData
    {
        public String MODULE = "SoundUiElement";
        public uint id;
        public String name;
        public uint hookId;
        public String file;
        public uint volume;
        int IIndexedData.Id
        {
            get { return (int)id; }
        }
        [D2OIgnore]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        [D2OIgnore]
        public uint HookId
        {
            get { return hookId; }
            set { hookId = value; }
        }
        [D2OIgnore]
        public String File
        {
            get { return file; }
            set { file = value; }
        }
        [D2OIgnore]
        public uint Volume
        {
            get { return volume; }
            set { volume = value; }
        }
    }
}