

// Generated on 10/28/2013 14:03:19
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SpeakingItemsTrigger", "com.ankamagames.dofus.datacenter.livingObjects")]
    [Serializable]
    public class SpeakingItemsTrigger : IDataObject, IIndexedData
    {
        private const String MODULE = "SpeakingItemsTriggers";
        public int triggersId;
        public List<int> textIds;
        public List<int> states;
        int IIndexedData.Id
        {
            get { return (int)triggersId; }
        }
        [D2OIgnore]
        public int TriggersId
        {
            get { return triggersId; }
            set { triggersId = value; }
        }
        [D2OIgnore]
        public List<int> TextIds
        {
            get { return textIds; }
            set { textIds = value; }
        }
        [D2OIgnore]
        public List<int> States
        {
            get { return states; }
            set { states = value; }
        }
    }
}