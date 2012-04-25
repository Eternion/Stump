using System.Collections.Generic;
using System.ComponentModel;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Server.WorldServer.Game.Effects.Instances;

namespace Stump.Tools.QuickItemEditor.Models
{
    public class ItemTemplateModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler evnt = PropertyChanged;
            if (evnt != null)
                evnt(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly ItemTemplate m_template;
        private readonly string m_name;

        public ItemTemplateModel(ItemTemplate template, string name)
        {
            m_template = template;
            m_name = name;
        }

        public ItemTemplate Template
        {
            get { return m_template; }
        }

        public int Id
        {
            get { return Template.Id; }
        }

        public uint Weight
        {
            get { return Template.Weight; }
            set
            {
                Template.Weight = value;
                OnPropertyChanged("Weight");
            }
        }

        public string Name
        {
            get { return m_name; }
        }

        public uint Level
        {
            get { return Template.Level; }
            set
            {
                Template.Level = value;
                OnPropertyChanged("Level");
            }
        }

        public float Price
        {
            get { return Template.Price; }
            set
            {
                Template.Price = value;
                OnPropertyChanged("Price");
            }
        }

        public bool Usable
        {
            get { return Template.Usable; }
            set
            {
                Template.Usable = value;
                OnPropertyChanged("Usable");
            }
        }

        public string Criteria
        {
            get { return Template.Criteria; }
            set
            {
                Template.Criteria = value;
                OnPropertyChanged("Criteria");
            }
        }

        public uint AppearanceId
        {
            get { return Template.AppearanceId; }
            set
            {
                Template.AppearanceId = value;
                OnPropertyChanged("AppearanceId");
            }
        }

        public List<EffectBase> Effects
        {
            get { return Template.Effects; }
            set { Template.Effects = value; }
        }
    }
}