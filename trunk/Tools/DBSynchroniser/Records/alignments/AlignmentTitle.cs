 


// Generated on 10/06/2013 14:21:57
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("AlignmentTitles")]
    [D2OClass("AlignmentTitle")]
    public class AlignmentTitleRecord : ID2ORecord
    {
        private const String MODULE = "AlignmentTitles";
        public int sideId;
        public List<int> namesId;
        public List<int> shortsId;

        [PrimaryKey("Id")]
        public int Id
        {
            get;
            set;
        }
        public int SideId
        {
            get { return sideId; }
            set { sideId = value; }
        }

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
        public byte[] NamesIdBin
        {
            get { return m_namesIdBin; }
            set
            {
                m_namesIdBin = value;
                namesId = value == null ? null : value.ToObject<List<int>>();
            }
        }

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
        
        public virtual object CreateObject()
        {
            
            var obj = new AlignmentTitle();
            obj.sideId = SideId;
            obj.namesId = NamesId;
            obj.shortsId = ShortsId;
            return obj;
        
        }
    }
}