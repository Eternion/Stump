

// Generated on 10/06/2013 17:58:56
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
        public uint id;
        public String uiName;
        public String openFile;
        public String closeFile;
        public List<SoundUiElement> subElements;
        public String MODULE = "SoundUi";
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
        public String UiName
        {
            get { return uiName; }
            set { uiName = value; }
        }
        [D2OIgnore]
        public String OpenFile
        {
            get { return openFile; }
            set { openFile = value; }
        }
        [D2OIgnore]
        public String CloseFile
        {
            get { return closeFile; }
            set { closeFile = value; }
        }
        [D2OIgnore]
        public List<SoundUiElement> SubElements
        {
            get { return subElements; }
            set { subElements = value; }
        }
    }
}