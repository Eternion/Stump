 


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
    [D2OClass("SpeakingItemsTriggers")]
    public class SpeakingItemsTriggerRecord : ID2ORecord
    {
        private const String MODULE = "SpeakingItemsTriggers";
        public int triggersId;
        public List<int> textIds;
        public List<int> states;

        [PrimaryKey("TriggersId", false)]
        public int TriggersId
        {
            get { return triggersId; }
            set { triggersId = value; }
        }

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
        public byte[] TextIdsBin
        {
            get { return m_textIdsBin; }
            set
            {
                m_textIdsBin = value;
                textIds = value == null ? null : value.ToObject<List<int>>();
            }
        }

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
        public byte[] StatesBin
        {
            get { return m_statesBin; }
            set
            {
                m_statesBin = value;
                states = value == null ? null : value.ToObject<List<int>>();
            }
        }

        public void AssignFields(object obj)
        {
            var castedObj = (SpeakingItemsTrigger)obj;
            
            TriggersId = castedObj.triggersId;
            TextIds = castedObj.textIds;
            States = castedObj.states;
        }
        
        public object CreateObject()
        {
            var obj = new SpeakingItemsTrigger();
            
            obj.triggersId = TriggersId;
            obj.textIds = TextIds;
            obj.states = States;
            return obj;
        
        }
    }
}