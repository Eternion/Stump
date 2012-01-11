using System;

namespace Stump.Tools.MonsterDataFinder.Data
{ // ReSharper disable InconsistentNaming

    [Serializable]
    public class MonsterJSON
    {
        public Monster monster;
    }

    [Serializable]
    public class Monster
    {
        public string actionpoint;
        public int actionpointmax;
        public int actionpointmin;
        public int color1;
        public int color2;
        public int color3;
        public int display;
        public string dropslist;
        public int? gfxid;
        public string gold;
        public int goldmax;
        public int goldmin;
        public int id;
        public string lifepoints;
        public int lifepointsmax;
        public int lifepointsmin;
        public string lvl;
        public int lvlmax;
        public int lvlmin;
        public string movmentpoint;
        public int movmentpointmax;
        public int movmentpointmin;
        public string name;
        public int raceid;
        public string racename;
        public string racenameurl;
        public string reductionair;
        public int reductionairmax;
        public int reductionairmin;
        public string reductionearth;
        public int reductionearthmax;
        public int reductionearthmin;
        public string reductionfire;
        public int reductionfiremax;
        public int reductionfiremin;
        public string reductionneutral;
        public int reductionneutralmax;
        public int reductionneutralmin;
        public string reductionwater;
        public int reductionwatermax;
        public int reductionwatermin;
        public string spells;
        public int superraceid;
        public string superracename;
        public string superracenameurl;
    }

    // ReSharper restore InconsistentNaming
}