using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack.Text;
using Stump.Core.IO;
using Stump.DofusProtocol.D2oClasses.Tools.D2o;
using Stump.DofusProtocol.D2oClasses;
using Stump.ORM;
using Stump.ORM.SubSonic.SQLGeneration.Schema;
using Stump.Server.WorldServer.Database.I18n;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Database.Items.Templates
{
    public class ItemSetTemplateRelator
    {
        public static string FetchQuery = "SELECT * FROM items_sets";
    }


    [TableName("items_sets")]
    [D2OClass("ItemSet", "com.ankamagames.dofus.datacenter.items")]
    public sealed class ItemSetTemplate : IAutoGeneratedRecord, IAssignedByD2O, ISaveIntercepter
    {
        private byte[] m_effectsBin;
        private string m_itemsCSV;
        private string m_name;

        [PrimaryKey("Id", false)]
        public uint Id
        {
            get;
            set;
        }

        public string ItemsCSV
        {
            get { return m_itemsCSV; }
            set
            {
                m_itemsCSV = value;

                if (value != null)
                    Items = DeserializeItems(value);
            }
        }

        [Ignore]
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

        public byte[] EffectsBin
        {
            get { return m_effectsBin; }
            set
            {
                m_effectsBin = value;

                if (m_effectsBin != null)
                    Effects = m_effectsBin.ToObject<List<List<EffectBase>>>();
            }
        }

        [Ignore]
        public List<List<EffectBase>> Effects
        {
            get;
            set;
        }

        #region IAssignedByD2O Members

        public void AssignFields(object d2oObject)
        {
            var itemSet = (ItemSet) d2oObject;
            Id = itemSet.id;
            ItemsCSV = SerializeItems(itemSet.items.Select(entry => (int) entry).ToArray());
            NameId = itemSet.nameId;
            BonusIsSecret = itemSet.bonusIsSecret;
            var effects = itemSet.effects.Select(entry => entry.Where(subentry => subentry != null).
                Select(subentry =>EffectManager.Instance.ConvertExportedEffect(subentry)).ToList()).ToList();
            EffectsBin = effects.ToBinary();
        }

        #endregion

        #region ISaveIntercepter Members

        public void BeforeSave(bool insert)
        {
            m_effectsBin = Effects.ToBinary();
            if (Items != null && Items.All(x => x != null))
                ItemsCSV = SerializeItems(Items.Select(entry => entry.Id).ToArray());
        }

        #endregion

        public EffectBase[] GetEffects(int itemsCount)
        {
            int index = itemsCount - 1;

            if (Effects == null || Effects.Count <= index || index < 0)
                return new EffectBase[0];

            return Effects[index].ToArray();
        }


        private string SerializeItems(int[] templateIds)
        {
            return templateIds.ToCSV(",");
        }

        private ItemTemplate[] DeserializeItems(string serialized)
        {
            int[] ids = serialized.FromCSV<int>(",");

            return ids.Select(x => ItemManager.Instance.TryGetTemplate(x)).ToArray();
        }
    }
}