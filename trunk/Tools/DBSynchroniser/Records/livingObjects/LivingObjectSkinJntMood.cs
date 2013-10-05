 


// Generated on 10/06/2013 01:10:59
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [D2OClass("LivingObjectSkinJntMood")]
    public class LivingObjectSkinJntMoodRecord : ID2ORecord
    {
        private const String MODULE = "LivingObjectSkinJntMood";
        public int skinId;
        public List<List<int>> moods;

        [PrimaryKey("SkinId", false)]
        public int SkinId
        {
            get { return skinId; }
            set { skinId = value; }
        }

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
        public byte[] MoodsBin
        {
            get { return m_moodsBin; }
            set
            {
                m_moodsBin = value;
                moods = value == null ? null : value.ToObject<List<List<int>>>();
            }
        }

        public void AssignFields(object obj)
        {
            var castedObj = (LivingObjectSkinJntMood)obj;
            
            SkinId = castedObj.skinId;
            Moods = castedObj.moods;
        }
        
        public object CreateObject()
        {
            var obj = new LivingObjectSkinJntMood();
            
            obj.skinId = SkinId;
            obj.moods = Moods;
            return obj;
        
        }
    }
}