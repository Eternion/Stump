
// Generated on 03/02/2013 21:17:46
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("SpeakingItemsText")]
    [Serializable]
    public class SpeakingItemText : IDataObject, IIndexedData
    {
        private const String MODULE = "SpeakingItemsText";
        public int textId;
        public float textProba;
        public uint textStringId;
        public int textLevel;
        public int textSound;
        public String textRestriction;

        int IIndexedData.Id
        {
            get { return (int)textId; }
        }

        public int TextId
        {
            get { return textId; }
            set { textId = value; }
        }

        public float TextProba
        {
            get { return textProba; }
            set { textProba = value; }
        }

        public uint TextStringId
        {
            get { return textStringId; }
            set { textStringId = value; }
        }

        public int TextLevel
        {
            get { return textLevel; }
            set { textLevel = value; }
        }

        public int TextSound
        {
            get { return textSound; }
            set { textSound = value; }
        }

        public String TextRestriction
        {
            get { return textRestriction; }
            set { textRestriction = value; }
        }

    }
}