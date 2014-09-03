

// Generated on 09/02/2014 22:34:41
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SoundUi", "com.ankamagames.dofus.datacenter.sounds")]
    [Serializable]
    public class SoundUi : IDataObject, IIndexedData
    {
        public String MODULE = "SoundUi";
        public uint id;
        public String uiName;
        public String openFile;
        public String closeFile;
        public List<SoundUiElement> subElements;
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
        public String UiName
        {
            get { return this.uiName; }
            set { this.uiName = value; }
        }
        [D2OIgnore]
        public String OpenFile
        {
            get { return this.openFile; }
            set { this.openFile = value; }
        }
        [D2OIgnore]
        public String CloseFile
        {
            get { return this.closeFile; }
            set { this.closeFile = value; }
        }
        [D2OIgnore]
        public List<SoundUiElement> SubElements
        {
            get { return this.subElements; }
            set { this.subElements = value; }
        }
    }
}