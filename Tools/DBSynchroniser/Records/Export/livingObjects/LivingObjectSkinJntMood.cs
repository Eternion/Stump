 


// Generated on 11/02/2013 14:55:49
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
    [TableName("LivingObjectSkinJntMood")]
    [D2OClass("LivingObjectSkinJntMood", "com.ankamagames.dofus.datacenter.livingObjects")]
    public class LivingObjectSkinJntMoodRecord : ID2ORecord, ISaveIntercepter
    {
        private const String MODULE = "LivingObjectSkinJntMood";
        public int skinId;
        public List<List<int>> moods;

        int ID2ORecord.Id
        {
            get { return (int)skinId; }
        }


        [D2OIgnore]
        [PrimaryKey("SkinId", false)]
        public int SkinId
        {
            get { return skinId; }
            set { skinId = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<List<int>> Moods
        {
            get { return moods; }
            set
            {
                moods = value;
                m_moodsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_moodsBin;
        [D2OIgnore]
        [BinaryField]
        [Browsable(false)]
        public byte[] MoodsBin
        {
            get { return m_moodsBin; }
            set
            {
                m_moodsBin = value;
                moods = value == null ? null : value.ToObject<List<List<int>>>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (LivingObjectSkinJntMood)obj;
            
            SkinId = castedObj.skinId;
            Moods = castedObj.moods;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            var obj = parent != null ? (LivingObjectSkinJntMood)parent : new LivingObjectSkinJntMood();
            obj.skinId = SkinId;
            obj.moods = Moods;
            return obj;
        }
        
        public virtual void BeforeSave(bool insert)
        {
            m_moodsBin = moods == null ? null : moods.ToBinary();
        
        }
    }
}