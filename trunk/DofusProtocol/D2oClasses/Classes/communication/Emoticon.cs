
// Generated on 03/02/2013 21:17:44
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Emoticons")]
    [Serializable]
    public class Emoticon : IDataObject, IIndexedData
    {
        private const String MODULE = "Emoticons";
        public uint id;
        public uint nameId;
        public uint shortcutId;
        public uint order;
        public String defaultAnim;
        public Boolean persistancy;
        public Boolean eight_directions;
        public Boolean aura;
        public List<String> anims;
        public uint cooldown = 1000;
        public uint duration = 0;
        public uint weight;

        int IIndexedData.Id
        {
            get { return (int)id; }
        }

        public uint Id
        {
            get { return id; }
            set { id = value; }
        }

        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }

        public uint ShortcutId
        {
            get { return shortcutId; }
            set { shortcutId = value; }
        }

        public uint Order
        {
            get { return order; }
            set { order = value; }
        }

        public String DefaultAnim
        {
            get { return defaultAnim; }
            set { defaultAnim = value; }
        }

        public Boolean Persistancy
        {
            get { return persistancy; }
            set { persistancy = value; }
        }

        public Boolean Eightdirections
        {
            get { return eight_directions; }
            set { eight_directions = value; }
        }

        public Boolean Aura
        {
            get { return aura; }
            set { aura = value; }
        }

        public List<String> Anims
        {
            get { return anims; }
            set { anims = value; }
        }

        public uint Cooldown
        {
            get { return cooldown; }
            set { cooldown = value; }
        }

        public uint Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        public uint Weight
        {
            get { return weight; }
            set { weight = value; }
        }

    }
}