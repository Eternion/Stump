
// Generated on 03/25/2013 19:24:38
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SoundUi")]
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

        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public String UiName
        {
            get { return uiName; }
            set { uiName = value; }
        }

        public String OpenFile
        {
            get { return openFile; }
            set { openFile = value; }
        }

        public String CloseFile
        {
            get { return closeFile; }
            set { closeFile = value; }
        }

        public List<SoundUiElement> SubElements
        {
            get { return subElements; }
            set { subElements = value; }
        }

    }
}