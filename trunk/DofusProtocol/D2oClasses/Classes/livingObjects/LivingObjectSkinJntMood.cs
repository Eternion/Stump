
// Generated on 03/25/2013 19:24:36
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("LivingObjectSkinJntMood")]
    [Serializable]
    public class LivingObjectSkinJntMood : IDataObject, IIndexedData
    {
        private const String MODULE = "LivingObjectSkinJntMood";
        public int skinId;
        public List<List<int>> moods;

        int IIndexedData.Id
        {
            get { return (int)skinId; }
        }

        public int SkinId
        {
            get { return skinId; }
            set { skinId = value; }
        }

        public List<List<int>> Moods
        {
            get { return moods; }
            set { moods = value; }
        }

    }
}