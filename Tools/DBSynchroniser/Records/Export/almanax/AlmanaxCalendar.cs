 


// Generated on 09/26/2016 01:50:39
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
    [TableName("AlmanaxCalendars")]
    [D2OClass("AlmanaxCalendar", "com.ankamagames.dofus.datacenter.almanax")]
    public class AlmanaxCalendarRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "AlmanaxCalendars";
        public int id;
        [I18NField]
        public uint nameId;
        [I18NField]
        public uint descId;
        public int npcId;
        public List<int> bonusesIds;

        int ID2ORecord.Id
        {
            get { return (int)id; }
        }


        [D2OIgnore]
        [PrimaryKey("Id", false)]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [D2OIgnore]
        [I18NField]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        [D2OIgnore]
        [I18NField]
        public uint DescId
        {
            get { return descId; }
            set { descId = value; }
        }

        [D2OIgnore]
        public int NpcId
        {
            get { return npcId; }
            set { npcId = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<int> BonusesIds
        {
            get { return bonusesIds; }
            set
            {
                bonusesIds = value;
                m_bonusesIdsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_bonusesIdsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] BonusesIdsBin
        {
            get { return m_bonusesIdsBin; }
            set
            {
                m_bonusesIdsBin = value;
                bonusesIds = value == null ? null : value.ToObject<List<int>>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (AlmanaxCalendar)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            DescId = castedObj.descId;
            NpcId = castedObj.npcId;
            BonusesIds = castedObj.bonusesIds;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (AlmanaxCalendar)parent : new AlmanaxCalendar();
            obj.id = Id;
            obj.nameId = NameId;
            obj.descId = DescId;
            obj.npcId = NpcId;
            obj.bonusesIds = BonusesIds;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_bonusesIdsBin = bonusesIds == null ? null : bonusesIds.ToBinary();
        
        }
    }
}