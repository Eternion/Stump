 


// Generated on 11/16/2015 14:26:41
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
    [TableName("MonsterRaces")]
    [D2OClass("MonsterRace", "com.ankamagames.dofus.datacenter.monsters")]
    public class MonsterRaceRecord : ID2ORecord, ISaveIntercepter
    {
        public const String MODULE = "MonsterRaces";
        public int id;
        public int superRaceId;
        [I18NField]
        public uint nameId;
        public List<uint> monsters;

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
        public int SuperRaceId
        {
            get { return superRaceId; }
            set { superRaceId = value; }
        }

        [D2OIgnore]
        [I18NField]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<uint> Monsters
        {
            get { return monsters; }
            set
            {
                monsters = value;
                m_monstersBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_monstersBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] MonstersBin
        {
            get { return m_monstersBin; }
            set
            {
                m_monstersBin = value;
                monsters = value == null ? null : value.ToObject<List<uint>>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (MonsterRace)obj;
            
            Id = castedObj.id;
            SuperRaceId = castedObj.superRaceId;
            NameId = castedObj.nameId;
            Monsters = castedObj.monsters;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (MonsterRace)parent : new MonsterRace();
            obj.id = Id;
            obj.superRaceId = SuperRaceId;
            obj.nameId = NameId;
            obj.monsters = Monsters;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_monstersBin = monsters == null ? null : monsters.ToBinary();
        
        }
    }
}