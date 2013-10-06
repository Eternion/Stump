 


// Generated on 10/06/2013 18:02:18
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [TableName("SpeakingItemsTriggers")]
    [D2OClass("SpeakingItemsTrigger", "com.ankamagames.dofus.datacenter.livingObjects")]
    public class SpeakingItemsTriggerRecord : ID2ORecord
    {
        int ID2ORecord.Id
        {
            get { return (int)Id; }
        }
        private const String MODULE = "SpeakingItemsTriggers";
        public int triggersId;
        public List<int> textIds;
        public List<int> states;

        [D2OIgnore]
        [PrimaryKey("Id")]
        public int Id
        {
            get;
            set;
        }
        [D2OIgnore]
        public int TriggersId
        {
            get { return triggersId; }
            set { triggersId = value; }
        }

        [D2OIgnore]
        [Ignore]
        public List<int> TextIds
        {
            get { return textIds; }
            set
            {
                textIds = value;
                m_textIdsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_textIdsBin;
        [D2OIgnore]
        public byte[] TextIdsBin
        {
            get { return m_textIdsBin; }
            set
            {
                m_textIdsBin = value;
                textIds = value == null ? null : value.ToObject<List<int>>();
            }
        }

        [D2OIgnore]
        [Ignore]
        public List<int> States
        {
            get { return states; }
            set
            {
                states = value;
                m_statesBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_statesBin;
        [D2OIgnore]
        public byte[] StatesBin
        {
            get { return m_statesBin; }
            set
            {
                m_statesBin = value;
                states = value == null ? null : value.ToObject<List<int>>();
            }
        }

        public virtual void AssignFields(object obj)
        {
            var castedObj = (SpeakingItemsTrigger)obj;
            
            TriggersId = castedObj.triggersId;
            TextIds = castedObj.textIds;
            States = castedObj.states;
        }
        
        public virtual object CreateObject(object parent = null)
        {
            
            var obj = parent != null ? (SpeakingItemsTrigger)parent : new SpeakingItemsTrigger();
            obj.triggersId = TriggersId;
            obj.textIds = TextIds;
            obj.states = States;
            return obj;
        
        }
    }
}