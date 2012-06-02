using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Database.Items.Templates
{
    [Serializable]
    [ActiveRecord("items_set")]
    [D2OClass("ItemSet", "com.ankamagames.dofus.datacenter.items")]
    public sealed class ItemSetTemplate : WorldBaseRecord<ItemSetTemplate>, IAssignedByD2O
    {
        private string m_name;

        [D2OField("id")]
        [PrimaryKey(PrimaryKeyType.Assigned, "Id")]
        public uint Id
        {
            get;
            set;
        }

        private byte[] m_serializedItems;

        [D2OField("items")]
        [Property("Items")]
        private byte[] SerializedItems
        {
            get { return m_serializedItems; }
            set
            {
                m_serializedItems = value; 

                if (value != null)
                    Items = DeserializeItems(value);
            }
        }

        public ItemTemplate[] Items
        {
            get;
            set;
        }

        [D2OField("nameId")]
        [Property("NameId")]
        public uint NameId
        {
            get;
            set;
        }

        public string Name
        {
            get { return m_name ?? (m_name = TextManager.Instance.GetText(NameId)); }
        }

        [D2OField("bonusIsSecret")]
        [Property("BonusIsSecret")]
        public Boolean BonusIsSecret
        {
            get;
            set;
        }

        private byte[] m_serializedEffects;

        [D2OField("effects")]
        [Property("BonusEffects", NotNull = true)]
        private byte[] SerializedEffects
        {
            get
            {
                return m_serializedEffects;
            }
            set
            {
                m_serializedEffects = value;

                if (m_serializedEffects != null)
                    BonusEffects = DeserializeEffects(m_serializedEffects);
            }
        }

        public List<List<EffectBase>> BonusEffects
        {
            get;
            set;
        }

        public EffectBase[] GetEffects(int itemsCount)
        {
            int index = itemsCount - 1;

            if (BonusEffects == null || BonusEffects.Count <= index || index < 0)
                return new EffectBase[0];

            return BonusEffects[index].ToArray();
        }

        private static byte[] SerializeEffects(List<List<EffectBase>> bonusEffects)
        {
            var writer = new BinaryWriter(new MemoryStream());

            foreach (var effects in bonusEffects)
            {
                var data = EffectManager.Instance.SerializeEffects(effects);

                writer.Write(data.Length);
                writer.Write(data);
            }

            return  ( (MemoryStream) writer.BaseStream ).ToArray();
        }

        private static List<List<EffectBase>> DeserializeEffects(byte[] serialized)
        {
            var reader = new BinaryReader(new MemoryStream(serialized));
            var effects = new List<List<EffectBase>>();

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                var length = reader.ReadInt32();

                effects.Add(EffectManager.Instance.DeserializeEffects(reader.ReadBytes(length)));
            }

            return effects;
        }

        private byte[] SerializeItems(IEnumerable<int> templateIds)
        {
            var writer = new BinaryWriter(new MemoryStream());

            foreach (var id in templateIds)
            {
                writer.Write(id);
            }

            return ( (MemoryStream)writer.BaseStream ).ToArray();
        }

        private ItemTemplate[] DeserializeItems(byte[] serialized)
        {
            var reader = new BinaryReader(new MemoryStream(serialized));
            var templates = new List<ItemTemplate>();

            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                var id = reader.ReadInt32();

                var template = ItemManager.Instance.TryGetTemplate(id);

                if (template == null)
                    throw new Exception("Item template " + id + " not found");

                templates.Add(template);
            }

            return templates.ToArray();
        }

        protected override bool OnFlushDirty(object id, System.Collections.IDictionary previousState, System.Collections.IDictionary currentState, NHibernate.Type.IType[] types)
        {
            SerializedEffects = (byte[])(currentState["SerializedEffects"] = SerializeEffects(BonusEffects));
            SerializedItems = (byte[])( currentState["SerializedItems"] = SerializeItems(Items.Select(entry => entry.Id)) );

            return base.OnFlushDirty(id, previousState, currentState, types);
        }

        protected override bool BeforeSave(System.Collections.IDictionary state)
        {
            SerializedEffects = (byte[])( state["SerializedEffects"] = SerializeEffects(BonusEffects) );
            SerializedItems = (byte[])( state["SerializedItems"] = SerializeItems(Items.Select(entry => entry.Id)) );

            return base.BeforeSave(state);
        }

        public object GenerateAssignedObject(string fieldName, object d2OObject)
        {
            if (fieldName == "SerializedItems")
            {
                var list = d2OObject as List<uint>;

                if (list == null)
                    return new byte[0];

                return SerializeItems(list.Select(entry => (int)entry));
            }

            if (fieldName == "SerializedEffects")
            {
                var list = d2OObject as List<List<EffectInstance>>;

                if (list == null)
                    return new byte[0];

                var effects = list.Select(entry => entry.Where(subentry => subentry != null).Select(subentry => EffectManager.Instance.ConvertExportedEffect(subentry)).ToList()).ToList();

                return SerializeEffects(effects);
            }

            return d2OObject;
        }
    }
}