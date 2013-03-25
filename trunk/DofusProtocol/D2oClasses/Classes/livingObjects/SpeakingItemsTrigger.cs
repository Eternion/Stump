
// Generated on 03/25/2013 19:24:36
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SpeakingItemsTriggers")]
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

        public int TriggersId
        {
            get { return triggersId; }
            set { triggersId = value; }
        }

        public List<int> TextIds
        {
            get { return textIds; }
            set { textIds = value; }
        }

        public List<int> States
        {
            get { return states; }
            set { states = value; }
        }

    }
}