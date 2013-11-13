 


// Generated on 11/02/2013 14:55:46
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
    [TableName("AlignmentTitles")]
    [D2OClass("AlignmentTitle", "com.ankamagames.dofus.datacenter.alignments")]
    public class AlignmentTitleRecord : ID2ORecord, ISaveIntercepter
    {
        private const String MODULE = "AlignmentTitles";
        public int sideId;
        public List<int> namesId;
        public List<int> shortsId;

        int ID2ORecord.Id
        {
            get { return (int)sideId; }
        }


        [D2OIgnore]
        [PrimaryKey("SideId", false)]
        public int SideId
        {
            get { return sideId; }
            set { sideId = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<int> NamesId
        {
            get { return namesId; }
            set
            {
                namesId = value;
                m_namesIdBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_namesIdBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] NamesIdBin
        {
            get { return m_namesIdBin; }
            set
            {
                m_namesIdBin = value;
                namesId = value == null ? null : value.ToObject<List<int>>();
            }
        }

        [D2OIgnore]
        [Ignore]
        public List<int> ShortsId
        {
            get { return shortsId; }
            set
            {
                shortsId = value;
                m_shortsIdBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_shortsIdBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] ShortsIdBin
        {
            get { return m_shortsIdBin; }
            set
            {
                m_shortsIdBin = value;
                shortsId = value == null ? null : value.ToObject<List<int>>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (AlignmentTitle)obj;
            
            SideId = castedObj.sideId;
            NamesId = castedObj.namesId;
            ShortsId = castedObj.shortsId;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (AlignmentTitle)parent : new AlignmentTitle();
            obj.sideId = SideId;
            obj.namesId = NamesId;
            obj.shortsId = ShortsId;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_namesIdBin = namesId == null ? null : namesId.ToBinary();
            m_shortsIdBin = shortsId == null ? null : shortsId.ToBinary();
        
        }
    }
}