 


// Generated on 10/06/2013 01:11:00
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [D2OClass("SoundUi")]
    public class SoundUiRecord : ID2ORecord
    {
        public uint id;
        public String uiName;
        public String openFile;
        public String closeFile;
        public List<SoundUiElement> subElements;
        public String MODULE = "SoundUi";

        [PrimaryKey("Id", false)]
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

        [Ignore]
        public List<SoundUiElement> SubElements
        {
            get { return subElements; }
            set
            {
                subElements = value;
                m_subElementsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_subElementsBin;
        public byte[] SubElementsBin
        {
            get { return m_subElementsBin; }
            set
            {
                m_subElementsBin = value;
                subElements = value == null ? null : value.ToObject<List<SoundUiElement>>();
            }
        }

        public void AssignFields(object obj)
        {
            var castedObj = (SoundUi)obj;
            
            Id = castedObj.id;
            UiName = castedObj.uiName;
            OpenFile = castedObj.openFile;
            CloseFile = castedObj.closeFile;
            SubElements = castedObj.subElements;
        }
        
        public object CreateObject()
        {
            var obj = new SoundUi();
            
            obj.id = Id;
            obj.uiName = UiName;
            obj.openFile = OpenFile;
            obj.closeFile = CloseFile;
            obj.subElements = SubElements;
            return obj;
        
        }
    }
}