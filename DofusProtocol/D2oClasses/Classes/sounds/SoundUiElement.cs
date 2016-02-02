

// Generated on 02/02/2016 14:15:07
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
            get { return this.id; }
            set { this.id = value; }
        }
        [D2OIgnore]
        public String Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        [D2OIgnore]
        public uint HookId
        {
            get { return this.hookId; }
            set { this.hookId = value; }
        }
        [D2OIgnore]
        public String File
        {
            get { return this.file; }
            set { this.file = value; }
        }
        [D2OIgnore]
        public uint Volume
        {
            get { return this.volume; }
            set { this.volume = value; }
        }
    }
}