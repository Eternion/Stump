

// Generated on 12/12/2013 16:57:37
using System;
using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;

namespace Stump.DofusProtocol.D2oClasses
{
    [D2OClass("Emoticon", "com.ankamagames.dofus.datacenter.communication")]
    [Serializable]
    public class Emoticon : IDataObject, IIndexedData
    {
        public const String MODULE = "Emoticons";
        public uint id;
        [I18NField]
        public uint nameId;
        [I18NField]
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
        [D2OIgnore]
        public uint Id
        {
            get { return id; }
            set { id = value; }
        }
        [D2OIgnore]
        public uint NameId
        {
            get { return nameId; }
            set { nameId = value; }
        }
        [D2OIgnore]
        public uint ShortcutId
        {
            get { return shortcutId; }
            set { shortcutId = value; }
        }
        [D2OIgnore]
        public uint Order
        {
            get { return order; }
            set { order = value; }
        }
        [D2OIgnore]
        public String DefaultAnim
        {
            get { return defaultAnim; }
            set { defaultAnim = value; }
        }
        [D2OIgnore]
        public Boolean Persistancy
        {
            get { return persistancy; }
            set { persistancy = value; }
        }
        [D2OIgnore]
        public Boolean Eight_directions
        {
            get { return eight_directions; }
            set { eight_directions = value; }
        }
        [D2OIgnore]
        public Boolean Aura
        {
            get { return aura; }
            set { aura = value; }
        }
        [D2OIgnore]
        public List<String> Anims
        {
            get { return anims; }
            set { anims = value; }
        }
        [D2OIgnore]
        public uint Cooldown
        {
            get { return cooldown; }
            set { cooldown = value; }
        }
        [D2OIgnore]
        public uint Duration
        {
            get { return duration; }
            set { duration = value; }
        }
        [D2OIgnore]
        public uint Weight
        {
            get { return weight; }
            set { weight = value; }
        }
    }
}