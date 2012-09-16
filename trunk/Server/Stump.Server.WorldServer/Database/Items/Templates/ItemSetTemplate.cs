using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Objects;
using System.IO;
using System.Linq;
using Castle.ActiveRecord;
using Stump.DofusProtocol.D2oClasses;
using Stump.DofusProtocol.D2oClasses.Tool;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Database
{
    public class ItemSetTemplateConfiguration : EntityTypeConfiguration<ItemSetTemplate>
    {
        public ItemSetTemplateConfiguration()
        {
            ToTable("items_sets");
            Ignore(x => x.Effects);
            Ignore(x => x.Items);
        }
    }

    [D2OClass("ItemSet", "com.ankamagames.dofus.datacenter.items")]
    public sealed class ItemSetTemplate : IAssignedByD2O, ISaveIntercepter
    {
        private string m_name;

        public uint Id
        {
            get;
            set;
        }

        private byte[] m_itemsBin;
        public byte[] ItemsBin
        {
            get { return m_itemsBin; }
            set
            {
                m_itemsBin = value; 

                if (value != null)
                    Items = DeserializeItems(value);
            }
        }

        public ItemTemplate[] Items
        {
            get;
            set;
        }

        public uint NameId
        {
            get;
            set;
        }

        public string Name
        {
            get { return m_name ?? (m_name = TextManager.Instance.GetText(NameId)); }
        }

        public Boolean BonusIsSecret
        {
            get;
            set;
        }

        private byte[] m_effectsBin;
        public byte[] EffectsBin
        {
            get
            {
                return m_effectsBin;
            }
            set
            {
                m_effectsBin = value;

                if (m_effectsBin != null)
                    Effects = DeserializeEffects(m_effectsBin);
            }
        }

        public List<List<EffectBase>> Effects
        {
            get;
            set;
        }

        public EffectBase[] GetEffects(int itemsCount)
        {
            int index = itemsCount - 1;

            if (Effects == null || Effects.Count <= index || index < 0)
                return new EffectBase[0];

            return Effects[index].ToArray();
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

        public void AssignFields(object d2oObject)
        {
            var itemSet = (DofusProtocol.D2oClasses.ItemSet)d2oObject;
            Id = itemSet.id;
            ItemsBin = SerializeItems(itemSet.items.Select(entry => (int)entry));
            NameId = itemSet.nameId;
            BonusIsSecret = itemSet.bonusIsSecret;
            var effects = itemSet.effects.Select(entry => entry.Where(subentry => subentry != null).
                Select(subentry => EffectManager.Instance.ConvertExportedEffect(subentry)).ToList()).ToList();
            EffectsBin = SerializeEffects(effects);
        }

        public void BeforeSave(ObjectStateEntry currentEntry)
        {
            EffectsBin = SerializeEffects(Effects);
            ItemsBin = SerializeItems(Items.Select(entry => entry.Id));
        }
    }
}