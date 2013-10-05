 


// Generated on 10/06/2013 01:10:57
using System;
using System.Collections.Generic;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;

namespace DBSynchroniser.Records
{
    [D2OClass("Emoticons")]
    public class EmoticonRecord : ID2ORecord
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

        [PrimaryKey("Id", false)]
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

        public Boolean Eight_directions
        {
            get { return eight_directions; }
            set { eight_directions = value; }
        }

        public Boolean Aura
        {
            get { return aura; }
            set { aura = value; }
        }

        [Ignore]
        public List<String> Anims
        {
            get { return anims; }
            set
            {
                anims = value;
                m_animsBin = value == null ? null : value.ToBinary();
            }
        }

        private byte[] m_animsBin;
        public byte[] AnimsBin
        {
            get { return m_animsBin; }
            set
            {
                m_animsBin = value;
                anims = value == null ? null : value.ToObject<List<String>>();
            }
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

        public void AssignFields(object obj)
        {
            var castedObj = (Emoticon)obj;
            
            Id = castedObj.id;
            NameId = castedObj.nameId;
            ShortcutId = castedObj.shortcutId;
            Order = castedObj.order;
            DefaultAnim = castedObj.defaultAnim;
            Persistancy = castedObj.persistancy;
            Eight_directions = castedObj.eight_directions;
            Aura = castedObj.aura;
            Anims = castedObj.anims;
            Cooldown = castedObj.cooldown;
            Duration = castedObj.duration;
            Weight = castedObj.weight;
        }
        
        public object CreateObject()
        {
            var obj = new Emoticon();
            
            obj.id = Id;
            obj.nameId = NameId;
            obj.shortcutId = ShortcutId;
            obj.order = Order;
            obj.defaultAnim = DefaultAnim;
            obj.persistancy = Persistancy;
            obj.eight_directions = Eight_directions;
            obj.aura = Aura;
            obj.anims = Anims;
            obj.cooldown = Cooldown;
            obj.duration = Duration;
            obj.weight = Weight;
            return obj;
        
        }
    }
}