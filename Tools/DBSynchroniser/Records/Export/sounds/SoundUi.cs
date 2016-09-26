 


// Generated on 09/26/2016 01:50:48
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("SoundUi")]
    [D2OClass("SoundUi", "com.ankamagames.dofus.datacenter.sounds")]
    public class SoundUiRecord : ID2ORecord, ISaveIntercepter
    {
        public String MODULE = "SoundUi";
        public uint id;
        public String uiName;
        public String openFile;
        public String closeFile;
        public List<SoundUiElement> subElements;

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        [NullString]
        public String UiName
        {
            get { return uiName; }
            set { uiName = value; }
        }

        [D2OIgnore]
        [NullString]
        public String OpenFile
        {
            get { return openFile; }
            set { openFile = value; }
        }

        [D2OIgnore]
        [NullString]
        public String CloseFile
        {
            get { return closeFile; }
            set { closeFile = value; }
        }

        [D2OIgnore]
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
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] SubElementsBin
        {
            get { return m_subElementsBin; }
            set
            {
                m_subElementsBin = value;
                subElements = value == null ? null : value.ToObject<List<SoundUiElement>>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (SoundUi)obj;
            
            Id = castedObj.id;
            UiName = castedObj.uiName;
            OpenFile = castedObj.openFile;
            CloseFile = castedObj.closeFile;
            SubElements = castedObj.subElements;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (SoundUi)parent : new SoundUi();
            obj.id = Id;
            obj.uiName = UiName;
            obj.openFile = OpenFile;
            obj.closeFile = CloseFile;
            obj.subElements = SubElements;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_subElementsBin = subElements == null ? null : subElements.ToBinary();
        
        }
    }
}