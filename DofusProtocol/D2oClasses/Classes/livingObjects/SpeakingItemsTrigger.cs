

// Generated on 12/29/2014 16:35:15
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
        public const String MODULE = "SpeakingItemsTriggers";
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
            get { return this.triggersId; }
            set { this.triggersId = value; }
        }
        [D2OIgnore]
        public List<int> TextIds
        {
            get { return this.textIds; }
            set { this.textIds = value; }
        }
        [D2OIgnore]
        public List<int> States
        {
            get { return this.states; }
            set { this.states = value; }
        }
    }
}